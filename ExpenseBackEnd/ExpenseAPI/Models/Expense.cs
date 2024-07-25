// 這個是用來代表一筆支出的 Entity
// 這個 Entity 會對應到資料庫中的一個 Table
// 他是一筆支出要，需要包含：標題，金額，發生日期和時間，分類
using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseAPI.Models
{
    /// <summary>
    /// 代表一筆支出的 Entity，對應到資料庫中的一個 Table。
    /// 包含標題、金額、發生日期和時間、分類等屬性。
    /// </summary>
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 支出的標題。
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// 支出的金額。
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        /// <summary>
        /// 支出發生的日期和時間。
        /// </summary>
        [Required]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 支出的分類。
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Category { get; set; }
    }
}


