using ExpenseAPI.Controllers;
using ExpenseAPI.Repostorys;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ExpenseAPI.Models;

namespace ExpenseAPITest
{
    /// <summary>
    /// // 這是用來測試ExpenseController的單元測試
    /// </summary>
    [TestClass]
    public class ExpenseControllerTest
    {
        private ExpenseContext CreateDbContext()
        {
            var databaseName = $"TestDatabase_{Guid.NewGuid()}"; // 確保每次調用都使用唯一的數據庫名稱
            var options = new DbContextOptionsBuilder<ExpenseContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;
            var dbContext = new ExpenseContext(options);
            return dbContext;
        }

        //測試Get方法
        [TestMethod]
        public void TestGet()
        {
            // Arrange
            var context = CreateDbContext();
            var controller = new ExpenseController(context);

            // Act
            var result = controller.GetExpenses().Result as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        //測試Post方法
        [TestMethod]
        public void TestPost()
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
                Category = "住"
            };

            // Act
            var result = controller.CreateExpense(expense).Result as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
        }

        //測試Put方法
        [TestMethod]
        public void TestUpdate()
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
                Category = "住"
            };
            context.Expenses.Add(expense);
            context.SaveChanges(); // 確保數據被插入到內存數據庫中

            // Act
            expense.Amount = 200; // 修改數據
            var result = controller.UpdateExpense(1, expense).Result as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
        }

        //測試Delete方法
        [TestMethod]
        public void TestDelete()
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
                Category = "住"
            };
            context.Expenses.Add(expense);
            context.SaveChanges(); // 確保數據被插入到內存數據庫中

            // Act
            var result = controller.DeleteExpense(1).Result as OkObjectResult;
            
            // Assert
            Assert.IsNotNull(result);
            var deletedExpense = context.Expenses.Find(1);
            Assert.IsNull(deletedExpense);

        }



    }
}