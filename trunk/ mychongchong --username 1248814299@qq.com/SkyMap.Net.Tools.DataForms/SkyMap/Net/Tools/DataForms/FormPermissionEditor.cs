namespace SkyMap.Net.Tools.DataForms
{
    using SkyMap.Net.DataForms;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class FormPermissionEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService service = null;
            if (((context != null) && (context.Instance != null)) && (provider != null))
            {
                service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    FormPermission instance = (FormPermission) context.Instance;
                    DataFormPermissionSettingForm dialog = new DataFormPermissionSettingForm();
                    dialog.FormPermission = instance;
                    if ((service.ShowDialog(dialog) == DialogResult.OK) && (dialog.FormPermission != null))
                    {
                        return dialog.FormPermission.DaoDataFormName;
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

