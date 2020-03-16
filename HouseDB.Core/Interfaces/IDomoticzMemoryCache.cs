using HouseDB.Core.DomoticzModels;

namespace HouseDB.Core.Interfaces
{
    public interface IDomoticzMemoryCache
    {
        void SetDomoticzDeviceValues(string key, DomoticzDeviceValuesForCaching value);
        DomoticzDeviceValuesForCaching TryGetDomoticzDeviceValues(string key);
        void InvalidateAll();
    }
}
