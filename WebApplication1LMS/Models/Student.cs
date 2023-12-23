using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1LMS.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string? Address { get; set; }
        public string? ContactNo { get; set; }
       public List<Book> Books { get; set; }=new List<Book>();

    }
}
