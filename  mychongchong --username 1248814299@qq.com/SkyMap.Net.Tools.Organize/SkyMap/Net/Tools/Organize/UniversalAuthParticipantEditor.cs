namespace SkyMap.Net.Tools.Organize
{
    using SkyMap.Net.OGM;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms.Design;

    public class UniversalAuthParticipantEditor : UITypeEditor
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
                    fSelectParticipant dialog = new fSelectParticipant();
                    if (!string.IsNullOrEmpty(instance.ParticipantId))
                    {
                        dialog.ParticipantID = instance.ParticipantId;
                    }
                    service.ShowDialog(dialog);
                    if (!string.IsNullOrEmpty(dialog.ParticipantID))
                    {
                        instance.ParticipantId = dialog.ParticipantID;
                        instance.ParticipantIdValue = dialog.ParticipantIdValue;
                        instance.ParticipantType = dialog.ParticipantType;
                        value = dialog.ParticipantName;
                        return value;
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

