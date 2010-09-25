namespace SkyMap.Net.Gui
{
    using DevExpress.XtraEditors;
    using System;
    using System.Windows.Forms;

    public sealed class BinderHelper
    {
        private BinderHelper()
        {
        }

        public static void DoBinding(BaseEdit[] bes, object dataSource, string[] dataMembers)
        {
            DoBinding(bes, dataSource, dataMembers, "EditValue");
        }

        public static void DoBinding(CheckBox[] chks, object dataSource, string[] dataMembers)
        {
            DoBinding(chks, dataSource, dataMembers, "Checked");
        }

        public static void DoBinding(System.Windows.Forms.ComboBox[] cmbs, object dataSource, string[] dataMembers)
        {
            DoBinding(cmbs, dataSource, dataMembers, "Text");
        }

        public static void DoBinding(TextBox[] txts, object dataSource, string[] dataMembers)
        {
            DoBinding(txts, dataSource, dataMembers, "Text");
        }

        private static void DoBinding(Control[] controls, object dataSource, string[] dataMembers, string propertyName)
        {
            if (((controls == null) || (dataSource == null)) || (dataMembers == null))
            {
                throw new ArgumentNullException();
            }
            if (controls.Length != dataMembers.Length)
            {
                throw new ArgumentException("控件数与成员数量不相等");
            }
            for (int i = 0; i < controls.Length; i++)
            {
                Binding binding = controls[i].DataBindings[propertyName];
                if (binding != null)
                {
                    controls[i].DataBindings.Remove(binding);
                }
                controls[i].DataBindings.Add(propertyName, dataSource, dataMembers[i]);
            }
        }

        public static void DoBinding(Control[] controls, object dataSource, string[] dataMembers, string[] propertyNames)
        {
            if (((controls == null) || (dataSource == null)) || (dataMembers == null))
            {
                throw new ArgumentNullException();
            }
            if (controls.Length != dataMembers.Length)
            {
                throw new ArgumentException("控件数与成员数量或属性数不相等");
            }
            for (int i = 0; i < controls.Length; i++)
            {
                string propertyName = "Text";
                if (propertyNames != null)
                {
                    propertyName = propertyNames[i];
                }
                Binding binding = controls[i].DataBindings[propertyName];
                if (binding != null)
                {
                    controls[i].DataBindings.Remove(binding);
                }
                controls[i].DataBindings.Add(propertyName, dataSource, dataMembers[i]);
            }
        }
    }
}

