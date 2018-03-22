// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Exporter.HouseDBService.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class MonthOverview
    {
        /// <summary>
        /// Initializes a new instance of the MonthOverview class.
        /// </summary>
        public MonthOverview()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the MonthOverview class.
        /// </summary>
        public MonthOverview(int? month = default(int?), IList<DeviceValue> deviceValues = default(IList<DeviceValue>), double? p1Usage = default(double?))
        {
            Month = month;
            DeviceValues = deviceValues;
            P1Usage = p1Usage;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "month")]
        public int? Month { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "deviceValues")]
        public IList<DeviceValue> DeviceValues { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "p1Usage")]
        public double? P1Usage { get; set; }

    }
}