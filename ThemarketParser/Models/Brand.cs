using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThemarketParser.Models
{
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string id { get; set; }
        [Required]
        public string name { get; set; }
        public virtual ICollection<Item> items { get; set; }
        public Brand() { items = new List<Item>(); }
    }
}
