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
        private static readonly HashSet<string> AllowedCategories = new(StringComparer.Ordinal)
        {
            "食",
            "衣",
            "住",
            "行"
        };

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
        /// <returns>支出項目列表。</returns>
        [HttpGet]
        public async Task<IActionResult> GetExpenses()
        {
            //// TODO，測試用，提交PR前記得要移除
            var test = "ABC";
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
            if (!TryValidateExpense(expense, out var badRequestResult))
            {
                return badRequestResult;
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
        /// <returns>更新結果。</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, Expense expense)
        {
            if (expense is null)
            {
                return BadRequest("支出資料不可為空");
            }

            if (id != expense.Id)
            {
                return BadRequest("ID指定不一致");
            }

            if (!TryValidateExpense(expense, out var badRequestResult))
            {
                return badRequestResult;
            }

            var existingExpense = await _context.Expenses.FindAsync(id);
            if (existingExpense == null)
            {
                return NotFound("找不到指定的支出");
            }

            _context.Entry(existingExpense).CurrentValues.SetValues(expense);
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
        /// <param name="badRequestResult">若驗證失敗，回傳對應的 BadRequest。</param>
        /// <returns>驗證是否通過。</returns>
        private bool TryValidateExpense(Expense? expense, out IActionResult? badRequestResult)
        {
            badRequestResult = null;

            if (expense is null)
            {
                badRequestResult = BadRequest("支出資料不可為空");
                return false;
            }

            if (expense.Amount < 0)
            {
                badRequestResult = BadRequest("金額不能為負數");
                return false;
            }

            if (string.IsNullOrWhiteSpace(expense.Category) || !AllowedCategories.Contains(expense.Category))
            {
                badRequestResult = BadRequest("分類只能為[食、衣、住、行]");
                return false;
            }

            if (expense.CreateDateTime > DateTime.Now.AddYears(-1))
            {
                badRequestResult = BadRequest("發生日期不能晚於 1 年前");
                return false;
            }

            if (string.IsNullOrWhiteSpace(expense.Title))
            {
                badRequestResult = BadRequest("標題不可為空");
                return false;
            }

            if (expense.Title.Length > 100)
            {
                badRequestResult = BadRequest("標題不能超過 100 個字元");
                return false;
            }

            if (expense.Amount > 1000)
            {
                badRequestResult = BadRequest("金額不能超過 1000");
                return false;
            }

            if (expense.Category.Length > 50)
            {
                badRequestResult = BadRequest("分類不能超過 50 個字元");
                return false;
            }

            return true;
        }
    }
}
