namespace SkyMap.Net.Tools.Organize
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using System;

    public class AuthTypeNode : AbstractDomainObjectNode<CAuthType>
    {
        private SkyMap.Net.Security.LoadAndCreateAuth loadAndCreateAuth = null;

        public override ObjectNode AddChild()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("添加'{0}'权限设置...", new object[] { base.Text });
            }
            SkyMap.Net.Security.LoadAndCreateAuth loadAndCreateAuth = this.LoadAndCreateAuth;
            if (loadAndCreateAuth != null)
            {
                loadAndCreateAuth.Create(this);
            }
            return null;
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("加载'{0}'已定义的权限设置列表...", new object[] { base.Text });
            }
            SkyMap.Net.Security.LoadAndCreateAuth loadAndCreateAuth = this.LoadAndCreateAuth;
            if (loadAndCreateAuth != null)
            {
                loadAndCreateAuth.Load(this);
            }
        }

        public override bool EnableAddChild
        {
            get
            {
                return true;
            }
        }

        private SkyMap.Net.Security.LoadAndCreateAuth LoadAndCreateAuth
        {
            get
            {
                CAuthType domainObject = base.DomainObject;
                if (((this.loadAndCreateAuth == null) || !domainObject.AuthSetClass.StartsWith(this.loadAndCreateAuth.GetType().FullName)) && ((domainObject != null) && !string.IsNullOrEmpty(domainObject.AuthSetClass)))
                {
                    Type type = Type.GetType(domainObject.AuthSetClass);
                    if (type != null)
                    {
                        this.loadAndCreateAuth = (SkyMap.Net.Security.LoadAndCreateAuth) Activator.CreateInstance(type);
                        if (this.loadAndCreateAuth != null)
                        {
                            this.loadAndCreateAuth.AuthType = domainObject;
                        }
                        else
                        {
                            LoggingService.WarnFormatted("不能创建类型'{0}'的实例", new object[] { domainObject.AuthSetClass });
                        }
                    }
                    else
                    {
                        LoggingService.WarnFormatted("不能获取类型'{0}'的定义!", new object[] { domainObject.AuthSetClass });
                    }
                }
                return this.loadAndCreateAuth;
            }
        }
    }
}

