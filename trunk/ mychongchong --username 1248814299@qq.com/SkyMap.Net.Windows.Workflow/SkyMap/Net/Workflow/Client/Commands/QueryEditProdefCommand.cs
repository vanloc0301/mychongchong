namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;

    public class QueryEditProdefCommand : QueryEditCommand, IComboBoxCommand, ICommand
    {
        private static List<Prodef> ps;

        public QueryEditProdefCommand()
        {
            SecurityUtil.CurrentPrincipalChanged += new EventHandler(this.SecurityUtil_CurrentPrincipalChanged);
        }

        public override void AddToQueryParameters(Dictionary<string, string> queryParameters)
        {
            string str = string.Empty;
            if (this.IsEnabled)
            {
                if (this.Value.Length > 0)
                {
                    str = this.Key + this.Operator + this.Value;
                }
            }
            else
            {
                foreach (Prodef prodef in ps)
                {
                    if (str != string.Empty)
                    {
                        str = str + ",";
                    }
                    str = str + "'" + prodef.Id + "'";
                }
                if (str.Length > 0)
                {
                    str = this.Key + " in (" + str + ")";
                }
            }
            if (str.Length == 0)
            {
                str = this.Key + this.Operator + "''";
            }
            queryParameters.Add(this.Key, str);
        }

        private void SecurityUtil_CurrentPrincipalChanged(object sender, EventArgs e)
        {
            ps = null;
            this.SetDataSource();
        }

        private void SetDataSource()
        {
            if (ps == null)
            {
                IDictionary<string, Prodef> prodefs = WorkflowService.Prodefs;
                ps = new List<Prodef>(4);
                if (!SecurityUtil.IsCanAccess("AdminData,Admin"))
                {
                    IList<string> currentPrincipalAuthResourcesByType = SecurityUtil.GetCurrentPrincipalAuthResourcesByType<string>("YWCX_PRODEF");
                    foreach (string str in currentPrincipalAuthResourcesByType)
                    {
                        if (prodefs.ContainsKey(str))
                        {
                            Prodef item = prodefs[str];
                            if (((item.Type == "FLW") || (item.Type == "REG")) && item.Status)
                            {
                                ps.Add(item);
                            }
                        }
                    }
                }
                else
                {
                    foreach (Prodef prodef in prodefs.Values)
                    {
                        if (((prodef.Type == "FLW") || (prodef.Type == "REG")) && prodef.Status)
                        {
                            ps.Add(prodef);
                        }
                    }
                }
            }
            (this.Owner as ToolBarComboBox).Items.Clear();
            (this.Owner as ToolBarComboBox).Items.AddRange(ps.ToArray());
        }

        public override string Key
        {
            get
            {
                return "p.PRODEF_ID";
            }
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
                this.SetDataSource();
            }
        }

        public override string Value
        {
            get
            {
                ToolBarComboBox owner = this.Owner as ToolBarComboBox;
                Prodef selectedItem = owner.SelectedItem as Prodef;
                if (selectedItem != null)
                {
                    return ("'" + selectedItem.Id + "'");
                }
                return string.Empty;
            }
        }
    }
}

