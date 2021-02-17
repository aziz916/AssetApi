using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AssetAPI.Models
{
    public class Asset
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public long id { get; set; }

        [Required]
        [StringLength(20)]
        public string name { get; set; }

        [JsonIgnore]
        public ICollection<AssetProperty> properties { get; set; }
    }
}
