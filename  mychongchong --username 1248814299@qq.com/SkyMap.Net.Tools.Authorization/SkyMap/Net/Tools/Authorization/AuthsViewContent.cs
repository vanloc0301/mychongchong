namespace SkyMap.Net.Tools.Authorization
{
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public class AuthsViewContent : AbstractViewContent
    {
        private AuthsSetting authsSetting = new AuthsSetting();

        public AuthsViewContent()
        {
            this.TitleName = "权限管理";
        }

        public override void Dispose()
        {
            this.authsSetting.Dispose();
        }

        public override void Load(string fileName)
        {
        }

        public override void RedrawContent()
        {
        }

        public override void Save()
        {
            this.authsSetting.Save();
        }

        public override void Save(string fileName)
        {
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return this.authsSetting;
            }
        }

        public override bool IsDirty
        {
            get
            {
                return this.authsSetting.IsDirty;
            }
            set
            {
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
    }
}

