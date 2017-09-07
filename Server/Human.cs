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
        private string Name { get; }
        private string Surname { get; }
        private string Nick { get; }
        private bool IsNick { get; }
        public int Age { get; }
        public AccesProvider UserAcces { get; }
        public Human(string name, string surname, int age, string nick = null, bool isnick = false, AccesProvider permissions = AccesProvider.User, int id = 0)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Nick = nick;
            IsNick = isnick;
            Age = age;
            UserAcces = permissions;
        }
        public string GetName()
        {
            return IsNick ? $"{Nick}" : $"{Name} {Surname}";
        }
        public static bool IsSimillarHuman(List<Human> ls1, List<Human> ls2)
        {
            foreach (Human hm in ls1)
            {
                if (ls2.Find(kek => kek != hm) == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
