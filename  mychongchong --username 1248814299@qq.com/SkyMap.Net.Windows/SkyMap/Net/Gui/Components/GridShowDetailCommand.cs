namespace SkyMap.Net.Gui.Components
{
    using SkyMap.Net.Components;
    using SkyMap.Net.Core;
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public abstract class GridShowDetailCommand : AbstractCommand
    {
        protected GridShowDetailCommand()
        {
        }

        public override void Run()
        {
            if (this.Owner is GridPanel)
            {
                object focusRow = (this.Owner as GridPanel).GetFocusRow();
                if (focusRow != null)
                {
                    PropertyInfo property = focusRow.GetType().GetProperty(this.PropertyName);
                    if (property != null)
                    {
                        GridAttribute ga = null;
                        DisplayNameAttribute attribute2 = null;
                        foreach (Attribute attribute3 in property.GetCustomAttributes(false))
                        {
                            if (attribute3 is GridAttribute)
                            {
                                ga = (GridAttribute) attribute3;
                            }
                            if (attribute3 is DisplayNameAttribute)
                            {
                                attribute2 = (DisplayNameAttribute) attribute3;
                            }
                        }
                        if (ga != null)
                        {
                            GridPanel gp = new GridPanel(ga, property.GetValue(focusRow, null));
                            GridDialog dialog = new GridDialog(gp);
                            dialog.Text = attribute2.DisplayName;
                            dialog.Show();
                        }
                    }
                }
            }
        }

        public abstract string PropertyName { get; }
    }
}

