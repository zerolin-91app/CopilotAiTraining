using ExpenseAPI.Controllers;
using ExpenseAPI.Repostorys;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ExpenseAPI.Models;

namespace ExpenseAPITest
{
    /// <summary>
    /// ExpenseController 單元測試。
    /// </summary>
    [TestClass]
    public class ExpenseControllerTest
    {
        private ExpenseContext CreateDbContext()
        {
            var databaseName = $"TestDatabase_{Guid.NewGuid()}";
            var options = new DbContextOptionsBuilder<ExpenseContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;
            var dbContext = new ExpenseContext(options);
            return dbContext;
        }

        [TestMethod]
        public async Task TestGet_ShouldReturnOkWithExpenses()
        {
            // Arrange
            var context = CreateDbContext();
            context.Expenses.Add(new Expense
            {
                Title = "TestGet",
                CreateDateTime = DateTime.Now.AddYears(-1),
                Amount = 100,
                Category = "食"
            });
            await context.SaveChangesAsync();

            var controller = new ExpenseController(context);

            // Act
            var result = await controller.GetExpenses() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            var expenses = result.Value as IEnumerable<Expense>;
            Assert.IsNotNull(expenses);
            Assert.AreEqual(1, expenses.Count());
        }

        [TestMethod]
        public async Task TestPost_ShouldCreateExpense()
        {
            // Arrange
            var context = CreateDbContext();
            var controller = new ExpenseController(context);
            var expense = new Expense
            {
                Id = 1,
                Title = "TestPost",
                CreateDateTime = DateTime.Now.AddYears(-1),
                Amount = 100,
                Category = "食"
            };

            // Act
            var result = await controller.CreateExpense(expense) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, await context.Expenses.CountAsync());
        }

        [TestMethod]
        public async Task TestUpdate_ShouldUpdateExpense()
        {
            // Arrange
            var context = CreateDbContext();
            var controller = new ExpenseController(context);
            var expense = new Expense
            {
                Id = 1,
                Title = "TestUpdate",
                CreateDateTime = DateTime.Now.AddYears(-1),
                Amount = 100,
                Category = "食"
            };
            context.Expenses.Add(expense);
            await context.SaveChangesAsync();

            // Act
            expense.Amount = 200;
            var result = await controller.UpdateExpense(1, expense) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            var updated = await context.Expenses.FindAsync(1);
            Assert.IsNotNull(updated);
            Assert.AreEqual(200, updated.Amount);
        }

        [TestMethod]
        public async Task TestDelete_ShouldRemoveExpense()
        {
            // Arrange
            var context = CreateDbContext();
            var controller = new ExpenseController(context);
            var expense = new Expense
            {
                Id = 1,
                Title = "TestDelete",
                CreateDateTime = DateTime.Now.AddYears(-1),
                Amount = 100,
                Category = "食"
            };
            context.Expenses.Add(expense);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteExpense(1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            var deletedExpense = await context.Expenses.FindAsync(1);
            Assert.IsNull(deletedExpense);
        }

        [TestMethod]
        public async Task TestPost_InvalidCategory_ShouldReturnBadRequest()
        {
            // Arrange
            var context = CreateDbContext();
            var controller = new ExpenseController(context);
            var expense = new Expense
            {
                Title = "InvalidCategory",
                CreateDateTime = DateTime.Now.AddYears(-1),
                Amount = 100,
                Category = "其他"
            };

            // Act
            var result = await controller.CreateExpense(expense) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("分類只能為[食、衣、住、行]", result.Value);
        }

        [TestMethod]
        public async Task TestUpdate_IdMismatch_ShouldReturnBadRequest()
        {
            // Arrange
            var context = CreateDbContext();
            var controller = new ExpenseController(context);
            var expense = new Expense
            {
                Id = 2,
                Title = "Mismatch",
                CreateDateTime = DateTime.Now.AddYears(-1),
                Amount = 100,
                Category = "食"
            };

            // Act
            var result = await controller.UpdateExpense(1, expense) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ID指定不一致", result.Value);
        }
    }
}