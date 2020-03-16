using HouseDB.Core.Enums;

namespace HouseDB.Core.SettingModels
{
    public class HouseDBSettings
    {
        public HostingEnvironmentRole HostingEnvironmentRole { get; set; }
        public int HostingEnvironmentPort { get; set; }
    }
}
