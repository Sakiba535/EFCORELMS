using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1LMS.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string BookName { get; set; }
        public string AuthorName {  get; set; }
        public decimal RentPrice {  get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BorrowingDate { get; set; } = DateTime.Now;

        public int? StudentId { get; set; }
        public  Student? Student { get; set; }
    }
}
