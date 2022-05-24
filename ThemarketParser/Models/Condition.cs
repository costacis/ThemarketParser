using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ThemarketParser.Data;

namespace ThemarketParser.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class Condition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        [JsonProperty("translations.ru")]
        public string trnslation { get; set; }
        public virtual ICollection<Item> items { get; set; }
        public Condition() { items = new List<Item>(); }
    }
}
