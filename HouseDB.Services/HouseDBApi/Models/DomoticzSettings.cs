// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace HouseDB.Services.Api.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class DomoticzSettings
    {
        /// <summary>
        /// Initializes a new instance of the DomoticzSettings class.
        /// </summary>
        public DomoticzSettings()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the DomoticzSettings class.
        /// </summary>
        public DomoticzSettings(string host = default(string), int? port = default(int?), int? wattIdx = default(int?))
        {
            Host = host;
            Port = port;
            WattIdx = wattIdx;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "host")]
        public string Host { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "port")]
        public int? Port { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "wattIdx")]
        public int? WattIdx { get; set; }

    }
}