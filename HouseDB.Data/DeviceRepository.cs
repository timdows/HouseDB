using HouseDB.Core.Entities;
using HouseDB.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HouseDB.Data
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly DataContext _dataContext;

        public DeviceRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<Device> GetAll()
        {
            return _dataContext.Devices
                .ToList();
        }

        public List<Device> GetAllDevicesForCachingValues()
        {
            return _dataContext.Devices
                .Where(item => item.IsForKwhImport && (item.DomoticzWattIdx != 0 || item.DomoticzKwhIdx != 0))
                .ToList();
        }
    }
}
