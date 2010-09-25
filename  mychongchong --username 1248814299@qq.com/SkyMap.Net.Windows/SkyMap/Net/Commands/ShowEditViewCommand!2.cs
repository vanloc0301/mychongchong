namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public abstract class ShowEditViewCommand<T, U> : AbstractMenuCommand where T: AbstractEditViewContent, new() where U: SkyMap.Net.DAO.DomainObject
    {
        protected ShowEditViewCommand()
        {
        }

        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("Load Edit View: {0} ...", new object[] { typeof(T).FullName });
            }
            U domainObject = this.DomainObject;
            if (domainObject != null)
            {
                T view = (T) ViewContentService.GetView(typeof(T));
                if (view == null)
                {
                    view = Activator.CreateInstance<T>();
                    WorkbenchSingleton.Workbench.ShowView(view);
                }
                else
                {
                    view.WorkbenchWindow.SelectWindow();
                }
                view.Load(domainObject, PropertyGridEditPanel.UnitOfWork);
                if (this.IsCloseOnLeave && (this.Owner is ObjectNode))
                {
                    (this.Owner as ObjectNode).TreeView.BeforeSelect += new TreeViewCancelEventHandler(this.TreeView_BeforeSelect);
                }
            }
        }

        private void TreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            T view = (T) ViewContentService.GetView(typeof(T));
            if (view != null)
            {
                WorkbenchSingleton.Workbench.CloseContent(view);
            }
            (sender as ExtTreeView).BeforeSelect -= new TreeViewCancelEventHandler(this.TreeView_BeforeSelect);
        }

        protected abstract U DomainObject { get; }

        protected virtual bool IsCloseOnLeave
        {
            get
            {
                return true;
            }
        }
    }
}

