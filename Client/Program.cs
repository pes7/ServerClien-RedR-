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
        public static int port = 8888;
        public static string address = "127.0.0.1";
        public static Human Me;
        public static List<ServerRequest> Rq;
        public static TcpClient client = null;

        public static Client.Main MainF;

        static void Main(string[] args)
        {
            Registration();
            Thread th = new Thread(CreateMain);
            th.Start();
        }

        static private void CreateMain()
        {
            using (MainF = new Client.Main())
            {
                MainF.ShowDialog();
            }
            Console.ReadKey();
        }

        static private void Menu()
        {

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
            Console.Clear();
            surname:
            Console.Write("Введите фамилию: ");
            string surname = Console.ReadLine();
            if (surname == null || surname.Length < 2 || surname.Length > 16)
            {
                Console.WriteLine("Ошибка #2");
                goto surname;
            }
            int i;
            Console.Clear();
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
            Console.Clear();
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
            Console.Clear();
            admin:
            int j;
            Console.Write("Вы админ? 1)Да 0)Нет : ");
            try
            {
                j = int.Parse(Console.ReadLine());
                if(j == 1)
                {
                    string pass;
                    Console.Write("Пароль: ");
                    pass = Console.ReadLine();
                    if (pass == "8912")
                    {
                        if (i == 0)
                        {
                            Me = new Human(userName, surname, ageA, null, false, Human.AccesProvider.Admin);
                        }
                        else
                        {
                            Me = new Human(userName, surname, ageA, nick, true, Human.AccesProvider.Admin);
                        }
                        Console.Clear();
                        return;
                    }
                }
            }
            catch
            {
                Console.WriteLine("Ошибка #6");
                goto admin;
            }
            if (i == 0)
            {
                Me = new Human(userName, surname, ageA);
            }
            else
            {
                Me = new Human(userName, surname, ageA, nick, true);
            }
            Console.Clear();
        }

        private void Login()
        {

        }
    }
}
