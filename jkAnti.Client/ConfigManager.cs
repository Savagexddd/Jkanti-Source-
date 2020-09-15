using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jkAnti.Client
{
    class ConfigManager
    {

        public static bool receivedConfig;
        public static ConfigObject Config;
        public static BlacklistConfig Config_blacklist;

        public static void parseConfig(string configString, string blacklistString)
        {
            try
            {
                ConfigManager.Config = JsonConvert.DeserializeObject<ConfigObject>(configString);
                ConfigManager.Config_blacklist = JsonConvert.DeserializeObject<BlacklistConfig>(blacklistString);
                ConfigManager.receivedConfig = true;
                Logger.Log("Received config from player " + API.GetPlayerName(API.PlayerId()), Logger.LogLevel.DEBUG);
            }
            catch (Exception ex)
            {
                Logger.Log("Couldn't load config: " + ex.Message, Logger.LogLevel.ERROR);
            }
        }
    }
}
