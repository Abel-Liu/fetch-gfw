using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;
using System.IO;
using System.Net;
using System.Text;

namespace FetchGFWList.Job
{
    public class FetchGFWJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var dir = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("/"), "f");

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
        }

    }

}