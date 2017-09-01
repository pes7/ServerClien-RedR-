using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    public class ServerRequest
    {
        public Human User { get; set; }
        public string Request { get; set; }
        public ServerRequest(string rq, Human hm = null)
        {
            User = hm;
            Request = rq;
        }
        public string GetMessage()
        {
            List<string> str = Request.Split('=').ToList();
            switch (str[0])
            {
                case "message":
                    return $"{User.GetName()}: {str[1]}";
                case "system":
                    return $"Сервер: {str[1]}";
                default:
                    return null;
            }
        }
        public string TypeRespons()
        {
            List<string> str = Request.Split('=').ToList();
            return str[0];
        }
    }
}
