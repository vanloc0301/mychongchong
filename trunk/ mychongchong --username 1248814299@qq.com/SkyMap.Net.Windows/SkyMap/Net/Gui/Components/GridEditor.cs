namespace SkyMap.Net.Gui.Components
{
    using SkyMap.Net.Components;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms.Design;

    public class GridEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (service != null)
            {
                GridAttribute ga = null;
                DisplayNameAttribute attribute2 = null;
                foreach (Attribute attribute3 in context.PropertyDescriptor.Attributes)
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
                    GridPanel gp = new GridPanel(ga, value);
                    GridDialog dialog = new GridDialog(gp);
                    dialog.Text = attribute2.DisplayName;
                    service.ShowDialog(dialog);
                }
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}

