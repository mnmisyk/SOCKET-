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

namespace SOCKET套接字
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Socket socketWithClient;
        Dictionary<string, Socket> DicSocket = new Dictionary<string, Socket>();
        private void button1_Click(object sender, EventArgs e)
        {
            // socket,bind.listen,

            Socket socketwatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ip = IPAddress.Any;

            IPEndPoint point = new IPEndPoint(ip, 1989);

            socketwatch.Bind(point);
            ShowMsg("监听成功");
            // max connections in moment
            socketwatch.Listen(10);

            Thread th = new Thread(listen);
            th.IsBackground = true;
            th.Start(socketwatch);
     



        }

        void listen(object o) //线程如果有参数，必须是object
        {
            Socket socketwatch = o as Socket; //如果能转换成功返回对象，否则返回null -as用法

            while(true)
            {
                socketWithClient = socketwatch.Accept();
                //加入存储序列
                DicSocket.Add(socketWithClient.RemoteEndPoint.ToString(), socketWithClient);
                //加入下拉框
                IpList.Items.Add(socketWithClient.RemoteEndPoint.ToString());
                ShowMsg(socketWithClient.RemoteEndPoint.ToString() + ":" + "连接成功");
                Thread th = new Thread(receive);
                th.IsBackground = true;
                th.Start(socketWithClient);

            }

        }
     
        
        /// <summary>
        /// 服务器端不停地接受客户端发来的消息
        /// </summary>
        /// <param name="o"></param>
        void receive(object o)
        {
            Socket socketWithClient = o as Socket;
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    //实际接收到地有效字数
                    int r = socketWithClient.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    string str = Encoding.UTF8.GetString(buffer, 0, r);
                    ShowMsg(socketWithClient.RemoteEndPoint + ":" + str);
                }
                catch { }
            }
        }

        void ShowMsg(string str)
        {
            textLog.AppendText(str + "\r\n");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void buttonSendToClient_Click(object sender, EventArgs e)
        {
            string str = textBoxSend.Text;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            string ip = IpList.SelectedItem.ToString();

            DicSocket[ip].Send(buffer);

            //
            ShowMsg("send to :"+socketWithClient.RemoteEndPoint);

            
        }
    }
}
