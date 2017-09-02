using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Server;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Client
{
    class Program
    {
        const int port = 8888;
        const string address = "127.0.0.1";
        public static Human Me;
        public static List<ServerRequest> Rq;
        static TcpClient client = null;

        static void Main(string[] args)
        {
            Registration();
            Thread th = new Thread(CreateMain);
            th.Start();
        }

        static private void CreateMain()
        {
            using (Client.Main mn = new Client.Main())
            {
                mn.ShowDialog();
            }
        }

        static public bool Connect(Human me)
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
                if (Response == "Connected") {
                    Me = me;
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

        static public void SendMessage(string message)
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
            }
        }

        static public List<ServerRequest> GetMessages()
        {
            IFormatter ser = new BinaryFormatter();
            NetworkStream stream = client.GetStream();

            ServerRequest message = new ServerRequest(0, $"command=getmessages", Me);
            ser.Serialize(stream, message);

            stream = client.GetStream();
            List<ServerRequest> Response = (List<ServerRequest>)ser.Deserialize(stream);
            return Response;
        }

        static private void Menu()
        {

        }

        static private void Updater(TcpClient cl)
        {
            IFormatter ser = new BinaryFormatter();
            NetworkStream stream = cl.GetStream();
            ServerRequest message = new ServerRequest(0,$"update=true");
            ser.Serialize(stream, message);

            stream = cl.GetStream();
            List<ServerRequest> Response = (List<ServerRequest>)ser.Deserialize(stream); // you have to cast the deserialized object 
            if (Rq != null)
            {
                List<ServerRequest> newRQ = GetComp(Rq, Response);
                if(newRQ.Count > 0)
                {
                    Console.Clear();
                    foreach (ServerRequest sr in newRQ)
                    {
                        Console.WriteLine($"{sr.GetMessage()}");
                    }
                }
            } else {
                foreach (ServerRequest sr in Response)
                {
                    Console.WriteLine($"{sr.GetMessage()}");
                }
            }
            Rq = Response;
        }

        static public ServerInfo getServerInfo()
        {
            IFormatter ser = new BinaryFormatter();
            NetworkStream streamInf = client.GetStream();

            ServerRequest message = new ServerRequest(0, $"command=getstatus");
            ser.Serialize(streamInf, message);

            streamInf = client.GetStream();
            ServerInfo Response = (ServerInfo)ser.Deserialize(streamInf);
            return Response;
        }

        static public List<ServerRequest>  GetComp(List<ServerRequest> rq1, List<ServerRequest> rq2)
        {
            List<ServerRequest> newRQ = new List<ServerRequest>();
            foreach(ServerRequest rq in rq1)
            {
                if(rq2.Find(kek => kek.Id == rq.Id) == null)
                {
                    newRQ.Add(rq);
                }
            }
            return newRQ;
        }

        static private void Registration()
        {
            name:
            Console.Write("Введите имя: ");
            string userName = Console.ReadLine();
            if (userName == null || userName.Length < 2 || userName.Length > 16)
            {
                Console.WriteLine("Ошибка #1");
                goto name;
            }
            surname:
            Console.Write("Введите фамилию: ");
            string surname = Console.ReadLine();
            if (surname == null || surname.Length < 2 || surname.Length > 16)
            {
                Console.WriteLine("Ошибка #2");
                goto surname;
            }
            int i;
            nick:
            Console.WriteLine("Хотите использовать ник или имя и фамилию в чате? 1)Да 0)Нет");
            try
            {
               i = int.Parse(Console.ReadLine());
            }catch(Exception ex)
            {
                Console.WriteLine("Ошибка #3");
                goto nick;
            }
            string nick = null;
            if (i == 1)
            {
                nickinp:
                Console.Write("Введите ник: ");
                nick = Console.ReadLine();
                if (nick == null || nick.Length < 2 || nick.Length > 16)
                {
                    Console.WriteLine("Ошибка #4");
                    goto nickinp;
                }
            }
            int ageA;
            age:
            Console.Write("Сколько вам лет: ");
            try
            {
                ageA = int.Parse(Console.ReadLine());
                if (ageA < 0 || ageA > 150)
                {
                    Console.WriteLine("Ошибка #5");
                    goto age;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка #5");
                goto age;
            }
            if (i == 0)
            {
                Me = new Human(userName, surname, ageA);
            }
else
            {
                Me = new Human(userName, surname, ageA, nick, true);
            }
        }

        private void Login()
        {

        }

        public static bool IsSimillarHuman(List<Human> ls1, List<Human> ls2)
        {
            bool ist = true;
            foreach(Human hm in ls1)
            {
                if(ls2.Find(kek=> kek != hm) == null)
                {
                    ist = false;
                }
            }
            return ist;
        }
    }
}
