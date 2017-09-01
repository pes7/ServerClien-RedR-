using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ClientObject
    {
        public TcpClient client;
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }

        public static Human User;
        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                while (true)
                {
                    IFormatter formatter = new BinaryFormatter();
                    ServerRequest Response = (ServerRequest)formatter.Deserialize(stream);
                    if (Response.TypeRespons() != "update")
                    {
                        User = Response.User;
                        Program.Messages.Add(Response);
                        UpdateScreen();

                        IsNewHuman(Response.User);
                    }
                    formatter.Serialize(stream, Program.Messages);
                }
            }
            catch (Exception ex)
            {
                switch (ex.HResult)
                {
                    case -2146232800:
                        Program.Messages.Add(new ServerRequest($"system={User.GetName()} покинул чат."));
                        Program.UsersOnline.Remove(Program.UsersOnline.Find(kek => kek.GetName() == User.GetName() && kek.Age == User.Age));
                        UpdateScreen();
                        break;
                    default:
                        Console.WriteLine($"[{ex.HResult}] {ex.Message}");
                        break;
                }
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
                Program.UsersOnline.Remove(Program.UsersOnline.Find(kek => kek.GetName() == User.GetName() && kek.Age == User.Age));
                UpdateScreen();
            }
        }
        private void IsNewHuman(Human hm)
        {
            if(Program.UsersOnline.Find(kek=>kek.GetName() == hm.GetName() && kek.Age == hm.Age) == null)
            {
                Program.UsersOnline.Add(hm);
            }
        }
        private void UpdateScreen()
        {
            Console.Clear();
            Console.WriteLine($"Online: {Program.UsersOnline.Count}");
            Console.WriteLine("List: {");
            foreach(Human hm in Program.UsersOnline)
            {
                Console.WriteLine($"    {hm.GetName()}");
            }
            Console.WriteLine("}");
            Console.WriteLine("Chat: ");
            foreach (ServerRequest sr in Program.Messages)
            {
                Console.WriteLine($"{sr.GetMessage()}");
            }
        }
    }
}
