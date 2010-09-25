namespace SkyMap.Net.Tools.Organize
{
    using SkyMap.Net.OGM;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms.Design;

    public class ParticipantEntityEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService service = null;
            if (((context != null) && (context.Instance != null)) && (provider != null))
            {
                service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    ParticipantEntityControl control = new ParticipantEntityControl((ParticipantEntity) value);
                    service.DropDownControl(control);
                    value = control.Value;
                    return value;
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

