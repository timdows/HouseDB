using HouseDB.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseDB.Core.SettingModels
{
    public class HouseDBSettings
    {
        public HostingEnvironmentRole HostingEnvironmentRole { get; set; }
        public int HostingEnvironmentPort { get; set; }
    }
}
