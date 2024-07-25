// 這是一個用來建立預設資料的類別


using ExpenseAPI.Repostorys;
using Microsoft.EntityFrameworkCore;

namespace ExpenseAPI.Models
{
    public static class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ExpenseContext(
                serviceProvider.GetRequiredService<DbContextOptions<ExpenseContext>>()))
            {
                // 檢查資料庫是否已經有資料
                if (context.Expenses.Any())
                {
                    return;   // 資料庫已經被初始化
                }

                context.Expenses.AddRange(
                    new Expense
                    {
                        Title = "範例支出1",
                        Amount = 100.0m,
                        CreateDateTime = DateTime.Now,
                        Category = "食"
                    },
                    new Expense
                    {
                        Title = "範例支出2",
                        Amount = 200.0m,
                        CreateDateTime = DateTime.Now.AddDays(-1),
                        Category = "行"
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
