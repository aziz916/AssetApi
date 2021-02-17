using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AssetAPI.Models
{
    public class AssetProperty
    {


        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public long id { get; set; }

        [Required]
        [JsonIgnore]
        public Asset asset { get; set; }

        [Required]
        [StringLength(10)]
        public string name { get; set; }

        [Required]
        public bool value { get; set; }


        [Required]
        public DateTime time_stamp { get; set; }
    }
}
