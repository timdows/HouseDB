using HouseDB.Core.DomoticzModels;
using HouseDB.Core.UseCases.DomoticzCache;

namespace HouseDB.Core.Interfaces
{
    public interface IDomoticzMemoryCache
    {
        void SetDomoticzDeviceValuesForCachingRequest(string key, InsertDomoticzDeviceValuesForCachingRequest value);
        InsertDomoticzDeviceValuesForCachingRequest TryGetDomoticzDeviceValuesForCachingRequest(string key);
        void InvalidateAll();
    }
}
