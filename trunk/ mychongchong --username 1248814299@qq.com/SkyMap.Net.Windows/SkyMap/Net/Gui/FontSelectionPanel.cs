namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui.XmlForms;
    using System;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Windows.Forms;

    public class FontSelectionPanel : BaseSharpDevelopUserControl
    {
        private static Font boldComboBoxFont;
        private static StringFormat drawStringFormat = new StringFormat(StringFormatFlags.NoWrap);

        public FontSelectionPanel()
        {
            base.SetupFromXmlStream(base.GetType().Assembly.GetManifestResourceStream("Resources.FontSelectionPanel.xfrm"));
            InstalledFontCollection fonts = new InstalledFontCollection();
            for (int i = 6; i <= 0x18; i++)
            {
                ((ComboBox) base.ControlDictionary["fontSizeComboBox"]).Items.Add(i);
            }
            ((ComboBox) base.ControlDictionary["fontSizeComboBox"]).TextChanged += new EventHandler(this.UpdateFontPreviewLabel);
            foreach (FontFamily family in fonts.Families)
            {
                if ((family.IsStyleAvailable(FontStyle.Regular) && family.IsStyleAvailable(FontStyle.Bold)) && family.IsStyleAvailable(FontStyle.Italic))
                {
                    ((ComboBox) base.ControlDictionary["fontListComboBox"]).Items.Add(new FontDescriptor(family));
                }
            }
            ((ComboBox) base.ControlDictionary["fontListComboBox"]).TextChanged += new EventHandler(this.UpdateFontPreviewLabel);
            ((ComboBox) base.ControlDictionary["fontListComboBox"]).SelectedIndexChanged += new EventHandler(this.UpdateFontPreviewLabel);
            ((ComboBox) base.ControlDictionary["fontListComboBox"]).MeasureItem += new MeasureItemEventHandler(this.MeasureComboBoxItem);
            ((ComboBox) base.ControlDictionary["fontListComboBox"]).DrawItem += new DrawItemEventHandler(this.ComboBoxDrawItem);
            boldComboBoxFont = new Font(base.ControlDictionary["fontListComboBox"].Font, FontStyle.Bold);
        }

        private void ComboBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox box = (ComboBox) sender;
            e.DrawBackground();
            if (e.Index >= 0)
            {
                FontDescriptor descriptor = (FontDescriptor) box.Items[e.Index];
                Rectangle layoutRectangle = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                Brush windowText = SystemBrushes.WindowText;
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    windowText = SystemBrushes.HighlightText;
                }
                e.Graphics.DrawString(descriptor.Name, descriptor.IsMonospaced ? boldComboBoxFont : box.Font, windowText, layoutRectangle, drawStringFormat);
            }
            e.DrawFocusRectangle();
        }

        private void MeasureComboBoxItem(object sender, MeasureItemEventArgs e)
        {
            ComboBox box = (ComboBox) sender;
            if (e.Index >= 0)
            {
                FontDescriptor descriptor = (FontDescriptor) box.Items[e.Index];
                SizeF ef = e.Graphics.MeasureString(descriptor.Name, box.Font);
                e.ItemWidth = (int) ef.Width;
                e.ItemHeight = box.Font.Height;
            }
        }

        public static Font ParseFont(string font)
        {
            try
            {
                string[] strArray = font.Split(new char[] { ',', '=' });
                return new Font(strArray[1], float.Parse(strArray[3]));
            }
            catch (Exception exception)
            {
                LoggingService.Warn(exception);
                return ResourceService.CourierNew10;
            }
        }

        private void UpdateFontPreviewLabel(object sender, EventArgs e)
        {
            base.ControlDictionary["fontPreviewLabel"].Font = this.CurrentFont;
        }

        public Font CurrentFont
        {
            get
            {
                int num = 10;
                try
                {
                    num = Math.Max(6, int.Parse(base.ControlDictionary["fontSizeComboBox"].Text));
                }
                catch (Exception)
                {
                }
                int selectedIndex = ((ComboBox) base.ControlDictionary["fontListComboBox"]).SelectedIndex;
                if (selectedIndex < 0)
                {
                    return this.Font;
                }
                FontDescriptor descriptor = (FontDescriptor) ((ComboBox) base.ControlDictionary["fontListComboBox"]).Items[selectedIndex];
                return new Font(descriptor.Name, (float) num);
            }
            set
            {
                int num = 0;
                for (int i = 0; i < ((ComboBox) base.ControlDictionary["fontListComboBox"]).Items.Count; i++)
                {
                    FontDescriptor descriptor = (FontDescriptor) ((ComboBox) base.ControlDictionary["fontListComboBox"]).Items[i];
                    if (descriptor.Name == value.Name)
                    {
                        num = i;
                    }
                }
                ((ComboBox) base.ControlDictionary["fontSizeComboBox"]).Text = value.Size.ToString();
                ((ComboBox) base.ControlDictionary["fontListComboBox"]).SelectedIndex = num;
                this.UpdateFontPreviewLabel(this, EventArgs.Empty);
            }
        }

        public string CurrentFontString
        {
            get
            {
                return this.CurrentFont.ToString();
            }
            set
            {
                this.CurrentFont = ParseFont(value);
            }
        }

        private class FontDescriptor
        {
            private FontFamily fontFamily;
            private bool initializedMonospace = false;
            private bool isMonospaced = false;

            public FontDescriptor(FontFamily fontFamily)
            {
                this.fontFamily = fontFamily;
            }

            private bool GetIsMonospaced(FontFamily fontFamily)
            {
                bool flag;
                using (Bitmap bitmap = new Bitmap(1, 1))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        using (Font font = new Font(fontFamily, 10f))
                        {
                            int width = (int) graphics.MeasureString("i.", font).Width;
                            int num2 = (int) graphics.MeasureString("mw", font).Width;
                            flag = width == num2;
                        }
                    }
                }
                return flag;
            }

            public bool IsMonospaced
            {
                get
                {
                    if (!this.initializedMonospace)
                    {
                        this.isMonospaced = this.GetIsMonospaced(this.fontFamily);
                    }
                    return this.isMonospaced;
                }
            }

            public string Name
            {
                get
                {
                    return this.fontFamily.Name;
                }
            }
        }
    }
}

