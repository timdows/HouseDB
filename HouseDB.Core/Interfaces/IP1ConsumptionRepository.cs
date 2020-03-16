using HouseDB.Core.Entities;
using System;
using System.Collections.Generic;

namespace HouseDB.Core.Interfaces
{
    public interface IP1ConsumptionRepository
    {
        List<P1Consumption> GetAll();

        List<DateTime> GetExistingDates();

        void Add(P1Consumption p1Consumption);
    }
}
