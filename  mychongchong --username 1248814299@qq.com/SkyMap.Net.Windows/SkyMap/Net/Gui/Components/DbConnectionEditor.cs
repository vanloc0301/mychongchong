namespace SkyMap.Net.Gui.Components
{
    using Microsoft.Data.ConnectionUI;
    using SkyMap.Net.Core;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class DbConnectionEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (((context != null) && (context.Instance != null)) && (provider != null))
            {
                DataConnectionDialog dialog = new DataConnectionDialog();
                dialog.DataSources.Add(DataSource.SqlDataSource);
                dialog.DataSources.Add(DataSource.OracleDataSource);
                dialog.DataSources.Add(DataSource.OdbcDataSource);
                dialog.DataSources.Add(DataSource.AccessDataSource);
                dialog.SelectedDataSource = DataSource.SqlDataSource;
                dialog.SelectedDataProvider = DataProvider.SqlDataProvider;
                IWindowsFormsEditorService service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                if (DataConnectionDialog.Show(dialog) == DialogResult.OK)
                {
                    value = dialog.ConnectionString;
                }
            }
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("设置的数据库连接字符串是:{0}", new object[] { value });
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

