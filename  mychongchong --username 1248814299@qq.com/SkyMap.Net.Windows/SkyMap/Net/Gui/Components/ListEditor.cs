namespace SkyMap.Net.Gui.Components
{
    using SkyMap.Net.Components;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Text;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class ListEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (((context != null) && (context.Instance != null)) && (provider != null))
            {
                IWindowsFormsEditorService edSvc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                if (edSvc == null)
                {
                    return value;
                }
                foreach (Attribute attribute in context.PropertyDescriptor.Attributes)
                {
                    if (attribute is ListAttribute)
                    {
                        ListAttribute attribute2 = (ListAttribute) attribute;
                        IList checkedObjs = (IList) value;
                        ObjectListUI control = new ObjectListUI(this);
                        control.Start(edSvc, attribute2.DataSource, checkedObjs);
                        edSvc.DropDownControl(control);
                        bool flag = false;
                        if (checkedObjs == null)
                        {
                            checkedObjs = new ArrayList();
                        }
                        else if (checkedObjs.Count > 0)
                        {
                            checkedObjs.Clear();
                            flag = true;
                        }
                        IList list2 = control.Value;
                        if (!(flag || (list2.Count <= 0)))
                        {
                            flag = true;
                        }
                        foreach (object obj2 in list2)
                        {
                            checkedObjs.Add(obj2);
                        }
                        if (flag)
                        {
                            LoggingService.Debug("选择的列表已改变...");
                            PropertyGridEditPanel.Instance.PropertyChanged(context.Instance, null);
                        }
                        value = checkedObjs;
                        control.End();
                    }
                }
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if ((context != null) && (context.Instance != null))
            {
                return UITypeEditorEditStyle.DropDown;
            }
            return base.GetEditStyle(context);
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, 8 * e.Bounds.Width, e.Bounds.Height);
            e.Graphics.FillRectangle(SystemBrushes.InactiveBorder, rect);
            IList list = (IList) e.Value;
            StringBuilder builder = new StringBuilder();
            foreach (object obj2 in list)
            {
                builder.Append(obj2.ToString()).Append(",");
            }
            if (builder.Length == 0)
            {
                builder.Append("None...");
            }
            e.Graphics.DrawString(builder.ToString(), SystemFonts.CaptionFont, SystemBrushes.Highlight, rect);
        }

        public override bool IsDropDownResizable
        {
            get
            {
                return true;
            }
        }

        private class ObjectListUI : CheckedListBox
        {
            private UITypeEditor editor;
            private IWindowsFormsEditorService edSvc;

            public ObjectListUI(UITypeEditor editor)
            {
                this.editor = editor;
                base.Height = 150;
                this.ItemHeight = Math.Max(4 + Cursors.Default.Size.Height, this.Font.Height);
                this.DrawMode = DrawMode.OwnerDrawFixed;
                base.BorderStyle = System.Windows.Forms.BorderStyle.None;
            }

            public void End()
            {
                this.edSvc = null;
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);
                if (e.Button == MouseButtons.Right)
                {
                    this.edSvc.CloseDropDown();
                }
            }

            public void Start(IWindowsFormsEditorService edSvc, IEnumerable objLists, IList checkedObjs)
            {
                this.edSvc = edSvc;
                foreach (object obj2 in objLists)
                {
                    int index = base.Items.Add(obj2);
                    if ((checkedObjs != null) && checkedObjs.Contains(obj2))
                    {
                        base.SetItemChecked(index, true);
                    }
                }
            }

            public IList Value
            {
                get
                {
                    return base.CheckedItems;
                }
            }
        }
    }
}

