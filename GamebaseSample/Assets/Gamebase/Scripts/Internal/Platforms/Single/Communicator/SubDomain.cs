using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class SubDomain
    {
        private readonly List<string> alphaList = new List<string>
        {
            "wss://gslb-gamebase.alpha-nhncloudservice.com:11442/lh",
            "wss://alpha-gslb-gamebase.nhngameplatform.com:11442/lh"
        };

        private readonly List<string> betaList = new List<string>
        {
            "wss://gslb-gamebase.beta-nhncloudservice.com:11443/lh",
            "wss://beta-gslb-gamebase.nhngameplatform.com:11443/lh"
        };

        private readonly List<string> realList = new List<string>
        {
            "wss://gslb-gamebase.nhncloudservice.com:11443/lh",
            "wss://gslb-gamebase.nhngameplatform.com:11443/lh"
        };

        private readonly Lighthouse.ZoneType zone;
        private int selectedIndex = 0;

        public SubDomain(Lighthouse.ZoneType zone)
        {
            this.zone = zone;
        }

        public string GetAddress()
        {
            return GetAddressList()[selectedIndex];
        }

        public string GetNextAddress()
        {
            if (selectedIndex + 1 >= GetAddressList().Count)
            {
                return string.Empty;
            }

            selectedIndex++;
            return GetAddress();
        }

        public void ResetAddress()
        {
            selectedIndex = 0;
        }

        private List<string> GetAddressList()
        {
            switch (zone)
            {
                case Lighthouse.ZoneType.ALPHA:
                    {
                        return alphaList;
                    }
                case Lighthouse.ZoneType.BETA:
                    {
                        return betaList;
                    }
                default:
                    {
                        return realList;
                    }
            }
        }
    }
}