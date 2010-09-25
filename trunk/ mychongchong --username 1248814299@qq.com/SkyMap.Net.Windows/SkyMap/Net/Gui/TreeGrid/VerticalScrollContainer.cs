namespace SkyMap.Net.Gui.TreeGrid
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class VerticalScrollContainer : Control
    {
        private ScrollButtonControl down;
        private IScrollable scrollable;
        private int scrollSpeed;
        private bool showButtonsOnlyIfRequired;
        private Timer timer;
        private ScrollButtonControl up;

        public VerticalScrollContainer()
        {
            EventHandler handler = null;
            MouseEventHandler handler2 = null;
            MouseEventHandler handler3 = null;
            this.showButtonsOnlyIfRequired = true;
            this.down = new ScrollButtonControl();
            this.up = new ScrollButtonControl();
            this.timer = new Timer();
            this.scrollSpeed = 5;
            this.up.Arrow = ScrollButton.Up;
            this.down.Arrow = ScrollButton.Down;
            this.up.Dock = DockStyle.Top;
            this.down.Dock = DockStyle.Bottom;
            base.TabStop = false;
            base.SetStyle(ControlStyles.Selectable, false);
            base.Controls.Add(this.up);
            base.Controls.Add(this.down);
            this.UpdateEnabled();
            this.timer.Interval = 50;
            if (handler == null)
            {
                handler = delegate {
                    this.ScrollBy((int) this.timer.Tag);
                };
            }
            this.timer.Tick += handler;
            if (handler2 == null)
            {
                handler2 = delegate {
                    this.timer.Tag = -this.scrollSpeed;
                    this.ScrollBy(-this.scrollSpeed);
                    this.timer.Start();
                };
            }
            this.up.MouseDown += handler2;
            if (handler3 == null)
            {
                handler3 = delegate {
                    this.timer.Tag = this.scrollSpeed;
                    this.ScrollBy(this.scrollSpeed);
                    this.timer.Start();
                };
            }
            this.down.MouseDown += handler3;
            this.up.MouseUp += new MouseEventHandler(this.StopTimer);
            this.down.MouseUp += new MouseEventHandler(this.StopTimer);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.timer.Dispose();
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if ((this.scrollable == null) && !base.DesignMode)
            {
                this.scrollable = e.Control as IScrollable;
                if (this.scrollable != null)
                {
                    this.scrollable.MouseWheel += new MouseEventHandler(this.ScrollableWheel);
                    base.Controls.SetChildIndex(e.Control, 0);
                    this.UpdateEnabled();
                }
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (this.scrollable == e.Control)
            {
                this.scrollable.MouseWheel -= new MouseEventHandler(this.ScrollableWheel);
                this.scrollable = null;
                this.UpdateEnabled();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            base.BeginInvoke(new MethodInvoker(this.PerformLayout));
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.UpdateEnabled();
        }

        private void ScrollableWheel(object sender, MouseEventArgs e)
        {
            this.ScrollBy(-e.Delta / 3);
        }

        private void ScrollBy(int amount)
        {
            this.scrollable.ScrollOffsetY = Math.Max(0, Math.Min((int) (this.scrollable.ScrollOffsetY + amount), (int) (this.scrollable.ScrollHeightY - this.scrollable.Height)));
            this.UpdateEnabled();
        }

        private void StopTimer(object sender, MouseEventArgs e)
        {
            this.timer.Stop();
        }

        private void UpdateEnabled()
        {
            if (this.scrollable == null)
            {
                this.up.Visible = this.down.Visible = true;
                this.up.Enabled = this.down.Enabled = false;
            }
            else
            {
                int scrollHeightY = this.scrollable.ScrollHeightY;
                if (this.showButtonsOnlyIfRequired)
                {
                    if (scrollHeightY <= base.Height)
                    {
                        this.scrollable.ScrollOffsetY = 0;
                        this.up.Visible = this.down.Visible = false;
                        return;
                    }
                    this.up.Visible = this.down.Visible = true;
                }
                else
                {
                    this.up.Visible = this.down.Visible = true;
                    if (this.scrollable.ScrollHeightY <= this.scrollable.Height)
                    {
                        this.scrollable.ScrollOffsetY = 0;
                        this.up.Enabled = this.down.Enabled = false;
                        return;
                    }
                }
                this.up.Enabled = this.scrollable.ScrollOffsetY > 0;
                this.down.Enabled = this.scrollable.ScrollOffsetY < (scrollHeightY - this.scrollable.Height);
            }
        }

        public int ScrollSpeed
        {
            get
            {
                return this.scrollSpeed;
            }
            set
            {
                this.scrollSpeed = value;
            }
        }

        public bool ShowButtonsOnlyIfRequired
        {
            get
            {
                return this.showButtonsOnlyIfRequired;
            }
            set
            {
                if (this.showButtonsOnlyIfRequired != value)
                {
                    this.showButtonsOnlyIfRequired = value;
                    this.UpdateEnabled();
                }
            }
        }

        public interface IScrollable
        {
            event MouseEventHandler MouseWheel;

            int Height { get; }

            int ScrollHeightY { get; }

            int ScrollOffsetY { get; set; }
        }
    }
}

