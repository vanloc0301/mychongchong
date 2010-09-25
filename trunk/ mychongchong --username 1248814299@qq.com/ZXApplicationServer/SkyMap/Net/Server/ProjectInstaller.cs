namespace SkyMap.Net.Server
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Configuration.Install;
    using System.ServiceProcess;

    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceInstaller serviceInstaller;
        private const string ServiceNameSwitch = "ServiceName";
        private ServiceProcessInstaller serviceProcessInstaller;

        public ProjectInstaller()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.serviceProcessInstaller = new ServiceProcessInstaller();
            this.serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            this.serviceInstaller = new ServiceInstaller();
            this.serviceInstaller.StartType = ServiceStartMode.Automatic;
            this.SetServiceName(ZXService.LookupServiceName());
            base.Installers.AddRange(new Installer[] { this.serviceProcessInstaller, this.serviceInstaller });
        }

        protected override void OnBeforeInstall(IDictionary stateSaver)
        {
            string serviceName = this.ServiceName(stateSaver);
            stateSaver["ServiceName"] = serviceName;
            this.SetServiceName(serviceName);
            base.OnBeforeInstall(stateSaver);
        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            this.SetServiceName(this.ServiceName(savedState));
            base.OnBeforeUninstall(savedState);
        }

        private string ServiceName(IDictionary savedState)
        {
            if (base.Context.Parameters.ContainsKey("ServiceName"))
            {
                return base.Context.Parameters["ServiceName"];
            }
            if (savedState.Contains("ServiceName"))
            {
                return savedState["ServiceName"].ToString();
            }
            return ZXService.LookupServiceName();
        }

        private void SetServiceName(string serviceName)
        {
            this.serviceInstaller.DisplayName = serviceName;
            this.serviceInstaller.ServiceName = serviceName;
        }

        public override string HelpText
        {
            get
            {
                return string.Format("Usage: installutil [/u] [/{0}=MyService] ZXApplicationServer.exe", "ServiceName");
            }
        }
    }
}

