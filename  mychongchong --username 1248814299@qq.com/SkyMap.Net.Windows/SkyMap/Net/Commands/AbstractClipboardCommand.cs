namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public abstract class AbstractClipboardCommand : AbstractMenuCommand
    {
        protected AbstractClipboardCommand()
        {
        }

        public static IClipboardHandler GetClipboardHandlerWrapper(Control ctl)
        {
            TextBoxBase textBox = ctl as TextBoxBase;
            if (textBox != null)
            {
                return new TextBoxWrapper(textBox);
            }
            ComboBox comboBox = ctl as ComboBox;
            if ((comboBox != null) && (comboBox.DropDownStyle != ComboBoxStyle.DropDownList))
            {
                return new ComboBoxWrapper(comboBox);
            }
            return null;
        }

        protected abstract bool GetEnabled(IClipboardHandler editable);
        public override void Run()
        {
            IClipboardHandler activeContent = WorkbenchSingleton.Workbench.ActiveContent as IClipboardHandler;
            if (activeContent == null)
            {
                activeContent = GetClipboardHandlerWrapper(WorkbenchSingleton.ActiveControl);
            }
            if (activeContent != null)
            {
                this.Run(activeContent);
            }
        }

        protected abstract void Run(IClipboardHandler editable);

        public override bool IsEnabled
        {
            get
            {
                IClipboardHandler activeContent = WorkbenchSingleton.Workbench.ActiveContent as IClipboardHandler;
                if (activeContent == null)
                {
                    activeContent = GetClipboardHandlerWrapper(WorkbenchSingleton.ActiveControl);
                }
                return ((activeContent != null) && this.GetEnabled(activeContent));
            }
        }

        private class ComboBoxWrapper : IClipboardHandler
        {
            private ComboBox comboBox;

            public ComboBoxWrapper(ComboBox comboBox)
            {
                this.comboBox = comboBox;
            }

            public void Copy()
            {
                ClipboardWrapper.SetText(this.comboBox.SelectedText);
            }

            public void Cut()
            {
                ClipboardWrapper.SetText(this.comboBox.SelectedText);
                this.comboBox.SelectedText = "";
            }

            public void Delete()
            {
                this.comboBox.SelectedText = "";
            }

            public void Paste()
            {
                this.comboBox.SelectedText = ClipboardWrapper.GetText();
            }

            public void SelectAll()
            {
                this.comboBox.SelectAll();
            }

            public bool EnableCopy
            {
                get
                {
                    return (this.comboBox.SelectionLength > 0);
                }
            }

            public bool EnableCut
            {
                get
                {
                    return (this.comboBox.SelectionLength > 0);
                }
            }

            public bool EnableDelete
            {
                get
                {
                    return true;
                }
            }

            public bool EnablePaste
            {
                get
                {
                    return ClipboardWrapper.ContainsText;
                }
            }

            public bool EnableSelectAll
            {
                get
                {
                    return (this.comboBox.Text.Length > 0);
                }
            }
        }

        private class TextBoxWrapper : IClipboardHandler
        {
            private TextBoxBase textBox;

            public TextBoxWrapper(TextBoxBase textBox)
            {
                this.textBox = textBox;
            }

            public void Copy()
            {
                this.textBox.Copy();
            }

            public void Cut()
            {
                this.textBox.Cut();
            }

            public void Delete()
            {
                this.textBox.SelectedText = "";
            }

            public void Paste()
            {
                this.textBox.Paste();
            }

            public void SelectAll()
            {
                this.textBox.SelectAll();
            }

            public bool EnableCopy
            {
                get
                {
                    return (this.textBox.SelectionLength > 0);
                }
            }

            public bool EnableCut
            {
                get
                {
                    return (!this.textBox.ReadOnly && (this.textBox.SelectionLength > 0));
                }
            }

            public bool EnableDelete
            {
                get
                {
                    return (!this.textBox.ReadOnly && (this.textBox.SelectionLength > 0));
                }
            }

            public bool EnablePaste
            {
                get
                {
                    return !this.textBox.ReadOnly;
                }
            }

            public bool EnableSelectAll
            {
                get
                {
                    return (this.textBox.TextLength > 0);
                }
            }
        }
    }
}

