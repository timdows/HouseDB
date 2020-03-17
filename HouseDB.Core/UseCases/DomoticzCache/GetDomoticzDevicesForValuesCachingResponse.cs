using HouseDB.Core.Entities;
using System.Collections.Generic;

namespace HouseDB.Core.UseCases.DomoticzCache
{
    public class GetDomoticzDevicesForValuesCachingResponse
    {
        public List<Device> Devices { get; set; }
    }
}
