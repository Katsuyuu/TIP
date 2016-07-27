using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BasePBX
{
    class UsersContainer
    {
        public List<UserModel> activeUsers { get; set; }

        public UsersContainer()
        {
            activeUsers = new List<UserModel>();
        }

        public void AddUser(UserModel um)
        {
            if (activeUsers.Contains(um))
                throw new InvalidOperationException("Podany użytkownik jest już zalogowany!");

            activeUsers.Add(um);
        }

        public string GetActiveUsers()
        {

            string returnActiveUsers = "";

            foreach(UserModel x in activeUsers)
            {
                returnActiveUsers += x.username + "|" + x.ip + "\n";
            }

            return returnActiveUsers;
        }

        public void RemoveUser(UserModel um)
        {
            Program.usersContainer.activeUsers.RemoveAll(x => x.username == um.username);
        }
    }
}
