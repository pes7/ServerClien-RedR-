using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public static class ServerFunc
    {
        private static TcpClient client = null;
        public static Human Me = null;
        private static Form fm = null;
        static private bool CanWork()
        {
            return client != null && Me != null ? true : false;

        }
        public static NetworkStream NetStream; // Небезопасная переменная, поскольку она может быть перезаписана с множества функций, лучше сделать List<>
        static public IFormatter Send(TcpClient client, ServerRequest rq)
        {
            IFormatter ser = new BinaryFormatter();
            NetStream = client.GetStream();
            ser.Serialize(NetStream, rq);
            NetStream = client.GetStream();
            return ser;
        }
        static public List<ServerRequest> GetMessages()
        {
            if (CanWork())
            {
                ServerRequest message = new ServerRequest(0, $"command=getmessages", Me);
                List<ServerRequest> Response = (List<ServerRequest>)Send(client, message).Deserialize(NetStream);
                return Response;
            } else return null;
        }
        static public ServerInfo getServerInfo()
        {
            if (CanWork())
            {
                ServerRequest message = new ServerRequest(0, $"command=getstatus");
                ServerInfo Response = (ServerInfo)Send(client, message).Deserialize(NetStream);
                if (Response.AdminCommands != null && Response.AdminCommands.Count > 0)
                    DoCommands(Response.AdminCommands);

                return Response;
            } else return null;
        }
        static public bool SendMessage(string Message)
        {
            if (CanWork())
            {
                ServerRequest message = new ServerRequest(0, $"message={Message}", Me);
                string Response = (string)Send(client, message).Deserialize(NetStream);
                if (Response == "Done")
                {
                    Console.WriteLine($"Message [{message}] sent.");
                    return true;
                }
                else return false;
            } else return false;
        }
        static public bool SendCommand(string command)
        {
            if (CanWork())
            {
                ServerRequest message = new ServerRequest(0, $"command={command}");
                string Response = (string)Send(client, message).Deserialize(NetStream);
                if (Response == "Done.")
                    return true;
                else
                    return false;
            }
            else return false;
        }
        static public bool Connect(string address, int port,Human me,Form Fm)
        {
            try
            {
                client = new TcpClient(address, port);
                ServerRequest message = new ServerRequest(0, $"command=connect", me);
                string Response = (string)Send(client, message).Deserialize(NetStream);
                if (Response == "Connected")
                {
                    Me = me;
                    fm = Fm;
                    Console.WriteLine("Connected.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return false;
        }
        static public void DoCommands(List<ServerRequest> commands)
        {
            foreach (ServerRequest rq in commands)
            {
                if (int.Parse(rq.GetMessageList()[1]) == Me.Id)
                {
                    switch (rq.GetMessageList()[0])
                    {
                        case "kick":
                            client.Close();
                            client = null;
                            Me = null;
                            fm.Close();
                            Console.WriteLine("YOU HAD BEEN KICKED");
                            break;
                    }
                }
            }
        }
        static public Human GetMyInf()
        {
            if (CanWork())
            {
                ServerRequest message = new ServerRequest(0, $"command=getme");
                Human Response = (Human)Send(client,message).Deserialize(NetStream);
                if (Response != null)
                {
                    Me = Response;
                    return Response;
                }
                else return null;
            }
            else return null;
        }
    }
}
