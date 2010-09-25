namespace SkyMap.Net.Gui
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class AutoHideContainer : Panel
    {
        private int activatorHeight;
        private bool autoHide;
        protected Control control;
        protected bool mouseIn;
        private bool showOnMouseDown;
        private bool showOnMouseMove;
        private bool showOverlay;

        public AutoHideContainer(Control control)
        {
            MouseEventHandler handler = null;
            MouseEventHandler handler2 = null;
            this.autoHide = true;
            this.showOverlay = false;
            this.showOnMouseMove = true;
            this.showOnMouseDown = true;
            this.activatorHeight = 1;
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            this.control = control;
            if (handler == null)
            {
                handler = delegate {
                    if (this.showOnMouseMove)
                    {
                        this.ShowOverlay = true;
                    }
                };
            }
            base.MouseMove += handler;
            if (handler2 == null)
            {
                handler2 = delegate {
                    if (this.showOnMouseDown)
                    {
                        this.ShowOverlay = true;
                    }
                };
            }
            base.MouseDown += handler2;
            control.MouseEnter += new EventHandler(this.OnControlMouseEnter);
            control.MouseLeave += new EventHandler(this.OnControlMouseLeave);
            this.Reformat();
        }

        protected virtual void OnControlMouseEnter(object sender, EventArgs e)
        {
            this.mouseIn = true;
        }

        protected virtual void OnControlMouseLeave(object sender, EventArgs e)
        {
            this.mouseIn = false;
            this.ShowOverlay = false;
        }

        protected virtual void Reformat()
        {
            if (this.autoHide)
            {
                if (this.showOverlay)
                {
                    base.Height = this.activatorHeight;
                    this.control.Dock = DockStyle.None;
                    this.control.Size = new Size(base.Width, this.control.PreferredSize.Height);
                    if (this.Dock != DockStyle.Bottom)
                    {
                        this.control.Location = new Point(base.Left, base.Top);
                    }
                    else
                    {
                        this.control.Location = new Point(base.Left, (base.Top - this.control.PreferredSize.Height) + 1);
                    }
                    base.Parent.Controls.Add(this.control);
                    this.control.BringToFront();
                }
                else
                {
                    base.Height = this.activatorHeight;
                    this.control.Dock = DockStyle.None;
                    this.control.Size = new Size(base.Width, 1);
                    this.control.Location = new Point(0, this.activatorHeight);
                    base.Controls.Add(this.control);
                }
            }
            else
            {
                base.Height = this.PreferredHeight;
                this.control.Dock = DockStyle.Fill;
                base.Controls.Add(this.control);
            }
        }

        public Color ActivatorColor
        {
            get
            {
                return this.ForeColor;
            }
            set
            {
                this.ForeColor = value;
            }
        }

        public int ActivatorHeight
        {
            get
            {
                return this.activatorHeight;
            }
            set
            {
                this.activatorHeight = value;
            }
        }

        public virtual bool AutoHide
        {
            get
            {
                return this.autoHide;
            }
            set
            {
                this.autoHide = value;
                this.Reformat();
            }
        }

        protected virtual int PreferredHeight
        {
            get
            {
                return this.control.PreferredSize.Height;
            }
        }

        public bool ShowOnMouseDown
        {
            get
            {
                return this.showOnMouseDown;
            }
            set
            {
                this.showOnMouseDown = value;
            }
        }

        public bool ShowOnMouseMove
        {
            get
            {
                return this.showOnMouseMove;
            }
            set
            {
                this.showOnMouseMove = value;
            }
        }

        public bool ShowOverlay
        {
            get
            {
                return this.showOverlay;
            }
            set
            {
                this.showOverlay = value;
                this.Reformat();
            }
        }
    }
}

