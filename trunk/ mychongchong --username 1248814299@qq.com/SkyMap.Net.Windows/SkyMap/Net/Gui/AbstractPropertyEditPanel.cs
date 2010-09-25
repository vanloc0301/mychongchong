namespace SkyMap.Net.Gui
{
    using System;
    using System.Windows.Forms;

    public class AbstractPropertyEditPanel : UserControl, IPropertyEditPanel, IDisposable
    {
        public virtual void CancelEdit()
        {
        }

        public virtual void PostEditor()
        {
        }

        public virtual void RedrawContent()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void Save()
        {
        }

        public virtual object SelectedObject
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
    }
}

