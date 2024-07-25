using ExpenseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseAPI.Repostorys
{
    /// <summary>
    /// 代表資料庫的連線並管理Expense實體。
    /// </summary>
    public class ExpenseContext : DbContext
    {
        /// <summary>
        /// 初始化 <see cref="ExpenseContext"/> 類別的新實例。
        /// </summary>
        /// <param name="options">配置DbContext的選項。</param>
        public ExpenseContext(DbContextOptions<ExpenseContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// 代表Expense實體的集合。
        /// </summary>
        public DbSet<Expense> Expenses { get; set; }
    }



    public class Startup
    {
        /// <summary>
        /// 配置應用程式所需的服務。
        /// </summary>
        /// <param name="services">定義了一系列應用程式服務的集合。</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // 註冊ExpenseContext使用In-Memory資料庫
            services.AddDbContext<ExpenseContext>(options =>
                options.UseInMemoryDatabase("ExpensesDatabase"));
        }
    }
}
