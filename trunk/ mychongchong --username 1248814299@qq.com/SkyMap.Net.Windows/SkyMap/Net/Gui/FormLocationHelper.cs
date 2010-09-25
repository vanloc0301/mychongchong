namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public static class FormLocationHelper
    {
        public static void Apply(Form form, string propertyName, bool isResizable)
        {
            form.StartPosition = FormStartPosition.Manual;
            if (isResizable)
            {
                form.Bounds = Validate(PropertyService.Get<Rectangle>(propertyName, GetDefaultBounds(form)));
            }
            else
            {
                form.Location = Validate(PropertyService.Get<Point>(propertyName, GetDefaultLocation(form)), form.Size);
            }
            form.Closing += delegate {
                if (isResizable)
                {
                    PropertyService.Set<Rectangle>(propertyName, form.Bounds);
                }
                else
                {
                    PropertyService.Set<Point>(propertyName, form.Location);
                }
            };
        }

        private static Rectangle GetDefaultBounds(Form form)
        {
            return new Rectangle(GetDefaultLocation(form), form.Size);
        }

        private static Point GetDefaultLocation(Form form)
        {
            Rectangle bounds = WorkbenchSingleton.MainForm.Bounds;
            Size size = form.Size;
            return new Point(bounds.Left + ((bounds.Width - size.Width) / 2), bounds.Top + ((bounds.Height - size.Height) / 2));
        }

        private static Rectangle Validate(Rectangle bounds)
        {
            Rectangle workingArea = Screen.FromPoint(new Point(bounds.X, bounds.Y)).WorkingArea;
            Rectangle rectangle2 = Screen.FromPoint(new Point(bounds.X + bounds.Width, bounds.Y)).WorkingArea;
            if ((bounds.Y < (workingArea.Y - 5)) && (bounds.Y < (rectangle2.Y - 5)))
            {
                bounds.Y = workingArea.Y - 5;
            }
            if (bounds.X < (workingArea.X - (bounds.Width / 2)))
            {
                bounds.X = workingArea.X - (bounds.Width / 2);
                return bounds;
            }
            if (bounds.X > (rectangle2.Right - (bounds.Width / 2)))
            {
                bounds.X = rectangle2.Right - (bounds.Width / 2);
            }
            return bounds;
        }

        private static Point Validate(Point location, Size size)
        {
            return Validate(new Rectangle(location, size)).Location;
        }
    }
}

