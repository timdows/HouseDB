using HouseDB.Core.DomoticzModels;
using MediatR;
using System.Collections.Generic;

namespace HouseDB.Core.UseCases.DomoticzCache
{
    public class InsertDomoticzDeviceValuesForCachingRequest : IRequest<InsertDomoticzDeviceValuesForCachingResponse>
    {
        public DomoticzDeviceValuesForCaching P1Values { get; set; }

        public List<DomoticzDeviceValuesForCaching> DomoticzDeviceValuesForCachings { get; set; }

    }
}
