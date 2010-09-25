namespace SkyMap.Net.Gui.Components
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections.Generic;

    public static class DataWordLookUpEditHelper
    {
        public static RepositoryItemLookUpEdit Create(string dataWordTypeCode, string displayMember, string valueMember)
        {
            RepositoryItemLookUpEdit edit = new RepositoryItemLookUpEdit();
            edit.DisplayMember = displayMember;
            edit.ValueMember = valueMember;
            edit.Columns.Add(new LookUpColumnInfo("Code", 40, "代码"));
            edit.Columns.Add(new LookUpColumnInfo("Name", 80, "名称"));
            edit.DataSource = DataWordService.FindDataWordsByTypeCode(dataWordTypeCode);
            return edit;
        }

        public static void Init(ComboBoxEdit comboBoxEdit, string dataWordTypeCode)
        {
            comboBoxEdit.Properties.Items.Clear();
            IList<DataWord> list = DataWordService.FindDataWordsByTypeCode(dataWordTypeCode);
            foreach (DataWord word in list)
            {
                comboBoxEdit.Properties.Items.Add(word.Name);
            }
        }

        public static void Init(Action clearAction, Action<string> addAction, string dataWordTypeCode)
        {
            if (clearAction != null)
            {
                clearAction();
            }
            IList<DataWord> list = DataWordService.FindDataWordsByTypeCode(dataWordTypeCode);
            foreach (DataWord word in list)
            {
                if (addAction != null)
                {
                    addAction(word.Name);
                }
            }
        }

        public static void Init(LookUpEdit lookupEdit, string dataWordTypeCode, string displayMember, string valueMember)
        {
            lookupEdit.Properties.DisplayMember = displayMember;
            lookupEdit.Properties.ValueMember = valueMember;
            lookupEdit.Properties.Columns.Add(new LookUpColumnInfo("Code", 40, "代码"));
            lookupEdit.Properties.Columns.Add(new LookUpColumnInfo("Name", 80, "名称"));
            lookupEdit.Properties.DataSource = DataWordService.FindDataWordsByTypeCode(dataWordTypeCode);
        }
    }
}

