namespace SkyMap.Net.DataForms
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Runtime.CompilerServices;

    public interface IDataForm
    {
        event EventHandler Changed;

        event DataFormCallBackEventHandler DataFormCallBack;

        void AddParams(string key, object val);
        void BindDataToControl(string controlName, DataTable dataSource, string memberName, string[] valueCollection);
        bool ExistFailSavedFile();
        PrintSet FilterTempletPrints(PrintSet printSet);
        bool LoadMe();
        void NotifySystemMessage(string message);
        void PostEditor();
        void Print(TempletPrint templetPrint, bool reBuild);
        void RestoreFailSavedDataSet();
        bool Save();
        void SetElse();
        void SetFormPermission(FormPermission formPermission, DAODataForm ddf, bool saveEnable);
        void SetPropertys();

        IDictionary DataControls { get; }

        DataFormController DataFormConntroller { get; }

        bool IsChanged { get; }

        string[] ProjectSubTypes { get; }
    }
}

