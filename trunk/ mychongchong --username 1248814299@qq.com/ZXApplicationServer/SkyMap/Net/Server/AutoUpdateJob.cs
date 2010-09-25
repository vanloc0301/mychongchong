namespace SkyMap.Net.Server
{
    using Quartz;
    using SkyMap.Net.Core;
    using System;

    public class AutoUpdateJob : IJob
    {
        public void Execute(JobExecutionContext context)
        {
            AutoUpdateHepler.TryUpdate();
        }
    }
}

