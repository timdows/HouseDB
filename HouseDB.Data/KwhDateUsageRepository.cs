using HouseDB.Core.Entities;
using HouseDB.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseDB.Data
{
    public class KwhDateUsageRepository : IKwhDateUsageRepository
    {
        private readonly DataContext _dataContext;

        public KwhDateUsageRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<DateTime> GetDatesBetweenDates(int deviceId, DateTime minDate, DateTime maxDate)
        {
            return _dataContext.KwhDateUsages
                .Where(item => item.DeviceId == deviceId &&
                               item.Date >= minDate &&
                               item.Date <= maxDate)
                .Select(a_item => a_item.Date)
                .ToList();
        }

        public void Add(KwhDateUsage kwhDateUsage, bool saveChanges = true)
        {
            _dataContext.KwhDateUsages.Add(kwhDateUsage);

            if (saveChanges)
            {
                _dataContext.SaveChanges();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public List<KwhDateUsage> GetKwhDateUsageBetweenDates(int deviceId, DateTime minDate, DateTime maxDate)
        {
            return _dataContext.KwhDateUsages
                .Where(item => item.DeviceId == deviceId &&
                               item.Date >= minDate &&
                               item.Date <= maxDate)
                .ToList();
        }
    }
}
