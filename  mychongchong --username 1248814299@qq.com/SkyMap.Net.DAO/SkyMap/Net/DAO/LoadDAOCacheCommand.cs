namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using System;
    using System.ComponentModel;

    public class LoadDAOCacheCommand : AbstractMenuCommand
    {
        public void OnCacheLoaded(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                MessageService.ShowMessage("缓存载入完成");
            }
            else
            {
                MessageService.ShowError(e.Error, "载入缓存错误");
            }
            DAOCacheService.CacheLoaded = (RunWorkerCompletedEventHandler) Delegate.Remove(DAOCacheService.CacheLoaded, new RunWorkerCompletedEventHandler(this.OnCacheLoaded));
        }

        public override void Run()
        {
            DAOCacheService.CacheLoaded = (RunWorkerCompletedEventHandler) Delegate.Combine(DAOCacheService.CacheLoaded, new RunWorkerCompletedEventHandler(this.OnCacheLoaded));
            DAOCacheService.LoadCaches();
        }
    }
}

