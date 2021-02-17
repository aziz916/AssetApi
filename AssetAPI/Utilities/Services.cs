using CsvHelper;
using AssetAPI.ClassHelper;
using AssetAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AssetAPI.Utilities
{
    public class Services
    {
        public AssetDbContext _Db;

        public Services(AssetDbContext db)
        {
            _Db = db;
        }
        public List<asset_csv> ReadCSVFile(string location)
        {
            try
            {
                using (var reader = new StreamReader(location, Encoding.Default))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                    {

                        var records = csv.GetRecords<asset_csv>().ToList();

                        //Grouping the records which has the same properties with the higher timestamp
                        var r = records.GroupBy(x => new { x.assetid, x.properties, x.value })
                            .Select(i => new asset_csv()
                            {
                                assetid = i.Key.assetid,
                                properties = i.Key.properties,
                                value = i.Key.value,
                                time_stamp = i.Max(xs => xs.time_stamp)
                            }).ToList();


                        return r;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<asset_csv> ReadCSVFileUpload(IFormFile file)
        {
            try
            {
                using (var reader = new StreamReader(file.OpenReadStream(), Encoding.Default))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                    {

                        var records = csv.GetRecords<asset_csv>().ToList();

                        //Grouping the records which has the same properties with the higher timestamp
                        var r = records.GroupBy(x => new { x.assetid, x.properties, x.value })
                            .Select(i => new asset_csv()
                            {
                                assetid = i.Key.assetid,
                                properties = i.Key.properties,
                                value = i.Key.value,
                                time_stamp = i.Max(xs => xs.time_stamp)
                            }).ToList();


                        return r;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void WriteCSVFile(string path, List<asset_csv> _AssetProperty)
        {
            using (StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(true)))
            using (CsvWriter cw = new CsvWriter(sw, CultureInfo.CurrentCulture))
            {
                cw.WriteHeader<AssetProperty>();
                cw.NextRecord();
                foreach (asset_csv stu in _AssetProperty)
                {
                    cw.WriteRecord<asset_csv>(stu);
                    cw.NextRecord();
                }
            }
        }
    }
}
