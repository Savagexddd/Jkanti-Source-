using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jkAnti.Client
{
    class ConfigObject
    {

        public string licenseKey { get; set; }

        public string discordWebhook { get; set; }

        public bool enableGlobalBanlist { get; set; }

        public bool godmodeCheck { get; set; }

        public bool DebugMode { get; set; }

        public bool VPN { get; set; }

        public List<string> bypass { get; set; }

        public List<string> admins { get; set; }

        public bool teleportCheck { get; set; }

        public bool disableExplosions { get; set; }

        public int maxBillAmount { get; set; }

        public int ignoredTeleports { get; set; }

        public bool speedCheck { get; set; }

        public bool baseChecks { get; set; }

    }
}
