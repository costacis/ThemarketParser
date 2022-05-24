using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ThemarketParser.Data;

namespace ThemarketParser.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public abstract class CategoryAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        [Required]
        [JsonProperty("translations.ru")]
        public string name { get; set; }
        public int? parentCategoryId { get; set; }
        public CategoryAbstract? parentCategory { get; set; }
        public virtual ICollection<CategoryAbstract>? childCategories { get; set; }
        public virtual ICollection<Item> items { get; set; }

        public CategoryAbstract() {
            childCategories = new List<CategoryAbstract>();
            items = new List<Item>(); 
        }

    }

    public class SexCategory: CategoryAbstract { }
    public class Category : CategoryAbstract { }
    public class ConcreteCategory : CategoryAbstract {
        [Required]
        [JsonProperty("sizes")]
        public string sizesName { get; set; }
        public virtual ICollection<Size> size { get; set; }
        public ConcreteCategory()
        {
            size = new List<Size>();
        }
    }
}
