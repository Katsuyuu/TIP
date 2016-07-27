using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BasePBX
{
    class UserModel
    {
        public string username { get; private set; }
        public string ip { get; private set; }

        public UserModel(string username, string ip)
        {
            this.username = username;
            this.ip = ip;
        }
    }
}
