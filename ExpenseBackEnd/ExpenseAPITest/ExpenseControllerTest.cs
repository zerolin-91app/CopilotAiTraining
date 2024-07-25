using ExpenseAPI.Controllers;
using ExpenseAPI.Repostorys;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ExpenseAPI.Models;

namespace ExpenseAPITest
{
    /// <summary>
    /// // �o�O�ΨӴ���ExpenseController���椸����
    /// </summary>
    [TestClass]
    public class ExpenseControllerTest
    {
        private ExpenseContext CreateDbContext()
        {
            var databaseName = $"TestDatabase_{Guid.NewGuid()}"; // �T�O�C���եγ��ϥΰߤ@���ƾڮw�W��
            var options = new DbContextOptionsBuilder<ExpenseContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;
            var dbContext = new ExpenseContext(options);
            return dbContext;
        }

        //����Get��k
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

        //����Post��k
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
                Category = "��"
            };

            // Act
            var result = controller.CreateExpense(expense).Result as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
        }

        //����Put��k
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
                Category = "��"
            };
            context.Expenses.Add(expense);
            context.SaveChanges(); // �T�O�ƾڳQ���J�줺�s�ƾڮw��

            // Act
            expense.Amount = 200; // �ק�ƾ�
            var result = controller.UpdateExpense(1, expense).Result as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
        }

        //����Delete��k
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
                Category = "��"
            };
            context.Expenses.Add(expense);
            context.SaveChanges(); // �T�O�ƾڳQ���J�줺�s�ƾڮw��

            // Act
            var result = controller.DeleteExpense(1).Result as OkObjectResult;
            
            // Assert
            Assert.IsNotNull(result);
            var deletedExpense = context.Expenses.Find(1);
            Assert.IsNull(deletedExpense);

        }



    }
}