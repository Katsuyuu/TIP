using System;
using Ozeki.Network;
using Ozeki.VoIP;
using System.Net;

namespace BasePBX
{
    class MyPBX : PBXBase
    {
        public string localAddress;

        public MyPBX(string localAddress, int minPortRange, int maxPortRange) : base(minPortRange, maxPortRange)
        {
            this.localAddress = localAddress;
            Console.WriteLine("PBX starting...");
            Console.WriteLine("Local address: " + localAddress);

            Program.usersContainer = new UsersContainer();
        }

        protected override void OnStart()
        {
            SetListenPort(localAddress, 5060, Ozeki.Network.TransportType.Udp);
            Console.WriteLine("Listened port: 5060(UDP)");

            Console.WriteLine("PBX started.");
            base.OnStart();
        }

        protected override RegisterResult OnRegisterReceived(ISIPExtension extension, SIPAddress from, int expires)
        {            
            UserModel um = new UserModel(from.DisplayName, from.Host.ToString());
            if (Program.usersContainer.activeUsers.Exists(x => x.username == from.DisplayName))
            {
                return null;
            }
            else
            {
                Program.usersContainer.activeUsers.Add(um);
                Console.WriteLine("Zajerestrowano użytkownika: '{0}'", extension.ExtensionID);
                Console.WriteLine("Aktualnie zarejestrowanych użytkowników: " + Program.usersContainer.activeUsers.Count);
                return base.OnRegisterReceived(extension, from, expires);
            }
        }

        protected override void OnUnregisterReceived(ISIPExtension extension)
        {
            UserModel um = new UserModel(extension.InstanceInfo.Identity.UserName, extension.InstanceInfo.Identity.Address);
            //string username = extension.InstanceInfo.Identity.UserName
            Program.usersContainer.RemoveUser(um);

            Console.WriteLine("Wyrejestrowano użytkownika: '{0}'", extension.ExtensionID);
            Console.WriteLine("Aktualnie zarejestrowanych użytkowników: " + Program.usersContainer.activeUsers.Count);
            base.OnUnregisterReceived(extension);
        }

        protected override void OnCallRequestReceived(ISessionCall call)
        {            
            Console.WriteLine("Call request received. Caller: " + call.DialInfo.CallerID + " callee: " + call.DialInfo.Dialed);
            base.OnCallRequestReceived(call);
        }
    }
}