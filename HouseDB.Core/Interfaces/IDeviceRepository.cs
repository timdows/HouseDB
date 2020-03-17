using HouseDB.Core.Entities;
using System.Collections.Generic;

namespace HouseDB.Core.Interfaces
{
    public interface IDeviceRepository
    {
        List<Device> GetAll();
        List<Device> GetAllDevicesForCachingValues();
        List<Device> GetAllDevicesForKwhExport();
    }
}
