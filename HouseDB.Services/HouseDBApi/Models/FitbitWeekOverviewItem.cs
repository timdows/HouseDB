// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace HouseDB.Services.HouseDBApi.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class FitbitWeekOverviewItem
    {
        /// <summary>
        /// Initializes a new instance of the FitbitWeekOverviewItem class.
        /// </summary>
        public FitbitWeekOverviewItem()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the FitbitWeekOverviewItem class.
        /// </summary>
        public FitbitWeekOverviewItem(System.DateTime? date = default(System.DateTime?), int? steps = default(int?), double? kiloMeters = default(double?), string displayText = default(string))
        {
            Date = date;
            Steps = steps;
            KiloMeters = kiloMeters;
            DisplayText = displayText;
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
        [JsonProperty(PropertyName = "steps")]
        public int? Steps { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "kiloMeters")]
        public double? KiloMeters { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "displayText")]
        public string DisplayText { get; set; }

    }
}
