namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.View;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using SkyMap.Net.DAO;

    public class WfViewListPrintsCommand : AbstractComboBoxCommand
    {
        private PrintSet printSet;

        private void barItem_Click(object sender, EventArgs e)
        {
            ToolBarComboBox owner = this.Owner as ToolBarComboBox;
            WfView caller = owner.Caller as WfView;
            if (this.IsNeedReList(caller.PrintSet))
            {
                this.printSet = caller.PrintSet;
                WaitDialogHelper.Show();
                try
                {
                    WaitDialogHelper.SetText("正在加载打印报表列表...");
                    owner.Items.Clear();
                    if ((caller.PrintSet != null) && (caller.PrintSet.TempletPrints.Count > 0))
                    {
                        List<TempletPrint> list = new List<TempletPrint>(caller.PrintSet.TempletPrints);
                        IComparer<TempletPrint> comparer = new myReverserClass<TempletPrint>();
                        list.Sort(comparer);
                        foreach (TempletPrint print in list)
                        {
                            owner.Items.Add(print);
                        }
                        owner.SelectedIndex = 0;
                    }
                }
                finally
                {
                    WaitDialogHelper.Close();
                }
            }
        }

        private bool IsNeedReList(PrintSet viewPrintSet)
        {
            if ((viewPrintSet != null) || (this.printSet != null))
            {
                if (((this.printSet == null) && (viewPrintSet != null)) || ((this.printSet != null) && (viewPrintSet == null)))
                {
                    return true;
                }
                if (((this.printSet.Id != viewPrintSet.Id) || ((this.printSet.TempletPrints == null) && (viewPrintSet != null))) || ((this.printSet.TempletPrints != null) && (viewPrintSet == null)))
                {
                    return true;
                }
                if (this.printSet.TempletPrints.Count != viewPrintSet.TempletPrints.Count)
                {
                    return true;
                }
                for (int i = 0; i < this.printSet.TempletPrints.Count; i++)
                {
                    if (!this.printSet.TempletPrints[i].Equals(viewPrintSet.TempletPrints[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void Run()
        {
            ToolBarComboBox owner = this.Owner as ToolBarComboBox;
            WfView caller = owner.Caller as WfView;
            caller.TemplatePrint = owner.SelectedItem as TempletPrint;
            caller.BarStatusUpdate();
        }

        public override object Owner
        {
            get
            {
                return base.Owner;
            }
            set
            {
                base.Owner = value;
                ToolBarComboBox owner = this.Owner as ToolBarComboBox;
                owner.ComboBox.DropDownHeight = 100;
                owner.Click += new EventHandler(this.barItem_Click);
            }
        }

        public class myReverserClass<T> : IComparer<T> where T: DomainObject
        {
            public int Compare(T x, T y)
            {
                return new CaseInsensitiveComparer().Compare(x.Name, y.Name);
            }
        }
    }
}

