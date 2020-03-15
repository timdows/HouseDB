// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace HouseDB.Services.HouseDBApi.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class DeviceOverview
    {
        /// <summary>
        /// Initializes a new instance of the DeviceOverview class.
        /// </summary>
        public DeviceOverview()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the DeviceOverview class.
        /// </summary>
        public DeviceOverview(Device device = default(Device), IList<DayValue> values = default(IList<DayValue>))
        {
            Device = device;
            Values = values;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "device")]
        public Device Device { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "values")]
        public IList<DayValue> Values { get; set; }

    }
}
