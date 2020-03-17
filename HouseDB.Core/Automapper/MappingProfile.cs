using AutoMapper;
using HouseDB.Core.Entities;
using HouseDB.Core.Models;

namespace HouseDB.Core.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Device, DeviceDTO>();
        }
    }
}
