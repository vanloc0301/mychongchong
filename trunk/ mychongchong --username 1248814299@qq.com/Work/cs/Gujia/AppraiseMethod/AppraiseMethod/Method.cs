using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Xml;
using SkyMap.Net.Gui;
using System.IO;
using System.Reflection;
using SkyMap.Net.Core;

namespace AppraiseMethod
{
    public  class Method :  SkyMap.Net.Gui.AbstractPadContent
    {
        Panel contentPanel = new Panel();
        Control Ctl;
        public override Control Control
        {
            get {
                Init_Control();
                contentPanel.Controls.Add(this.Ctl);
                return contentPanel; 
            }
        }

        private void Init_Control()
        {
            this.Ctl = new MethodControl();
            this.Ctl.Dock = DockStyle.Fill;
            
        }
    }
}
