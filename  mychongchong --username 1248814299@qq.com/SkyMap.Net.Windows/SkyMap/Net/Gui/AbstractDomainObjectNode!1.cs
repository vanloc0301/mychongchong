namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using System;
    using System.Windows.Forms;

    public class AbstractDomainObjectNode<T> : ObjectNode where T: SkyMap.Net.DAO.DomainObject
    {
        private T domainObject;

        public override ObjectNode AddChild()
        {
            if (this.EnableAddChild)
            {
                throw new Exception("The method or operation is not implemented.");
            }
            return null;
        }

        public override void Copy()
        {
            if (this.DomainObject != null)
            {
                LoggingService.InfoFormatted("复制节点：{0}", new object[] { this.DomainObject.Name });
                ClipboardWrapper.SetDataObject(new DataObject(this.DomainObject));
            }
        }

        public override void Delete()
        {
            if (this.EnableDelete)
            {
                if (this.DomainObject != null)
                {
                    UnitOfWork work = new UnitOfWork(this.domainObject.GetType());
                    work.RegisterRemoved(this.DomainObject);
                    work.Commit();
                }
                base.Remove();
            }
        }

        public override void PropertyChanged()
        {
            if (this.domainObject != null)
            {
                this.SetText();
            }
        }

        public override void SaveAs()
        {
            if (this.domainObject is ISaveAs)
            {
                SkyMap.Net.DAO.DomainObject domainObject = this.DomainObject;
                if (domainObject is ISaveAs)
                {
                    this.SaveAs((ISaveAs) domainObject);
                }
            }
            else
            {
                base.SaveAs();
            }
        }

        private void SaveAs(ISaveAs domainObject)
        {
            InputBox box = new InputBox("请输入使用的远程DAO对象地址：", "提示", "http://127.0.0.1:7502/DBDAO");
            if (box.ShowDialog(WorkbenchSingleton.MainForm) == DialogResult.OK)
            {
                string result = box.Result;
                if (!string.IsNullOrEmpty(result))
                {
                    try
                    {
                        UnitOfWork unitOfWork = new UnitOfWork(domainObject.GetType(), result);
                        domainObject.SaveAs(unitOfWork);
                        unitOfWork.Commit();
                        MessageHelper.ShowInfo("另存成功！");
                    }
                    catch (Exception exception)
                    {
                        MessageHelper.ShowError("另存时发生错误", exception);
                        LoggingService.Error(exception);
                    }
                }
                else
                {
                    MessageHelper.ShowInfo("请输入远程DAO对象地址");
                    this.SaveAs(domainObject);
                }
            }
        }

        public override void Select()
        {
            this.PropertyEditPanel.SelectedObject = this.DomainObject;
            PropertyView.Instance.PropertyEditPanel = this.PropertyEditPanel;
        }

        protected virtual void SetText()
        {
            string str = this.domainObject.ToString();
            if (str != base.Text)
            {
                base.Text = str;
            }
        }

        public T DomainObject
        {
            get
            {
                return this.domainObject;
            }
            set
            {
                this.domainObject = value;
                this.SetText();
                base.SetIcon(IconService.GetImageForProjectType(this.domainObject.GetType().Name));
            }
        }

        public override DataObject DragDropDataObject
        {
            get
            {
                return new DataObject(this.DomainObject);
            }
        }

        public override bool EnableCopy
        {
            get
            {
                return true;
            }
        }

        public override bool EnableDelete
        {
            get
            {
                return true;
            }
        }

        public override bool EnablePaste
        {
            get
            {
                return true;
            }
        }

        public virtual IPropertyEditPanel PropertyEditPanel
        {
            get
            {
                return PropertyGridEditPanel.Instance;
            }
        }
    }
}

