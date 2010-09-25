namespace SkyMap.Net.Tools.Criteria
{
    using SkyMap.Net.Criteria;
    using SkyMap.Net.Gui;
    using System;

    public class TySearchtableNode : AbstractDomainObjectNode<TySearchtable>
    {
        public override ObjectNode AddChild()
        {
            TySearchXNode parent = base.Parent as TySearchXNode;
            TySearchx domainObject = parent.DomainObject;
            TyFieldsSelect select = new TyFieldsSelect();
            select.DisplayIndex = domainObject.TyFieldsSelects.Count * 10;
            select.Name = "新字段";
            select.TableName = base.DomainObject.TableName;
            select.IsDisplay = "是";
            select.IsCondtion = "否";
            select.Type = "System.String";
            select.TySearchx = domainObject;
            select.Save();
            return base.AddSingleNode<TyFieldsSelect, TyFieldsSelectNode>(select);
        }

        protected override void Initialize()
        {
            TySearchXNode parent = base.Parent as TySearchXNode;
            TySearchx domainObject = parent.DomainObject;
            foreach (TyFieldsSelect select in domainObject.TyFieldsSelects)
            {
                if (select.TableName == base.DomainObject.TableName)
                {
                    base.AddSingleNode<TyFieldsSelect, TyFieldsSelectNode>(select);
                }
            }
        }

        public override string ContextmenuAddinTreePath
        {
            get
            {
                return "/Workbench/Pads/ObjectNodes/TySearchtableContextMenu";
            }
            set
            {
            }
        }
    }
}

