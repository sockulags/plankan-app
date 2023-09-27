using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models
{
    public class Transaction
    {

        
        [Key] 
        public int TransactionId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Välj en kategori.")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int Amount { get; set; }
       
        public string? Note { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [NotMapped]
        public string? CategoryTitleWithIcon
        {
            get
            {
                return Category == null? "": $"{Category.Icon} {Category.Title}";
            }
        }

        public string? FormattedAmount
        {
            get
            {
                return ((Category == null || Category.Type == "Expense")? "- " : "+ ") + Amount.ToString("C0");
            }
        }

       
    }
}
