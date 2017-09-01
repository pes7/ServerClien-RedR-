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
        private string Name { get; set; }
        private string Surname { get; set; }
        private string Nick { get; set; }
        private bool IsNick { get; set; }
        public int Age { get; set; }
        AccesProvider UserAcces { get; set; }
        public Human(string name, string surname, int age, string nick = null, bool isnick = false, AccesProvider permissions = AccesProvider.User)
        {
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
    }
}
