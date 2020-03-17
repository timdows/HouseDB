using HouseDB.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HouseDB.Core.Interfaces
{
    public interface IP1ConsumptionRepository
    {
        List<P1Consumption> GetAll();
        List<P1Consumption> GetUntillLastMonth();
        List<DateTime> GetExistingDates();
        
        void Add(P1Consumption p1Consumption, bool saveChanges = true);

        Task SaveChangesAsync();
    }
}
