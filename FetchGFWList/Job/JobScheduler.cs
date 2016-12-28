using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace FetchGFWList.Job
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            var trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithSimpleSchedule(s => s.WithIntervalInMinutes(5).RepeatForever())
                    .Build();

            scheduler.ScheduleJob(JobBuilder.Create<FetchGFWJob>().Build(), trigger);

        }
    }
}