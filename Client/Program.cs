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

        static void Main(string[] args)
        {

            TcpClient client = null;

            try
            {
                client = new TcpClient(address, port);
                IFormatter ser = new BinaryFormatter();
                NetworkStream stream = client.GetStream();
                Registration();
                while (true)
                {
                    Console.Write(Me.GetName() + ": ");
                    string hk = Console.ReadLine();

                    ServerRequest message = new ServerRequest($"message={hk}",Me);
                    ser.Serialize(stream, message);

                    Console.Clear();

                    stream = client.GetStream();
                    List<ServerRequest> Response = (List<ServerRequest>)ser.Deserialize(stream);
                    foreach(ServerRequest sr in Response)
                    {
                        Console.WriteLine($"{sr.GetMessage()}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if(client != null)
                    client.Close();
            }
            Console.ReadKey();
        }

        static private void Menu()
        {

        }

        static private void Updater(TcpClient cl)
        {
            IFormatter ser = new BinaryFormatter();
            NetworkStream stream = cl.GetStream();
            ServerRequest message = new ServerRequest($"update=true");
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

        static private List<ServerRequest> GetComp(List<ServerRequest> rq1, List<ServerRequest> rq2)
        {
            List<ServerRequest> newRQ = new List<ServerRequest>();
            foreach(ServerRequest rq in rq1)
            {
                if(rq2.Find(kek => kek.User == rq.User && kek.Request == rq.Request) == null)
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
    }
}
