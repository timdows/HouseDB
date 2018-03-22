// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Proxy.HouseDBService.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class KwhYearUsageClientModel
    {
        /// <summary>
        /// Initializes a new instance of the KwhYearUsageClientModel class.
        /// </summary>
        public KwhYearUsageClientModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the KwhYearUsageClientModel class.
        /// </summary>
        public KwhYearUsageClientModel(Device device = default(Device), int? year = default(int?), double? yearUsage = default(double?), IList<KwhMonthUsageValue> monthValues = default(IList<KwhMonthUsageValue>), IList<KwhWeekUsageValue> weekValues = default(IList<KwhWeekUsageValue>), IList<KwhDateUsage> dayValues = default(IList<KwhDateUsage>))
        {
            Device = device;
            Year = year;
            YearUsage = yearUsage;
            MonthValues = monthValues;
            WeekValues = weekValues;
            DayValues = dayValues;
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
        [JsonProperty(PropertyName = "year")]
        public int? Year { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "yearUsage")]
        public double? YearUsage { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "monthValues")]
        public IList<KwhMonthUsageValue> MonthValues { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "weekValues")]
        public IList<KwhWeekUsageValue> WeekValues { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "dayValues")]
        public IList<KwhDateUsage> DayValues { get; set; }

    }
}