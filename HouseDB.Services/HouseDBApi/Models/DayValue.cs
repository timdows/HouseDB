// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace HouseDB.Services.HouseDBApi.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class DayValue
    {
        /// <summary>
        /// Initializes a new instance of the DayValue class.
        /// </summary>
        public DayValue()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the DayValue class.
        /// </summary>
        public DayValue(System.DateTime? date = default(System.DateTime?), double? usage = default(double?))
        {
            Date = date;
            Usage = usage;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public System.DateTime? Date { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "usage")]
        public double? Usage { get; set; }

    }
}
