using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations;

namespace ASM.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
