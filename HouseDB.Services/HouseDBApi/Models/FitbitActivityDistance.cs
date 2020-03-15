// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace HouseDB.Services.HouseDBApi.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class FitbitActivityDistance
    {
        /// <summary>
        /// Initializes a new instance of the FitbitActivityDistance class.
        /// </summary>
        public FitbitActivityDistance()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the FitbitActivityDistance class.
        /// </summary>
        public FitbitActivityDistance(System.DateTime? date = default(System.DateTime?), double? kiloMeters = default(double?), long? id = default(long?))
        {
            Date = date;
            KiloMeters = kiloMeters;
            Id = id;
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
        [JsonProperty(PropertyName = "kiloMeters")]
        public double? KiloMeters { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public long? Id { get; set; }

    }
}
