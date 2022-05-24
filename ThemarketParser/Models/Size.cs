using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThemarketParser.Models
{
    public class Size
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string? eur { get; set; }
        [Required]
        public string us { get; set; }
        public int categoryId { get; set; }
        [Required]
        public virtual Category category { get; set; }
        public virtual ICollection<Item> items { get; set; }
        public Size() { items = new List<Item>(); }
    }
}
