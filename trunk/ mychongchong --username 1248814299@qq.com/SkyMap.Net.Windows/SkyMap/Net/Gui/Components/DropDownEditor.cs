namespace SkyMap.Net.Gui.Components
{
    using SkyMap.Net.Components;
    using SkyMap.Net.Core;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class DropDownEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (((context != null) && (context.Instance != null)) && (provider != null))
            {
                IWindowsFormsEditorService service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                if (service != null)
                {
                    foreach (Attribute attribute in context.PropertyDescriptor.Attributes)
                    {
                        if (attribute is DropDownAttribute)
                        {
                            if (LoggingService.IsDebugEnabled)
                            {
                                LoggingService.DebugFormatted("Attribute是:{0}", new object[] { attribute.GetType().FullName });
                            }
                            DropDownAttribute attribute2 = (DropDownAttribute) attribute;
                            IEnumerable dataSource = attribute2.DataSource;
                            ListBox control = new ListBox();
                            control.Items.Clear();
                            foreach (object obj2 in dataSource)
                            {
                                control.Items.Add(obj2);
                            }
                            control.SelectedItem = value;
                            control.Size = new Size(control.Size.Width, (control.Items.Count * control.ItemHeight) + 20);
                            service.DropDownControl(control);
                            value = control.SelectedItem;
                            return value;
                        }
                    }
                }
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if ((context != null) && (context.Instance != null))
            {
                return UITypeEditorEditStyle.DropDown;
            }
            return base.GetEditStyle(context);
        }
    }
}

