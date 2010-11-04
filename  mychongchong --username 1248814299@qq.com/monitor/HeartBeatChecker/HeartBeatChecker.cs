using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MSXML2;
using System.Net.Mail;
using System.Threading;
using System.IO;

namespace HeartBeatChecker
{
    public partial class HeartBeatChecker : Form
    {
        private Icon mNetIcon = new Icon("favicon.ico");

        private NotifyIcon TrayIcon;

        private ContextMenu notifyiconMnu; 


        private MSXML2.XMLHTTP objXmlHTTP = new XMLHTTP();
        string[] arrUrl = new string[20];
        //string strFileContent = "";
        public HeartBeatChecker()
        {
            InitializeComponent();//5^1~a~s~p·x^
            Initializenotifyicon();
        }

        private void Initializenotifyicon()
        {
            //设定托盘程序的各个属性 
            TrayIcon = new NotifyIcon();
            TrayIcon.Icon = mNetIcon;
            TrayIcon.Text = "用Visual C#做托盘程序";
            TrayIcon.Visible = true;
            TrayIcon.Click += new System.EventHandler(this.click);

            ////定义一个MenuItem数组，并把此数组同时赋值给ContextMenu对象 
            MenuItem[] mnuItms = new MenuItem[2];
            mnuItms[0] = new MenuItem();
            mnuItms[0].Text = "显示";
            mnuItms[0].Click += new System.EventHandler(this.show);

            mnuItms[1] = new MenuItem();
            mnuItms[1].Text = "退出系统";
            mnuItms[1].Click += new System.EventHandler(this.close);

            notifyiconMnu = new ContextMenu(mnuItms);
            TrayIcon.ContextMenu = notifyiconMnu;
            ////为托盘程序加入设定好的ContextMenu对象 
        }



        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "监控中";
            int j = ReadText();
            if (j <=1 )
            {
                MessageBox.Show("在当前目录下未找到site.ini文件或文件中没有有效网址!");
                return;
            }


            int intT = 1000;
            try
            {
                intT = Convert.ToInt32(txtTime.Text) * intT;
                if (intT < 60000)
                {
                    MessageBox.Show("最小时间间隔为60秒" + intT.ToString());
                    txtTime.Focus();
                    return;
                }
                else
                {
                    timer1.Interval = intT;
                }
                if (backgroundWorker1.IsBusy)
                {
                    MessageBox.Show("正在运行中....");
                }
                else
                {
                    backgroundWorker1.RunWorkerAsync();
                }              
                
            }
            catch
            {
                MessageBox.Show("最小时间间隔为60秒!" + intT.ToString());//5^1~a~s~p·x^
                txtTime.Focus();
                return;            
            }

        }

        private void StartCheck()
        {
            string v = "";
            string tmpstr = "";
            int intError = 0;
            timer1.Stop();
            for (int i = 1; i <= arrUrl.Length; i++)
            {
                try
                {
                    延迟(30* 1000);
                    v = CheckServer(arrUrl[i].ToString());//5^1~a~s~p·x^
                    if (v != "")
                    {
                        intError = 1;
                        if (CheckServer("http://www.baidu.com") == "")
                        {
                            SendMail(v);
                            tmpstr= string.Format("{0}不能访问!", arrUrl[i].ToString());                           
                            Msg(tmpstr);
                        }
                        else
                        {
                            intError = 0;   //如果BAIDU也出错说明网络问题，不发送邮件
                            tmpstr = "网络有问题";
                            Msg(tmpstr);
                        }
                    }
                    else
                    {
                        tmpstr = string.Format("{0}正常  >>>", arrUrl[i].ToString());                        
                        Msg(tmpstr);
                    }
                }
                catch (Exception e)
                {
                    break;
                }
                
            }
            if (intError != 0)
            {
                timer1.Stop();
                Action ac = new Action(delegate() {
                    延迟(30 * 60 * 1000);   //发过邮件后，过半小时再继续测，以免重复提醒
                 Application.DoEvents();
                });
                ac.Invoke();


                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                timer1.Start();
                
            }
        }

        private void Msg(string tmpstr)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(delegate() { this.lblMsg.Text = tmpstr; }));

            }
            else
            {
                this.lblMsg.Text = "网络有问题！";
            }
        }

        public void 延迟(int h)
        {

            var t = DateTime.Now.AddMilliseconds(h);

            while (DateTime.Now < t) Application.DoEvents();

        }



        private string CheckServer(string strUrl)
        {
            string strTitle = "";
            try
            {
                objXmlHTTP.open("GET", strUrl, false, null, null);
                objXmlHTTP.send("");
                if (objXmlHTTP.readyState == 4)
                {

                    if (objXmlHTTP.status.ToString() != "200")
                    {
                        strTitle = strUrl + "发生 HTTP" + objXmlHTTP.status.ToString() + " 错误!";
                    }
                    else
                    {
                        if (objXmlHTTP.responseText.IndexOf("ok") < 0 && objXmlHTTP.responseText.IndexOf("title") <= 0)
                        {
                            strTitle = strUrl + "运行环境可能发生错误!";
                        }
                    }
                }
                objXmlHTTP.abort();
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message + e.TargetSite + objXmlHTTP.status.ToString());
                strTitle = strUrl + " 发生异常!";
            }
            return strTitle;
            
        }

        private void SendMail(string strTitle)
        {
            if (txtSMTP.Text.Trim() != "" && txtMail.Text.Trim() != "")
            {
                
                MailAddress objFrom = new MailAddress(txtMail.Text.Trim(), txtMail.Text.Trim());
                MailAddress objTo = new MailAddress(txtToMail.Text, txtToMail.Text);
                MailMessage objMail = new MailMessage(objFrom,objTo);
                objMail.Subject = strTitle;
 
                objMail.Priority = MailPriority.High;
                SmtpClient MySmtpClient = new SmtpClient(txtSMTP.Text,25);
                MySmtpClient.UseDefaultCredentials = false;

                MySmtpClient.Credentials = new System.Net.NetworkCredential(txtUserName.Text, txtPassword.Text);
                MySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                MySmtpClient.EnableSsl = false;
                try
                {
                    MySmtpClient.Send(objMail);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                objMail.Dispose();
                    

            }
        }

        private int ReadText()
        {
            try
            {
                FileStream aFile = new FileStream(Application.StartupPath + "\\Site.ini", FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                 
                string s = "";                
                int j = 1;
                int n = 1;
                s = sr.ReadLine();
                while (s != null && j<20)
                {
                    if (((s.ToString()).ToLower()).IndexOf("http://") >= 0)
                    {
                        arrUrl[j] = s.ToString();
                        j++;
                    }
                    n++;
                    s = sr.ReadLine();
                }
                sr.Close();
                sr.Dispose();
                aFile.Close();
                aFile.Dispose();
                return j;
 
            }
            catch(Exception e)
            {
                return -1;
            }

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            StartCheck();
        }

        private void HeartBeatChecker_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
        }

        private void HeartBeatChecker_Closed(object sender, FormClosedEventArgs e)
        {
            TrayIcon.Visible = false;
        }
        public void click(object sender, System.EventArgs e)
        {
            //this.Visible = true;
            
        }

        public void close(object sender, System.EventArgs e)
        {
            MessageBox.Show("你选择了退出系统!");
            this.Close();
        }

        public void show(object sender, System.EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.Visible = true;
            this.TopMost = true;
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        } 

    }
}
