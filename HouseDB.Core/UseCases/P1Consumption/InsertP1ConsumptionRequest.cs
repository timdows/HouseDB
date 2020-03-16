using HouseDB.Core.DomoticzModels;
using MediatR;
using System.Collections.Generic;

namespace HouseDB.Core.UseCases.P1Consumption
{
    public class InsertP1ConsumptionRequest : IRequest<InsertP1ConsumptionResponse>
    {
        public List<DomoticzP1Consumption> DomoticzP1Consumptions { get; set; }
    }
}
