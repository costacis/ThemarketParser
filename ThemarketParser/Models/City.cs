using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThemarketParser.Models
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        [Required]
        public string title { get; set; }

        public virtual ICollection<Item> items { get; set; }
        public City() { items = new List<Item>(); }
    }
}
