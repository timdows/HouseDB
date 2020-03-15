using HouseDB.Core.Entities;
using HouseDB.Core.Interfaces;
using System.Collections.Generic;

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
            throw new System.NotImplementedException();
        }
    }
}
