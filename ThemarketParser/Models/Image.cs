using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ThemarketParser.Data;

namespace ThemarketParser.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string id { get; set; }
        [Required]
        [JsonProperty("urls.670")]
        public string url { get; set; }
        public string itemId { get; set; }
        [Required]
        public Item item { get; set; }
    }
}
