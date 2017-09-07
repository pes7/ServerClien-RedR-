using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    public class ServerInfo
    {
        public enum ServerStatus { Running, Error, Aborted}
        public string Name { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public List<Human> OnlineUsers { get; set; }
        public List<ServerRequest> AdminCommands { get; set; }
        public ServerStatus Status { get; set; }
        public ServerInfo(string name, string ip = "127.0.0.1", int port = 8888, List<Human> onlineUsers = null, List<ServerRequest> admincomm = null, ServerStatus st = ServerStatus.Aborted)
        {
            Name = name;
            Ip = ip;
            Port = port;
            OnlineUsers = onlineUsers;
            AdminCommands = admincomm;
            Status = st;
        }
    }
}
