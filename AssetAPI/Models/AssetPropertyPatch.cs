using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetAPI.Models
{
    public class AssetPropertyPatch
    {              
        
        [StringLength(10)]
        public string name { get; set; }
        
        public bool value { get; set; }

        [Required]
        public DateTime time_stamp { get; set; }
    }
}
