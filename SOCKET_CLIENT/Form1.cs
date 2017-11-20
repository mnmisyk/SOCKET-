using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOCKET_CLIENT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Socket socketsend;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                socketsend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(textBoxIP.Text);
                IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(textBoxPort.Text));
                socketsend.Connect(point);
                showMsg("连接成功");
                Thread th = new Thread(receive);
                th.IsBackground = true;
                th.Start();
                
            }
            catch { }
        }
        /// <summary>
        /// 不停地接受来自服务器的消息
        /// </summary>
        void receive()
        {
            while (true)
            {
                byte[] buffer = new byte[1024 * 1024 * 2];
                int r = socketsend.Receive(buffer);
                if (r == 0)
                {
                    break;
                }
                string s = Encoding.UTF8.GetString(buffer,0,r);
                showMsg(socketsend.RemoteEndPoint+": "+s);
            }
        }
        void showMsg(string str)
        {
            textBoxMsg.AppendText(str + "\r\n");
        }
        /// <summary>
        /// 客户端发消息给服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string str = textBoxSend.Text.Trim();
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            socketsend.Send(buffer);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
