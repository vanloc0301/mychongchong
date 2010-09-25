namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class PropertyGridEditPanel : AbstractPropertyEditPanel
    {
        private static bool changed = false;
        private PropertyGrid grid = new PropertyGrid();
        private static PropertyGridEditPanel instance;
        private static SkyMap.Net.DAO.UnitOfWork unitOfWork;

        public static  event SelectedGridItemChangedEventHandler SelectedGridItemChanged;

        public static  event EventHandler SelectedObjectChanged;

        public PropertyGridEditPanel()
        {
            this.grid.PropertySort = PropertyService.Get<bool>("FormsDesigner.DesignerOptions.PropertyGridSortAlphabetical", false) ? PropertySort.Alphabetical : PropertySort.CategorizedAlphabetical;
            this.grid.Dock = DockStyle.Fill;
            this.grid.SelectedObjectsChanged += delegate (object sender, EventArgs e) {
                if (SelectedObjectChanged != null)
                {
                    SelectedObjectChanged(sender, e);
                }
            };
            this.grid.SelectedGridItemChanged += delegate (object sender, SelectedGridItemChangedEventArgs e) {
                if (SelectedGridItemChanged != null)
                {
                    SelectedGridItemChanged(sender, e);
                }
            };
            base.Controls.Add(this.grid);
            this.grid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.PropertyChanged);
            this.grid.ContextMenuStrip = MenuService.CreateContextMenu(this, "/Views/PropertyPad/ContextMenu");
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("PropertyGridEditPanel created");
            }
        }

        public override void CancelEdit()
        {
            if (unitOfWork != null)
            {
                unitOfWork.Clear();
            }
        }

        protected override void Dispose(bool disposing)
        {
            instance = null;
            base.Dispose(disposing);
        }

        private void OnPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (sender is DomainObject)
            {
                LoggingService.DebugFormatted("{0}属性值有更改...", new object[] { sender });
                UnitOfWork.RegisterDirty(sender as DomainObject);
            }
            else if (this.grid.SelectedObject is DomainObject)
            {
                if (LoggingService.IsDebugEnabled && (e != null))
                {
                    LoggingService.DebugFormatted("将属性值：{0}修改为{1}", new object[] { e.OldValue, e.ChangedItem.Value });
                }
                try
                {
                    UnitOfWork.RegisterDirty(this.grid.SelectedObject as DomainObject);
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                }
            }
            else
            {
                LoggingService.Info("选择的对象不是DomainObject");
            }
        }

        public void PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("属性值发生改变...");
            }
            this.OnPropertyValueChanged(sender, e);
        }

        public override void RedrawContent()
        {
            this.grid.Refresh();
        }

        public override void Save()
        {
            if (unitOfWork != null)
            {
                unitOfWork.Commit();
            }
            else
            {
                LoggingService.Warn("保存对象时，ＵnitOfWork 是空值，所以什么也不会发生...");
            }
        }

        private static void unitOfWork_Changed(object sender, EventArgs e)
        {
            LoggingService.Debug("有对象修改");
            PropertyView.Instance.IsDirty = true;
        }

        private static void unitOfWork_Cleared(object sender, EventArgs e)
        {
            changed = false;
            PropertyView.Instance.IsDirty = false;
        }

        public static PropertyGrid Grid
        {
            get
            {
                if (instance == null)
                {
                    return null;
                }
                return instance.grid;
            }
        }

        public static PropertyGridEditPanel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PropertyGridEditPanel();
                }
                return instance;
            }
        }

        public override object SelectedObject
        {
            get
            {
                return this.grid.SelectedObject;
            }
            set
            {
                LoggingService.DebugFormatted("选择了新的编辑对象：{0}", new object[] { value });
                if (((this.grid.SelectedObject != null) && (value != null)) && (value.GetType().Namespace != this.grid.SelectedObject.GetType().Namespace))
                {
                    unitOfWork = null;
                }
                this.grid.SelectedObject = value;
            }
        }

        public static SkyMap.Net.DAO.UnitOfWork UnitOfWork
        {
            get
            {
                if (((unitOfWork == null) && (Grid.SelectedObject != null)) && (Grid.SelectedObject is DomainObject))
                {
                    if (unitOfWork == null)
                    {
                        LoggingService.Warn("创建新的UnitOfWork...");
                    }
                    unitOfWork = new SkyMap.Net.DAO.UnitOfWork(Grid.SelectedObject.GetType());
                    unitOfWork.Changed += new EventHandler(PropertyGridEditPanel.unitOfWork_Changed);
                    unitOfWork.Cleared += new EventHandler(PropertyGridEditPanel.unitOfWork_Cleared);
                }
                if (unitOfWork == null)
                {
                    LoggingService.Warn("UnitOfWork is null");
                }
                return unitOfWork;
            }
        }
    }
}

