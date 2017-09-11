using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    public class Human
    {
        public enum AccesProvider { Admin,Moder,User };
        public int Id { get; set; }
        public string Name { get; }
        public string Surname { get; }
        public string Nick { get; }
        public bool IsNick { get; }
        public int Age { get; }
        public int Folls { get; set; }
        public AccesProvider UserAcces { get; }
        public Human(string name, string surname, int age, string nick = null, bool isnick = false, AccesProvider permissions = AccesProvider.User, int id = 0, int folls = 0)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Nick = nick;
            IsNick = isnick;
            Age = age;
            UserAcces = permissions;
            Folls = folls;
        }
        public string GetName()
        {
            return IsNick ? $"{Nick}" : $"{Name} {Surname}";
        }
        public static bool IsSimillarHuman(List<Human> ls1, List<Human> ls2)
        {
            foreach (Human hm in ls1)
            {
                if (ls2.Find(kek => kek.Id != hm.Id) == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
