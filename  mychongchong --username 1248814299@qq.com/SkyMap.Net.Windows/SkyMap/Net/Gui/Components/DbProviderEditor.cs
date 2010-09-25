namespace SkyMap.Net.Gui.Components
{
    using SkyMap.Net.Core;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Drawing.Design;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class DbProviderEditor : UITypeEditor
    {
        private sealed class c__DisplayClass1
        {
            // Fields
            public object value;
        }
        private sealed class c__DisplayClass3
        {
            // Fields
            public IWindowsFormsEditorService wfes;
        }
        private sealed class c__DisplayClass5
        {
            // Fields
            public DbProviderEditor.c__DisplayClass1 CS_8__locals2;
            public DbProviderEditor.c__DisplayClass3 CS_8__locals4;
            public ListBox lst;

            // Methods
            public void Eb__0(object sender, EventArgs e)
            {
                this.CS_8__locals4.wfes.CloseDropDown();
                this.CS_8__locals2.value = this.lst.SelectedValue;
            }
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (((context != null) && (context.Instance != null)) && (provider != null))
            {
                IWindowsFormsEditorService wfes = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                if (wfes != null)
                {
                    c__DisplayClass3 class3 = new c__DisplayClass3();
                    c__DisplayClass1 class4 = new c__DisplayClass1();
                    c__DisplayClass3 CS__a8locals4 = class3;
                    c__DisplayClass1 CS__a8locals2 = class4;
                    ListBox lst = new ListBox();
                    DataTable factoryClasses = DbProviderFactories.GetFactoryClasses();
                    lst.DataSource = factoryClasses;
                    lst.DisplayMember = "InvariantName";
                    lst.ValueMember = "InvariantName";
                    lst.SelectionMode = SelectionMode.One;
                    lst.DoubleClick += delegate(object sender, EventArgs e)
                    {
                        CS__a8locals4.wfes.CloseDropDown();
                        CS__a8locals2.value = lst.SelectedValue;
                    };
                    wfes.DropDownControl(lst);
                    value = lst.Text;
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("选择数据源类型是:{0}", new object[] { value });
                    }
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

