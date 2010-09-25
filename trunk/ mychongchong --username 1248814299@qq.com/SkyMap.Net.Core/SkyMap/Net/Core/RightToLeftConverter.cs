namespace SkyMap.Net.Core
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public static class RightToLeftConverter
    {
        public static string[] RightToLeftLanguages = new string[] { "ar", "he", "fa", "urdu" };

        public static void Convert(Control control)
        {
            if (IsRightToLeft)
            {
                if (control.RightToLeft != RightToLeft.Yes)
                {
                    control.RightToLeft = RightToLeft.Yes;
                }
            }
            else if (control.RightToLeft == RightToLeft.Yes)
            {
                control.RightToLeft = RightToLeft.No;
            }
            ConvertLayout(control);
        }

        private static void ConvertLayout(Control control)
        {
            bool isRightToLeft = IsRightToLeft;
            DateTimePicker picker = control as DateTimePicker;
            Form form = control as Form;
            ListView view = control as ListView;
            ProgressBar bar = control as ProgressBar;
            TabControl control2 = control as TabControl;
            TrackBar bar2 = control as TrackBar;
            TreeView view2 = control as TreeView;
            if ((form != null) && (form.RightToLeftLayout != isRightToLeft))
            {
                form.RightToLeftLayout = isRightToLeft;
            }
            if ((view != null) && (view.RightToLeftLayout != isRightToLeft))
            {
                view.RightToLeftLayout = isRightToLeft;
            }
            if ((bar != null) && (bar.RightToLeftLayout != isRightToLeft))
            {
                bar.RightToLeftLayout = isRightToLeft;
            }
            if ((control2 != null) && (control2.RightToLeftLayout != isRightToLeft))
            {
                control2.RightToLeftLayout = isRightToLeft;
            }
            if ((bar2 != null) && (bar2.RightToLeftLayout != isRightToLeft))
            {
                bar2.RightToLeftLayout = isRightToLeft;
            }
            if ((view2 != null) && (view2.RightToLeftLayout != isRightToLeft))
            {
                view2.RightToLeftLayout = isRightToLeft;
            }
        }

        private static void ConvertLayoutRecursive(Control control)
        {
            if (IsRightToLeft == (control.RightToLeft == RightToLeft.Yes))
            {
                ConvertLayout(control);
                foreach (Control control2 in control.Controls)
                {
                    ConvertLayoutRecursive(control2);
                }
            }
        }

        public static void ConvertRecursive(Control control)
        {
            if (IsRightToLeft != (control.RightToLeft == RightToLeft.Yes))
            {
                ReConvertRecursive(control);
            }
        }

        private static AnchorStyles Mirror(AnchorStyles anchor)
        {
            bool flag = (anchor & AnchorStyles.Right) == AnchorStyles.Right;
            bool flag2 = (anchor & AnchorStyles.Left) == AnchorStyles.Left;
            if (flag)
            {
                anchor |= AnchorStyles.Left;
            }
            else
            {
                anchor &= ~AnchorStyles.Left;
            }
            if (flag2)
            {
                anchor |= AnchorStyles.Right;
                return anchor;
            }
            anchor &= ~AnchorStyles.Right;
            return anchor;
        }

        private static void Mirror(Control control)
        {
            switch (control.Dock)
            {
                case DockStyle.None:
                    control.Anchor = Mirror(control.Anchor);
                    control.Location = MirrorLocation(control);
                    break;

                case DockStyle.Left:
                    control.Dock = DockStyle.Right;
                    break;

                case DockStyle.Right:
                    control.Dock = DockStyle.Left;
                    break;
            }
            if (control.RightToLeft == RightToLeft.Yes)
            {
                foreach (Control control2 in control.Controls)
                {
                    Mirror(control2);
                }
            }
        }

        private static Point MirrorLocation(Control control)
        {
            return new Point((control.Parent.ClientSize.Width - control.Left) - control.Width, control.Top);
        }

        public static void ReConvertRecursive(Control control)
        {
            Convert(control);
            foreach (Control control2 in control.Controls)
            {
                ConvertLayoutRecursive(control2);
            }
            if (IsRightToLeft)
            {
                if (control is Form)
                {
                    foreach (Control control2 in control.Controls)
                    {
                        foreach (Control control3 in control2.Controls)
                        {
                            Mirror(control3);
                        }
                    }
                }
                else
                {
                    foreach (Control control2 in control.Controls)
                    {
                        Mirror(control2);
                    }
                }
            }
        }

        public static bool IsRightToLeft
        {
            get
            {
                foreach (string str in RightToLeftLanguages)
                {
                    if (ResourceService.Language.StartsWith(str))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}

