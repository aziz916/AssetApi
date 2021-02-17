using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace AssetAPI.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AssetPropertyPatch, AssetProperty>();
            CreateMap<AssetProperty, AssetPropertyPatch>();
        }
    }
}
