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
        static public List<ServerRequest> GetMessages()
        {
            if (CanWork())
            {
                IFormatter ser = new BinaryFormatter();
                NetworkStream stream = client.GetStream();

                ServerRequest message = new ServerRequest(0, $"command=getmessages", Me);
                ser.Serialize(stream, message);

                stream = client.GetStream();
                List<ServerRequest> Response = (List<ServerRequest>)ser.Deserialize(stream);
                return Response;
            } else return null;
        }
        static public ServerInfo getServerInfo()
        {
            if (CanWork())
            {
                IFormatter ser = new BinaryFormatter();
                NetworkStream streamInf = client.GetStream();

                ServerRequest message = new ServerRequest(0, $"command=getstatus");
                ser.Serialize(streamInf, message);

                streamInf = client.GetStream();
                ServerInfo Response = (ServerInfo)ser.Deserialize(streamInf);

                if(Response.AdminCommands != null && Response.AdminCommands.Count > 0)
                    DoCommands(Response.AdminCommands);

                return Response;
            } else return null;
        }
        static public bool SendMessage(string message)
        {
            if (CanWork())
            {
                IFormatter ser = new BinaryFormatter();
                NetworkStream stream = client.GetStream();

                ServerRequest Message = new ServerRequest(0, $"message={message}", Me);
                ser.Serialize(stream, Message);

                stream = client.GetStream();
                string Response = (string)ser.Deserialize(stream);
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
                IFormatter ser = new BinaryFormatter();
                NetworkStream streamInf = client.GetStream();

                ServerRequest message = new ServerRequest(0, $"command={command}");
                ser.Serialize(streamInf, message);

                streamInf = client.GetStream();
                string Response = (string)ser.Deserialize(streamInf);
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
                IFormatter ser = new BinaryFormatter();
                NetworkStream stream = client.GetStream();

                ServerRequest message = new ServerRequest(0, $"command=connect", me);
                ser.Serialize(stream, message);

                stream = client.GetStream();
                string Response = (string)ser.Deserialize(stream);
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
                Console.WriteLine(int.Parse(rq.GetMessageList()[1])+"");
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
    }
}
