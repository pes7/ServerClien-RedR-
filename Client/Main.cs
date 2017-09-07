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

        public int[,] Sizer = {{541,364},{781,364}/*Settings*/, {539, 487}/*Admin*/};

        private bool IsSettingsUI = false;
        private bool ISAdminUI = false;

        public Main()
        {
            InitializeComponent();
            Old = new List<ServerRequest>();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //new Server.Human("Nazar", "Ukolov", 18, "pes7", false, Server.Human.AccesProvider.Admin)
            if (ServerFunc.Connect(Program.address,Program.port,Program.Me))
                timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServerFunc.SendMessage(textBox1.Text);
            textBox1.Text = "";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateMessages();
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            ServerInfo SerInf = ServerFunc.getServerInfo();
            label1.Text = $"Online:{SerInf.OnlineUsers.Count}";
            this.Text = $"[{Program.Me.GetName()}] Server name: {SerInf.Name}";
            if (OldH != null) {
                if (Human.IsSimillarHuman(SerInf.OnlineUsers, OldH))
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
            List<ServerRequest> iniz = Old != null ? ServerRequest.GetComp(ServerFunc.GetMessages(), Old) : ServerFunc.GetMessages();
            if (iniz != null)
            {
                foreach (ServerRequest msg in iniz)
                {
                    Color FontColor = Color.Black;
                    if (msg.User != null)
                    {
                        switch (msg.User.UserAcces)
                        {
                            case Human.AccesProvider.Admin:
                                FontColor = Color.Red;
                                break;
                            case Human.AccesProvider.Moder:
                                FontColor = Color.Orange;
                                break;
                        }
                    }else{
                        FontColor = Color.Green; // SYSTEM
                    }
                    listBox1.Items.Add(new MyLisboxItem(FontColor,msg.GetMessage()));
                    Old.Add(msg);
                }
            }
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            MyLisboxItem item = listBox1.Items[e.Index] as MyLisboxItem;
            if (item != null)
            {
                e.Graphics.DrawString(
                    item.Message,
                    listBox1.Font,
                    new SolidBrush(item.ItemColor),
                    0,
                    e.Index * listBox1.ItemHeight
                );
            }else
            {
                Console.WriteLine("ERROR");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (!IsSettingsUI)
            {
                int index = ISAdminUI ? 0 : 2;
                ISAdminUI = ISAdminUI ? false : true;
                this.Size = new Size(Sizer[index, 0], Sizer[index, 1]);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
