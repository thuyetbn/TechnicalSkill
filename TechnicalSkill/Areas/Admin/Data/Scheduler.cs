using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechnicalSkill.Areas.Admin.Data;

namespace TechnicalSkill
{
    public class Scheduler
    {
        public static async void Start()
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            if (!scheduler.IsStarted)
            {
                await scheduler.Start();
            }
            
            IJobDetail job = JobBuilder.Create<Job>().WithIdentity("ExecuteTaskServiceCallJob1", "group1").Build();

            ITrigger trigger = TriggerBuilder.Create().WithIdentity("ExecuteTaskServiceCallTrigger1", "group1").StartNow().WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever()).Build();
            await scheduler.ScheduleJob(job, trigger);
        }

    }
}