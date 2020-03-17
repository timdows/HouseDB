using HouseDB.Core.DomoticzModels;
using MediatR;
using System.Collections.Generic;

namespace HouseDB.Core.UseCases.DomoticzKwhDeviceValue
{
    public class InsertDomoticzDeviceKwhValuesRequest : IRequest<InsertDomoticzDeviceKwhValuesResponse>
    {
        public int DeviceId { get; set; }
        public List<DomoticzDeviceKwhUsage> DomoticzDeviceKwhUsages { get; set; }
    }
}
