﻿using System;
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

        public Human User;
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
                    switch (Response.TypeRespons())
                    {
                        case "update":

                            break;
                        case "system":
                           
                            break;
                        case "command":
                            switch (Response.GetMessage())
                            {
                                case "connect":
                                    //User = Response.User;
                                    formatter.Serialize(stream, "Connected");
                                    User = IsNewHuman(Response.User);
                                    Program.Messages.Add(new ServerRequest(0, $"system={User.GetName()} присоединился."));
                                    UpdateScreen();
                                    break;
                                case "getmessages":
                                    formatter.Serialize(stream, Program.Messages);
                                    break;
                                case "getstatus":
                                    Program.ServerInf.OnlineUsers = Program.UsersOnline;
                                    Program.ServerInf.AdminCommands = Program.AdminCommands;
                                    formatter.Serialize(stream, Program.ServerInf);
                                    break;
                                case "kick":
                                    Program.Messages.Add(new ServerRequest(0, $"system={Program.UsersOnline.Find(kek=>kek.Id==int.Parse(Response.GetMessageList()[2])).GetName()} был кикнут!"));
                                    Program.AdminCommands.Add(new ServerRequest(Program.AdminCommands.Count, $"kick={Response.GetMessageList()[2]}"));
                                    formatter.Serialize(stream, "Done");
                                    UpdateScreen();
                                    break;
                                case "getme":
                                    formatter.Serialize(stream,User);
                                    break;
                            }
                            break;
                        case "message":
                            User = Response.User;
                            Response.Id = Program.Messages.Count;
                            Program.Messages.Add(Response);
                            IsNewHuman(Response.User);
                            UpdateScreen();
                            formatter.Serialize(stream,"Done");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                switch (ex.HResult)
                {
                    case -2146232800:
                        if (User != null)
                        {
                            Program.Messages.Add(new ServerRequest(Program.Messages.Count, $"system={User.GetName()} покинул чат."));
                            if (Program.UsersOnline.Count > 0)
                            {
                                Program.UsersOnline.Remove(Program.UsersOnline.Find(kek => kek.GetName() == User.GetName() && kek.Age == User.Age));
                            }
                            UpdateScreen();
                        }
                        break;
                    default:
                        if (User != null)
                        {
                            Program.Messages.Add(new ServerRequest(Program.Messages.Count, $"system={User.GetName()} покинул чат."));
                            if (Program.UsersOnline.Count > 0)
                            {
                                Program.UsersOnline.Remove(Program.UsersOnline.Find(kek => kek.GetName() == User.GetName() && kek.Age == User.Age));
                            }
                            UpdateScreen();
                            Console.WriteLine($"[{ex.HResult}] {ex.Message}");
                        }
                        break;
                }
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }
        private Human IsNewHuman(Human hm)
        {
            if (Program.UsersOnline.Find(kek => kek.GetName() == hm.GetName() && kek.Age == hm.Age) == null)
            {
                hm.Id = Program.HmId;
                Program.HmId++;
                Program.UsersOnline.Add(hm);
                return hm;
            }
            else return null;
        }
        private void UpdateScreen()
        {
            Console.Clear();
            Console.WriteLine($"Online: {Program.UsersOnline.Count}");
            Console.WriteLine($"Messages: {Program.Messages.Count}");
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
