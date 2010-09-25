namespace SkyMap.Net.Gui.Components
{
    using SkyMap.Net.Components;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class ListStringEditor : UITypeEditor
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
                    if (attribute is StringsDropDownAttribute)
                    {
                        StringsDropDownAttribute attribute2 = (StringsDropDownAttribute) attribute;
                        string str = (string) value;
                        List<string> checkedObjs = null;
                        if (!string.IsNullOrEmpty(str))
                        {
                            checkedObjs = new List<string>();
                            checkedObjs.AddRange(str.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries));
                        }
                        StringListUI control = new StringListUI(this);
                        control.Start(edSvc, attribute2.DataSource, checkedObjs);
                        edSvc.DropDownControl(control);
                        bool flag = false;
                        if (checkedObjs == null)
                        {
                            checkedObjs = new List<string>();
                        }
                        else if (checkedObjs.Count > 0)
                        {
                            checkedObjs.Clear();
                        }
                        IList list2 = control.Value;
                        if (!(flag || (list2.Count <= 0)))
                        {
                            flag = true;
                        }
                        foreach (string str2 in list2)
                        {
                            checkedObjs.Add(str2);
                        }
                        string str3 = string.Join("+", checkedObjs.ToArray());
                        if (!str3.Equals(value))
                        {
                            value = str3;
                            LoggingService.Debug("选择的列表已改变...");
                            PropertyGridEditPanel.Instance.PropertyChanged(context.Instance, null);
                        }
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
            return false;
        }

        public override bool IsDropDownResizable
        {
            get
            {
                return true;
            }
        }

        private class StringListUI : CheckedListBox
        {
            private UITypeEditor editor;
            private IWindowsFormsEditorService edSvc;

            public StringListUI(UITypeEditor editor)
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

            public void Start(IWindowsFormsEditorService edSvc, IEnumerable objLists, List<string> checkedObjs)
            {
                this.edSvc = edSvc;
                foreach (string str in objLists)
                {
                    int index = base.Items.Add(str);
                    if ((checkedObjs != null) && checkedObjs.Contains(str))
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

