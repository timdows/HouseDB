using HouseDB.Core.DomoticzModels;
using HouseDB.Core.UseCases.DomoticzCache;

namespace HouseDB.Core.Interfaces
{
    public interface IDomoticzMemoryCache
    {
        void SetDomoticzDeviceValuesForCachingRequest(InsertDomoticzDeviceValuesForCachingRequest value);
        InsertDomoticzDeviceValuesForCachingRequest TryGetDomoticzDeviceValuesForCachingRequest();
        void InvalidateAll();
    }
}
