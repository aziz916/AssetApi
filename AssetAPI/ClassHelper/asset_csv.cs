using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetAPI.ClassHelper
{
    public class asset_csv
    {
        public long assetid { get; set; }
        public string properties { get; set; }
        public bool value { get; set; }
        public DateTime time_stamp { get; set; }
    }
}
