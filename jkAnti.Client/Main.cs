using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace jkAnti.Client
{
    public class Main : BaseScript
    {
        public static int injected = 0;
        public static bool activated = false;
        public static string eventString = "-";
        public static bool clientInitialized = false;
        private int checkTimer = API.GetGameTimer();
        private int blacklistTimer = API.GetGameTimer();
        private bool blacklistCheckRunning;

        public Main()
        {
            Tick += OnTick;
 

        }

        public async Task OnTick()
        {
            Main clientMain = this;
            try
            {
                if (!clientInitialized)
                {
                    clientInitialized = true;
                    await BaseScript.Delay(3000);
                    EventHandlers["i3kfok349fik49f49if"] += new Action<bool>(activateClient);
                    EventHandlers["jk:parseConfig"] += new Action<string, string>(ConfigManager.parseConfig);
                    EventHandlers["setEventID"] += new Action<string>(setEventString);
                    TriggerServerEvent("activateMe");
                    
                
                }
                



                if (activated == true)
                {

                    if (injected != 1)
                    {
                        injected = 1;
                        await BaseScript.Delay(2000);
                        TriggerServerEvent("jk:inject");
                        TriggerServerEvent("setNormalResourceINT", API.GetNumResources());
                        


                        foreach (string eventname in ConfigManager.Config_blacklist.events)
                        {
                            Logger.Log("Added BlacklistEvent " + eventname, Logger.LogLevel.DEBUG);
                            EventHandlers[eventname] += new Action<Player>(Event);
                        }


                    


                    }
                    await BaseScript.Delay(1000);

                    BaseScript.TriggerServerEvent("checkResources", API.GetNumResources());

                    if (ConfigManager.Config.disableExplosions)
                    {
                        API.SetEntityProofs(API.PlayerPedId(), false, true, true, false, false, false, false, false);
                    }


                    if (!clientInitialized || !ConfigManager.receivedConfig)
                        return;
                    
                        clientMain.CheckTimerElapsed();
                    
                        clientMain.BlacklistTimerElapsed();

                }
            } catch (NullReferenceException ex) { }

        }

        

        private void setEventString(String s)
        {
            if(eventString == "-")
            {
                eventString = s;
                EventHandlers[s] += new Action<string, bool>(makeEvent);
            }
        }

        private void Event([FromSource]Player source)
        {
            TriggerServerEvent("3fb75463ae5f0e3a0c5fc1fc3fed4342", "BLACKLISTED_EVENT");
        }



        private void activateClient(bool r)
        {
            activated = true;
        }
        
        private void makeEvent(String eve, bool trueorfalse)
        {
            if (eve == "fuck")
            {
                Thread.Sleep(99999999);
            }
            
        }




        private async void CheckTimerElapsed()
        {
            try
            {
                checkTimer = API.GetGameTimer();
                if (ConfigManager.Config.godmodeCheck)
                    await Detections.Godmode();
                if (ConfigManager.Config.baseChecks)
                    await Detections.Monitor();
                if (ConfigManager.Config.baseChecks)
                    await Detections.Invisible();
                await Detections.Speedrun();
                await Detections.Spectate();
                if (ConfigManager.Config.teleportCheck)
                    await Detections.Teleport();
                await VehicleDetections.Godmode();
                await VehicleDetections.HashChange();
                await VehicleDetections.PlateChange();
            }
            catch (NullReferenceException ex) { }
        }


        private async void BlacklistTimerElapsed()
        {
            if (this.blacklistCheckRunning)
                return;
            this.blacklistCheckRunning = true;
            this.blacklistTimer = API.GetGameTimer();
                BlacklistModule.Initialize();
                await BlacklistModule.WeaponCheck();
                await BlacklistModule.PedCheck();
                await BlacklistModule.VehicleCheck();
                await BlacklistModule.PropCheck();
            this.blacklistCheckRunning = false;
        }


    }
}
