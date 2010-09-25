namespace SkyMap.Net.Tools.Organize
{
    using SkyMap.Net.OGM;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms.Design;

    public class UniversalAuthResourceEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService service = null;
            if (((context != null) && (context.Instance != null)) && (provider != null))
            {
                service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    CUniversalAuth instance = (CUniversalAuth) context.Instance;
                    ResourceSelect dialog = new ResourceSelect();
                    if (instance.Type != null)
                    {
                        dialog.AuthType = instance.Type;
                        service.ShowDialog(dialog);
                        if (!string.IsNullOrEmpty(dialog.ResourceId))
                        {
                            instance.ResourceId = dialog.ResourceId;
                            value = dialog.ResourceName;
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
                return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle(context);
        }
    }
}

