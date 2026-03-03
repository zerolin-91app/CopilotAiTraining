using ExpenseAPI.Models;
using ExpenseAPI.Repostorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseAPI.Controllers
{
    /// <summary>
    /// 處理與支出相關的HTTP請求。
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private const decimal MaxAmount = 1000m;
        private const int MaxTitleLength = 100;
        private const int MaxCategoryLength = 50;
        private static readonly HashSet<string> ValidCategories = ["食", "衣", "住", "行"];

        private readonly ExpenseContext _context;

        /// <summary>
        /// 初始化 <see cref="ExpenseController"/> 類的新實例。
        /// </summary>
        /// <param name="context">提供對資料庫操作的能力。</param>
        public ExpenseController(ExpenseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 獲取所有支出項目。
        /// </summary>
        /// <summary>
        /// Retrieves all expense records from the database.
        /// </summary>
        /// <returns>An IActionResult containing the list of expense records (HTTP 200 OK).</returns>
        [HttpGet]
        public async Task<IActionResult> GetExpenses()
        {
            var expenses = await _context.Expenses
                .AsNoTracking()
                .ToListAsync();

            return Ok(expenses);
        }

        /// <summary>
        /// 創建一個新的支出項目。
        /// </summary>
        /// <param name="expense">要創建的支出項目。</param>
        /// <returns>創建結果。</returns>
        [HttpPost]
        public async Task<IActionResult> CreateExpense(Expense expense)
        {
            var validationResult = ValidateExpense(expense);
            if (!string.IsNullOrEmpty(validationResult))
            {
                return BadRequest(validationResult);
            }

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetExpenses), new { id = expense.Id }, expense);
        }

        /// <summary>
        /// 更新指定ID的支出項目。
        /// </summary>
        /// <param name="id">要更新的支出項目ID。</param>
        /// <param name="expense">更新後的支出項目。</param>
        /// <summary>
        /// Updates an existing expense identified by the route id with the provided expense data.
        /// </summary>
        /// <param name="id">The id of the expense to update; must match expense.Id.</param>
        /// <param name="expense">The expense payload containing updated values.</param>
        /// <returns>An IActionResult representing the HTTP response: 200 OK with a success message when updated; 400 BadRequest if the id does not match or validation fails; 404 NotFound if the expense does not exist.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest("ID指定不一致");
            }

            var validationResult = ValidateExpense(expense);
            if (!string.IsNullOrEmpty(validationResult))
            {
                return BadRequest(validationResult);
            }

            var existingExpense = await _context.Expenses.FindAsync(expense.Id);
            if (existingExpense == null)
            {
                return NotFound("找不到指定的支出");
            }

            existingExpense.Title = expense.Title;
            existingExpense.Amount = expense.Amount;
            existingExpense.CreateDateTime = expense.CreateDateTime;
            existingExpense.Category = expense.Category;

            await _context.SaveChangesAsync();

            return Ok(new { message = "支出更新成功" });
        }

        /// <summary>
        /// 刪除指定ID的支出項目。
        /// </summary>
        /// <param name="id">要刪除的支出項目ID。</param>
        /// <returns>刪除結果。</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound("找不到指定的支出");
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return Ok(new { message = "支出刪除成功" });
        }

        /// <summary>
        /// 驗證支出項目的資料。
        /// </summary>
        /// <param name="expense">要驗證的支出項目。</param>
        /// <summary>
        /// Validates the provided Expense and reports the first validation failure.
        /// </summary>
        /// <param name="expense">The expense to validate.</param>
        /// <returns>An empty string if validation passes; otherwise a user-facing error message (in Chinese) describing the first failed rule.</returns>
        private string ValidateExpense(Expense expense)
        {
            if (expense.Amount < 0)
            {
                return "金額不能為負數";
            }

            if (!ValidCategories.Contains(expense.Category))
            {
                return "分類只能為[食、衣、住、行]";
            }

            if (expense.CreateDateTime > DateTime.Now.AddYears(-1))
            {
                return "發生日期不能晚於 1 年前";
            }

            if (expense.Title.Length > MaxTitleLength)
            {
                return "標題不能超過 100 個字元";
            }

            if (expense.Amount > MaxAmount)
            {
                return "金額不能超過 1000";
            }

            if (expense.Category.Length > MaxCategoryLength)
            {
                return "分類不能超過 50 個字元";
            }

            return string.Empty;
        }
    }
}
