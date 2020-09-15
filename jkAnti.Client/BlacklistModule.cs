using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace jkAnti.Client
{
    public static class BlacklistModule
    {
        public static bool hasInitialized = false;
        public static List<int> propBlacklist = new List<int>();
        public static List<int> weaponBlacklist = new List<int>();
        public static List<int> vehicleBlacklist = new List<int>();
        public static List<int> pedBlacklist = new List<int>();

        public static async void Initialize()
        {
            if (BlacklistModule.hasInitialized)
                return;
            if (!ConfigManager.receivedConfig)
                return;
            BlacklistModule.propBlacklist.Add(-145066854);
            BlacklistModule.propBlacklist.Add(-1207431159);
            BlacklistModule.propBlacklist.Add(-1874162628);
            BlacklistModule.pedBlacklist.Add(API.GetHashKey("sr_prop_spec_tube_xxs_01a"));
            BlacklistModule.pedBlacklist.Add(-356333586);
            ConfigManager.Config_blacklist.props.ForEach((Action<string>)(def => BlacklistModule.propBlacklist.Add(API.GetHashKey(def))));
            ConfigManager.Config_blacklist.weapons.ForEach((Action<string>)(def => BlacklistModule.weaponBlacklist.Add(API.GetHashKey(def))));
            ConfigManager.Config_blacklist.vehicles.ForEach((Action<string>)(def => BlacklistModule.vehicleBlacklist.Add(API.GetHashKey(def))));
            ConfigManager.Config_blacklist.peds.ForEach((Action<string>)(def => BlacklistModule.pedBlacklist.Add(API.GetHashKey(def))));
            ConfigManager.Config_blacklist.propHashes.ForEach((Action<int>)(def => BlacklistModule.propBlacklist.Add(def)));
            Logger.Log("BlacklistModule > Executed Init", Logger.LogLevel.DEBUG);
            BlacklistModule.hasInitialized = true;
        }

        public static async Task WeaponCheck()
        {
            await BaseScript.Delay(0);
            BlacklistModule.weaponBlacklist.ForEach((Action<int>)(def =>
            {
                Logger.Log("BlacklistModule > Executed Weapon Check with " + def, Logger.LogLevel.DEBUG);
                if (!Game.PlayerPed.Weapons.HasWeapon((WeaponHash)def))
                    return;
                Game.PlayerPed.Weapons.RemoveAll();
            }));
            Logger.Log("BlacklistModule > Executed Weapon Check @ " + API.GetGameTimer().ToString(), Logger.LogLevel.DEBUG);
        }

        public static async Task PropCheck()
        {
            await BaseScript.Delay(0);
            foreach (Prop allProp in World.GetAllProps())
            {
                Logger.Log("BlacklistModule > Executed Prop Check with " + allProp.Model.Hash, Logger.LogLevel.DEBUG);
                if (BlacklistModule.propBlacklist.Contains(allProp.Model.Hash))
                {
                    CommonFunctions.RequestAndDelete(allProp.Handle, true);
                    allProp.Delete();
                }
            }
            Logger.Log("BlacklistModule > Executed Prop Check @ " + API.GetGameTimer().ToString(), Logger.LogLevel.DEBUG);
        }

        public static async Task VehicleCheck()
        {
            await BaseScript.Delay(0);
            foreach (Vehicle allVehicle in World.GetAllVehicles())
            {
                Logger.Log("BlacklistModule > Executed Vehicle Check with " + allVehicle.Model.Hash, Logger.LogLevel.DEBUG);
                if (BlacklistModule.vehicleBlacklist.Contains(allVehicle.Model.Hash))
                {
                    if (API.IsPedInVehicle(Game.PlayerPed.Handle, allVehicle.Handle, false))
                        API.TaskLeaveVehicle(Game.PlayerPed.Handle, allVehicle.Handle, 16);
                    CommonFunctions.RequestAndDelete(allVehicle.Handle, false);
                    allVehicle.Delete();
                }
            }
            Logger.Log("BlacklistModule > Executed Vehicle Check @ " + API.GetGameTimer().ToString(), Logger.LogLevel.DEBUG);
        }

        public static async Task PedCheck()
        {
            await BaseScript.Delay(0);
            foreach (Ped allPed in World.GetAllPeds())
            {
                Logger.Log("BlacklistModule > Executed Ped Check with " + allPed.Model.Hash, Logger.LogLevel.DEBUG);
                Ped ped = allPed;
                if (BlacklistModule.pedBlacklist.Contains(ped.Model.Hash))
                {
                    if (!ped.IsPlayer)
                    {
                        ped.Detach();
                        ped.Delete();
                    }
                    else
                    {
                        if(ped == Game.PlayerPed)
                        {
                            ViolationManager.SendViolation(ViolationType.BLACKLISTED_PED);
                        }
                    }
                }
                BlacklistModule.weaponBlacklist.ForEach((Action<int>)(def =>
                {
                    if (!ped.Weapons.HasWeapon((WeaponHash)def))
                        return;
                    ped.Weapons.RemoveAll();
                }));
            }
           /* if(Game.PlayerPed.Model.Hash != 0x705E61F2 && Game.PlayerPed.Model.Hash != 0x9C9EFFD8 && Game.PlayerPed.Model.Hash != 0xC1C46677)
            {
                API.SetPlayerModel(API.PlayerPedId(), 0x705E61F2);
            }*/
            Logger.Log("BlacklistModule > Executed Ped Check @ " + API.GetGameTimer().ToString(), Logger.LogLevel.DEBUG);
        }


    }
}
