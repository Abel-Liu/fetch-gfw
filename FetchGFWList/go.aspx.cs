using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;

namespace FetchGFWList
{
    public partial class go : System.Web.UI.Page
    {

        public static Thread FetchGFWListThread;

        public static bool IsThreadRun()
        {
            return FetchGFWListThread.ThreadState == ThreadState.Running || FetchGFWListThread.ThreadState == ThreadState.Suspended;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.labInfo.Text = DateTime.Now.ToString();

            FetchGFW();
        }

        void FetchGFW()
        {
            var dir = Path.Combine(Server.MapPath("/"), "f");

            if (FetchGFWListThread == null || !IsThreadRun())
            {
                try
                {
                    FetchGFWListThread.Abort();
                }
                catch
                { }

                FetchGFWListThread = new Thread(delegate ()
                {
                    while (true)
                    {
                        try
                        {
                            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("https://raw.githubusercontent.com/gfwlist/gfwlist/master/gfwlist.txt");
                            var res = (HttpWebResponse)req.GetResponse();
                            var stream = res.GetResponseStream();
                            StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                            var content = reader.ReadToEnd();

                            if (!Directory.Exists(dir))
                                Directory.CreateDirectory(dir);

                            var mainFile = Path.Combine(dir, "gfw.txt");
                            File.WriteAllText(mainFile, content);
                        }
                        catch (Exception ex)
                        {
                        }

                        System.Threading.Thread.Sleep(10 * 60 * 1000);
                    }
                });

                FetchGFWListThread.Start();
            }

        }
    }
}