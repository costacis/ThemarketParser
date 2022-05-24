using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThemarketParser.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string id { get; set; }
        [Required]
        public string userID { get; set; }
        [Required]
        public string model { get; set; }
        [Required]
        public string description { get; set; }
        public int price { get; set; }
        public int status { get; set; }
        public DateTime addedAt { get; set; }
        [Required]
        public string prettyPath { get; set; }
        public int likesCount { get; set; }
        public int deliveryPrice { get; set; }
        public int sexCategoryId { get; set; }
        [Required]
        [JsonIgnore]
        public virtual SexCategory sexCategory { get; set; }
        public int categoryId { get; set; }
        [Required]
        [JsonIgnore]
        public virtual Category category { get; set; }
        public int concreteCategoryId { get; set; }
        [Required]
        [JsonIgnore]
        public virtual ConcreteCategory concreteCategory { get; set; }
        public int sizeId { get; set; }
        [Required]
        [JsonIgnore]
        public virtual Size size { get; set; }
        public int conditionId { get; set; }
        [Required]
        [JsonIgnore]
        public virtual Condition condition { get; set; }
        public int cityId { get; set; }
        [Required]
        [JsonIgnore]
        public virtual City city { get; set; }

        public virtual IList<Brand> brands { get; set; }
        [JsonIgnore]
        public virtual IList<Image> images { get; set; }
        public bool isModified { get; set; }

        public Item() { 
            brands = new List<Brand>(); 
            images = new List<Image>();
            isModified = false;
        }

    }
}
