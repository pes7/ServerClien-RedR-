using Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Main : Form
    {
        public List<ServerRequest> Old = null;
        public List<Human> OldH = null;

        public Main()
        {
            InitializeComponent();
            Old = new List<ServerRequest>();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //new Server.Human("Nazar", "Ukolov", 18, "pes7", false, Server.Human.AccesProvider.Admin)
            if (Program.Connect(Program.Me))
                timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.SendMessage(textBox1.Text);
            textBox1.Text = "";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateMessages();
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            ServerInfo SerInf = Program.getServerInfo();
            label1.Text = $"Online:{SerInf.OnlineUsers.Count}";
            this.Text = $"Server name: {SerInf.Name}";
            if (OldH != null) {
                if (Program.IsSimillarHuman(SerInf.OnlineUsers, OldH))
                {
                    listBox2.Items.Clear();
                    foreach (Human hm in SerInf.OnlineUsers)
                    {
                        listBox2.Items.Add(hm.GetName());
                    }
                }
                OldH = SerInf.OnlineUsers;
            }
            else
            {
                listBox2.Items.Clear();
                foreach (Human hm in SerInf.OnlineUsers)
                {
                    listBox2.Items.Add(hm.GetName());
                }
                OldH = SerInf.OnlineUsers;
            }
        }

        private void UpdateMessages()
        {
            List<ServerRequest> iniz = Old != null ? Program.GetComp(Program.GetMessages(), Old) : Program.GetMessages();
            if (iniz != null)
            {
                foreach (ServerRequest msg in iniz)
                {
                    listBox1.Items.Add(msg.GetMessage());
                    Old.Add(msg);
                }
            }
        }
    }
}
