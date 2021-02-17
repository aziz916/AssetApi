using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AssetAPI.ClassHelper;
using AssetAPI.Models;
using AssetAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;




namespace AssetAPI.Controllers

{
    [ApiExplorerSettings(IgnoreApi = false)]
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private AssetDbContext _dbcontext;
        private readonly IConfiguration _Configuration;
        private readonly IMapper _mapper;
        public AssetsController(AssetDbContext dbcontext, IConfiguration configuration, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _Configuration = configuration;
            _mapper = mapper;
        }



        //Post assets
        [HttpPost()]
        public async Task<IActionResult> insertasset(Asset asset)
        {
            _dbcontext.asset.Add(asset);
            await _dbcontext.SaveChangesAsync();
            return Ok(asset);
        }

        // GET: api/<ValuesController>
        //Get asset IDs for property set to specific value 
        [HttpGet()]
        public IEnumerable<dynamic> Get(string property, bool value)
        {
            IEnumerable<dynamic> result;
            if (property != null)
            {
                result = _dbcontext.asset
                 .Select(asset => new
                 {
                     name = asset.name,
                     properties = asset.properties
                         .Where(asset_property => asset_property.name == property && asset_property.value == value)
                         .ToList()
                 }).ToList();
            }
            else
            {
                result = _dbcontext.asset
                 .Select(asset => new
                 {
                     name = asset.name,
                     properties = asset.properties
                         .ToList()
                 }).ToList();
            }

            return result;

        }

        // POST api/<ValuesController>
        //Request that triggers file processing. 
        [Produces("application/json")]
        [HttpPost("sync")]
        public async Task<ActionResult> Post(IFormFile file)
        {
            try
            {
                List<asset_csv> rows = new List<asset_csv>();
                AssetProperty _assetProperty = new AssetProperty();
                Services _services = new Services(_dbcontext);

                if (file != null && file.FileName.Length > 0 && file.FileName.EndsWith(".csv"))
                {
                    rows = _services.ReadCSVFileUpload(file);
                }
                else
                {
                    rows = _services.ReadCSVFile(_Configuration.GetValue<string>("AppIdentitySettings:InputPath"));
                }

                if (rows != null)
                {
                    foreach (var row in rows)
                    {
                        //If asset from file cannot be found in DB it has to be logged (anyhow can be even console).                         
                        var _assetInstance = _dbcontext.asset.Where(x => x.id == row.assetid).FirstOrDefault();
                        if (_assetInstance == null)
                        {

                            Console.WriteLine("Skipping row: " + row.assetid.ToString() + row.properties.ToString());
                            continue;

                        }

                        var _assetPropertyInstance = _dbcontext.asset_property.Where(x => x.asset.id == row.assetid && x.name == row.properties).FirstOrDefault();
                        if (_assetPropertyInstance == null)
                        {
                            var _newAssetProperty = new AssetProperty
                            {
                                asset = _assetInstance,
                                name = row.properties,
                                value = false
                            };
                            _dbcontext.asset_property.Add(_newAssetProperty);
                            await _dbcontext.SaveChangesAsync();

                        }
                        else if (_assetPropertyInstance.time_stamp < row.time_stamp)
                        {
                            _assetPropertyInstance.value = row.value;
                            _assetPropertyInstance.time_stamp = row.time_stamp;
                            await _dbcontext.SaveChangesAsync();

                        }
                        else
                        {
                            Console.WriteLine("Skipping row: " + row.assetid.ToString() + row.properties.ToString());
                        }
                    }
                }
                return Ok("Data Successfully Inserted !!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PATCH api/<ValuesController>
        //Set property for asset. 
        [Produces("application/json")]
        [HttpPatch("{asset_id}/properties/{property_id}")]
        public async Task<ActionResult<AssetProperty>> Patch([FromRoute] int asset_id, [FromRoute] int property_id, [FromBody] JsonPatchDocument<AssetPropertyPatch> _assetPropertyPatch)
        {
            try
            {

                //Get the asset instance
                var _assetInstance = _dbcontext.asset.Where(asset => asset.id == asset_id).FirstOrDefault();
                if (_assetInstance == null)
                    return NotFound("Asset not found");

                //get Property instance
                var _assetPropertyInstance = _dbcontext.asset_property.Where(property => property.id == property_id && property.asset.id == asset_id).FirstOrDefault();
                if (_assetPropertyInstance == null)
                    return NotFound("Asset property not found");

                // Map retrieved assetinstance to assetproperty  model with other properties (More or less with exactly same name)
                var assetPropertytoPatch = _mapper.Map<AssetPropertyPatch>(_assetPropertyInstance);

                // Apply assetinstance to ModelState
                _assetPropertyPatch.ApplyTo(assetPropertytoPatch, ModelState);


                if (_assetPropertyInstance.time_stamp < assetPropertytoPatch.time_stamp)
                {

                    // Assign entity changes to original entity retrieved from database
                    _mapper.Map(assetPropertytoPatch, _assetPropertyInstance);
                    // Say to entity framework that you have changes in assetpropety entity and it's modified
                    _dbcontext.Entry(_assetPropertyInstance).State = EntityState.Modified;
                    await _dbcontext.SaveChangesAsync();

                    return Ok(_mapper.Map<AssetPropertyPatch>(_assetPropertyInstance));
                }
                else
                {
                    return BadRequest("Expect latest timestamp");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}