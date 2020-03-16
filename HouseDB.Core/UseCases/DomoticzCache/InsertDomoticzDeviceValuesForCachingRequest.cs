using HouseDB.Core.DomoticzModels;
using MediatR;

namespace HouseDB.Core.UseCases.DomoticzCache
{
    public class InsertDomoticzDeviceValuesForCachingRequest : IRequest<InsertDomoticzDeviceValuesForCachingResponse>
    {
        public DomoticzDeviceValuesForCaching DomoticzDeviceValuesForCaching { get; set; }
    }
}
