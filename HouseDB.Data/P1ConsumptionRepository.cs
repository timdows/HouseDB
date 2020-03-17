using HouseDB.Core.Entities;
using HouseDB.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseDB.Data
{
    public class P1ConsumptionRepository : IP1ConsumptionRepository
    {
        private readonly DataContext _dataContext;

        public P1ConsumptionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(P1Consumption p1Consumption, bool saveChanges = true)
        {
            _dataContext.P1Consumptions.Add(p1Consumption);

            if (saveChanges)
            {
                _dataContext.SaveChanges();
            }
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

        public List<P1Consumption> GetUntillLastMonth()
        {
            var beginningOfLastMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);

            return _dataContext.P1Consumptions
                .Where(item => item.Date >= beginningOfLastMonth)
                .ToList();
        }

        public double GetUsageBetweenDates(DateTime start, DateTime stop)
        {
            var consumption = _dataContext.P1Consumptions
                .Where(item => item.Date >= start &&
                               item.Date <= stop)
                .Sum(item => item.DayUsage);
            return Math.Round(consumption, 2);
        }

        public async Task SaveChangesAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
