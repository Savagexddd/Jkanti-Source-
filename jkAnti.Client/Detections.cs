using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace jkAnti.Client
{
    class Detections
    {
        private static bool IsGodmodeCheckRunning = false;
        private static Vector3 LastPosition = Game.PlayerPed.Position;
        public static int IgnoreTeleportCount = ConfigManager.Config.ignoredTeleports;

        public static async Task Monitor()
        {
            await BaseScript.Delay(0);
            Logger.Log("Executed Monitor.", Logger.LogLevel.DEBUG);
            int handle = Game.PlayerPed.Handle;
            API.SetPedInfiniteAmmoClip(handle, false);
            API.SetEntityInvincible(handle, false);
            API.SetPlayerInvincible(API.PlayerId(), false);
            API.SetEntityCanBeDamaged(handle, true);
            API.NetworkSetLocalPlayerInvincibleTime(0);
        }

        public static async Task Invisible()
        {
            Logger.Log("Executed InvisibleCheck.", Logger.LogLevel.DEBUG);
            await BaseScript.Delay(0);
            int handle = Game.PlayerPed.Handle;
            if (API.GetEntityAlpha(handle) < 128)
            {
                API.SetEntityVisible(handle, true, true);
                API.ResetEntityAlpha(handle);
                Game.PlayerPed.IsVisible = true;
            }
            if (!API.IsEntityVisible(handle) || !Game.PlayerPed.IsVisible)
            {
                API.SetEntityVisible(handle, true, true);
                Game.PlayerPed.IsVisible = true;
            }
        }

        public static async Task Speedrun()
        {
            Logger.Log("Executed SpeedCheck.", Logger.LogLevel.DEBUG);
            await BaseScript.Delay(0);
            int handle = Game.PlayerPed.Handle;
            if (Game.PlayerPed.ParachuteState != ParachuteState.None || API.GetPedParachuteState(handle) >= 0 || (Game.PlayerPed.IsFalling || Game.PlayerPed.IsInParachuteFreeFall) || API.IsPedFalling(handle))
            {
                API.SetEntityMaxSpeed(handle, 80f);
                if ((double)API.GetEntitySpeed(handle) > 80.0)
                {
                    //ViolationManager.SendViolation(ViolationType.PLAYER_SPEEDHACK);
                    Logger.Log("Detected Speed.", Logger.LogLevel.DEBUG);
                }
            }
            else
            {
                API.SetEntityMaxSpeed(handle, 7.1f);
                if ((double)API.GetEntitySpeed(handle) > 15.0 && !API.IsPedInAnyVehicle(handle, false))
                {
                   // ViolationManager.SendViolation(ViolationType.PLAYER_SPEEDHACK);
                    Logger.Log("Detected Speed.", Logger.LogLevel.DEBUG);
                }

            }
        }

        public static async Task Godmode()
        {
            Logger.Log("Checking if GodmodeCheck is running", Logger.LogLevel.DEBUG);
            if (Detections.IsGodmodeCheckRunning)
                return;
            Logger.Log("Executed GodmodeCheck.", Logger.LogLevel.DEBUG);
            Detections.IsGodmodeCheckRunning = true;
            await BaseScript.Delay(1250);
            if(!API.IsPedDeadOrDying(API.PlayerPedId(), true))
            {
                int armour = API.GetPedArmour(API.PlayerPedId());
                if(API.GetPlayerMaxArmour(API.PlayerId()) > 100)
                {
                    ViolationManager.SendViolation(ViolationType.PLAYER_GODMODE);
                }

                int currentHealth = API.GetEntityHealth(API.PlayerPedId());
                if(currentHealth > 200)
                {
                    ViolationManager.SendViolation(ViolationType.PLAYER_GODMODE);
                }

                if (API.GetPlayerInvincible(API.PlayerPedId()))
                {
                    ViolationManager.SendViolation(ViolationType.PLAYER_GODMODE);
                }
                
                if (API.GetUsingseethrough() || API.GetUsingseethrough())
                {
                    ViolationManager.SendViolation(ViolationType.NIGHT_VISION);
                }

            }
            Detections.IsGodmodeCheckRunning = false;
        }

        public static async Task Spectate()
        {
            Logger.Log("Executed SpectateCheck.", Logger.LogLevel.DEBUG);
            await BaseScript.Delay(0);
            Vector3 gameplayCamCoord = API.GetGameplayCamCoord();
            Vector3 position = Game.PlayerPed.Position;
            float squared2D = gameplayCamCoord.DistanceToSquared2D(position);
            if ((double)squared2D <= 5000.0)
                return;
            Logger.Log("Detected Spectate.", Logger.LogLevel.DEBUG);
            ViolationManager.SendViolation(ViolationType.PLAYER_SPECTATE);
        }

        public static async Task Teleport()
        {
            Logger.Log("Executed TeleportCheck.", Logger.LogLevel.DEBUG);
            await BaseScript.Delay(0);
            int handle = Game.PlayerPed.Handle;
            if ((double)Detections.LastPosition.DistanceToSquared(Game.PlayerPed.Position) > 30000.0 && API.GetPedParachuteState(handle) <= 0 && (!Game.PlayerPed.IsFalling && !API.IsPedInParachuteFreeFall(handle)) && (!API.IsPedFalling(handle) && !API.IsPedInAnyVehicle(handle, false)))
            {
                if (Detections.IgnoreTeleportCount > 0)
                {
                    --Detections.IgnoreTeleportCount;
                    Logger.Log("Ignore this Teleport.", Logger.LogLevel.DEBUG);
                }
                else
                    Logger.Log("Detected Teleport.", Logger.LogLevel.DEBUG);
                ViolationManager.SendViolation(ViolationType.PLAYER_TELEPORT);
            }
            Detections.LastPosition = Game.PlayerPed.Position;
        }

    }
}
