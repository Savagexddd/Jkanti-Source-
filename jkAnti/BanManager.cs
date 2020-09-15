using CitizenFX.Core;
using jkAnti.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jkAnti
{
    public static class BanManager
    {
        public static Dictionary<String, String> bans = new Dictionary<String, String>();

        public static bool isPlayerBanned(String name)
        {
            if(bans.ContainsKey(name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isLicenseBanned(String license)
        {
            if (bans.ContainsValue(license))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void banPlayer(Player p)
        {
            if (!bans.ContainsKey(p.Name))
            {
                bans.Add(p.Name, p.Identifiers["license"]);
                DiscordWebhookManager.SendToWebhook("Player was temporarily banned", "The player " + p.Name + " was temporarily banned. This ban is until the next server restart." + "\n```" + Common.GetPlayerIdentiferString(p.Identifiers) + "```", DiscordColor.BLUE);
            }
        }

    }
}
