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
}
