using System;
using Ozeki.Network;

namespace BasePBX
{
    class Program
    {
        public static UsersContainer usersContainer;

        static void Main(string[] args)
        {
            usersContainer = new UsersContainer();
            var myPBX = new MyPBX(NetworkAddressHelper.GetLocalIP().ToString(), 20000, 20500);
            myPBX.Start();
            AsynchronousSocketListener.StartListening();

            Console.ReadLine();
            myPBX.Stop();
        }
    }
}
