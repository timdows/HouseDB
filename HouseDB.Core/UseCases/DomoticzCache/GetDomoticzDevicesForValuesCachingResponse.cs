using HouseDB.Core.Models;
using System.Collections.Generic;

namespace HouseDB.Core.UseCases.DomoticzCache
{
    public class GetDomoticzDevicesForValuesCachingResponse
    {
        public List<DeviceDTO> Devices { get; set; }
    }
}
