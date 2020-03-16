using HouseDB.Core.Entities;
using HouseDB.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HouseDB.Data
{
    public class P1ConsumptionRepository : IP1ConsumptionRepository
    {
        private readonly DataContext _dataContext;

        public P1ConsumptionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(P1Consumption p1Consumption)
        {
            _dataContext.P1Consumptions.Add(p1Consumption);
            _dataContext.SaveChanges();
        }

        public List<P1Consumption> GetAll()
        {
            return _dataContext.P1Consumptions
                .ToList();
        }

        public List<DateTime> GetExistingDates()
        {
            return _dataContext.P1Consumptions
                .Select(item => item.Date)
                .ToList();
        }
    }
}
