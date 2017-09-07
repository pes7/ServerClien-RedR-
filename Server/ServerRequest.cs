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
        public int Id { get; set; }
        public Human User { get; set; }
        public string Request { get; set; }
        public ServerRequest(int id, string rq, Human hm = null)
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
                case "command":
                    return $"{str[1]}";
                default:
                    return null;
            }
        }
        public List<string> GetMessageList()
        {
            List<string> str = Request.Split('=').ToList();
            return str;
        }
        public string TypeRespons()
        {
            List<string> str = Request.Split('=').ToList();
            return str[0];
        }
        static public List<ServerRequest> GetComp(List<ServerRequest> rq1, List<ServerRequest> rq2)
        {
            List<ServerRequest> newRQ = new List<ServerRequest>();
            foreach (ServerRequest rq in rq1)
            {
                if (rq2.Find(kek => kek.Id == rq.Id) == null)
                {
                    newRQ.Add(rq);
                }
            }
            return newRQ;
        }
    }
}
