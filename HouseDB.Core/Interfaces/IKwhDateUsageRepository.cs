using HouseDB.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HouseDB.Core.Interfaces
{
    public interface IKwhDateUsageRepository
    {
        List<KwhDateUsage> GetKwhDateUsageBetweenDates(int deviceId, DateTime minDate, DateTime maxDate);
        List<DateTime> GetDatesBetweenDates(int deviceId, DateTime minDate, DateTime maxDate);

        void Add(KwhDateUsage kwhDateUsage, bool saveChanges = true);
        Task SaveChangesAsync();
    }
}
