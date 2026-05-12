using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public static class ConfigurationUtility
    {
        public static string GetConfigurationSettingValue(string configName, string defaultValue = "")
        {
            var configValue = ConfigurationManager.AppSettings[configName];
            return string.IsNullOrEmpty(configValue) ? defaultValue : configValue;
        }

        public static string GetConnectionStringValue(string configName)
        {
            return ConfigurationManager.ConnectionStrings[configName].ConnectionString;
        }
    }
}
