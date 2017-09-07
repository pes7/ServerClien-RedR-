using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        const int port = 8888;
        static TcpListener listener;
        public static List<ServerRequest> Messages;
        public static List<Human> UsersOnline;
        public static ServerInfo ServerInf;
        public static int HmId = 0;

        public static List<ServerRequest> AdminCommands;

        static void Main(string[] args)
        {
            Messages = new List<ServerRequest>();
            UsersOnline = new List<Human>();
            AdminCommands = new List<ServerRequest>();
            ServerInf = new ServerInfo("pes7's server");
            ServerInf.Status = ServerInfo.ServerStatus.Running;
            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                listener.Start();
                Console.WriteLine("Ожидание подключений...");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);

                    // создаем новый поток для обслуживания нового клиента
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }
    }
}
