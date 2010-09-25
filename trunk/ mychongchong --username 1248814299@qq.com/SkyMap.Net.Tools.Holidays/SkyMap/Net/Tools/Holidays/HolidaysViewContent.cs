namespace SkyMap.Net.Tools.Holidays
{
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public class HolidaysViewContent : AbstractViewContent
    {
        private HolidaySetting hs = new HolidaySetting();

        public HolidaysViewContent()
        {
            this.TitleName = "假日管理";
        }

        public override void Dispose()
        {
            this.hs.Dispose();
        }

        public override void Load(string fileName)
        {
        }

        public override void RedrawContent()
        {
        }

        public override void Save()
        {
        }

        public override void Save(string fileName)
        {
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return this.hs;
            }
        }

        public override bool IsDirty
        {
            get
            {
                return false;
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

