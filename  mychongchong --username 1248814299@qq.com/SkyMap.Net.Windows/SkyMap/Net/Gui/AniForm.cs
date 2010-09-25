namespace SkyMap.Net.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class AniForm : Form
    {
        private Container components = null;
        private const int GWL_EXSTYLE = -20;
        private const int GWL_STYLE = -16;
        private const int HWND_TOPMOST = -1;
        private bool isMouseDown = false;
        private bool m_bActivated;
        private bool m_bAnimating;
        private AniForm m_baseForm = null;
        private bool m_bAutoDispose;
        private bool m_bCloseRequested;
        private SkyMap.Net.Gui.BackgroundMode m_bgMode;
        private SkyMap.Net.Gui.BorderStyle m_borderStyle;
        private bool m_bPersistent;
        private bool m_bSavedBounds;
        private Color m_colorEnd;
        private Color m_colorStart;
        private AnimateDirection m_direction;
        private AutoResetEvent m_eventClosed;
        private ManualResetEvent m_eventNotifyClosed;
        private int m_iAdjSpeed;
        private int m_iBorderWidth;
        private int m_iCalcSpeed;
        private int m_iDelay;
        private int m_iDelta;
        private int m_iGradientSize;
        private int m_iInterval;
        private int m_iLastDelta;
        private int m_iSpeed;
        private Rectangle m_oldBounds;
        private Point m_origLocation;
        private FormPlacement m_placement;
        private SkyMap.Net.Gui.StackMode m_stackMode = SkyMap.Net.Gui.StackMode.None;
        private Point m_startLocation;
        private WndMover m_wndMover = null;
        private Point mouseOffset;
        private static StackArray s_currentForms = new StackArray();
        private const int SW_HIDE = 0;
        private const int SW_SHOWNOACTIVATE = 4;
        private const uint SWP_FRAMECHANGED = 0x20;
        private const uint SWP_HIDEWINDOW = 0x80;
        private const uint SWP_NOACTIVATE = 0x10;
        private const uint SWP_NOCOPYBITS = 0x100;
        private const uint SWP_NOMOVE = 2;
        private const uint SWP_NOOWNERZORDER = 0x200;
        private const uint SWP_NOREDRAW = 8;
        private const uint SWP_NOSENDCHANGING = 0x400;
        private const uint SWP_NOSIZE = 1;
        private const uint SWP_NOZORDER = 4;
        private const uint SWP_SHOWWINDOW = 0x40;
        private const int WS_BORDER = 0x800000;
        private const int WS_CAPTION = 0xc00000;
        private const int WS_EX_APPWINDOW = 0x40000;

        public event EventHandler AnimatingDone;

        public event EventHandler Expanded;

        public AniForm()
        {
            this.CommonConstruction();
        }

        private void AddToStack()
        {
            if (this.m_stackMode != SkyMap.Net.Gui.StackMode.None)
            {
                this.m_baseForm = (AniForm) s_currentForms.Push(this, this.m_stackMode);
            }
        }

        private void AniFunc()
        {
            try
            {
                this.Expand();
                if (this.WaitForCloseRequest(this.Delay))
                {
                    throw new CloseRequestedException();
                }
                if (!this.IsActivated)
                {
                    this.Contract();
                }
            }
            catch (CloseRequestedException)
            {
                this.CloseRequested = false;
            }
            if (!this.IsActivated)
            {
                this.EndAnimation();
            }
            this.CloseRequested = false;
        }

        public void Animate()
        {
            if (!this.Animating)
            {
                this.m_eventNotifyClosed.Reset();
                if (!this.m_bSavedBounds)
                {
                    if (this.BackgroundMode == SkyMap.Net.Gui.BackgroundMode.GradientVertical)
                    {
                        this.m_iGradientSize = base.ClientRectangle.Height;
                    }
                    else if (this.BackgroundMode == SkyMap.Net.Gui.BackgroundMode.GradientHorizontal)
                    {
                        this.m_iGradientSize = base.ClientRectangle.Width;
                    }
                    this.m_oldBounds = base.Bounds;
                    this.m_bSavedBounds = true;
                    this.m_origLocation = this.StartLocation;
                }
                this.ResetPosition();
                this.Calculate();
                this.AddToStack();
                try
                {
                    this.InitLocation();
                    this.ResetPosition();
                    this.Animating = true;
                    SetWindowPos(base.Handle, (IntPtr) (-1), 0, 0, 0, 0, 0x93);
                    ShowWindow(base.Handle, 4);
                    ThreadStart start = new ThreadStart(this.AniFunc);
                    new Thread(start).Start();
                }
                catch (OffDisplayException exception)
                {
                    this.RemoveFromStack();
                    exception.GetHashCode();
                }
            }
        }

        protected void Calculate()
        {
            int num = 0;
            int width = 0;
            if (this.Direction == AnimateDirection.LeftToRight)
            {
                num = base.Bounds.Width + 1;
                width = this.m_oldBounds.Width;
            }
            else
            {
                num = base.Bounds.Height + 1;
                width = this.m_oldBounds.Height;
            }
            this.m_iCalcSpeed = 90 - this.Speed;
            this.m_iAdjSpeed = this.m_iCalcSpeed;
            this.m_iLastDelta = (width - num) % this.m_iCalcSpeed;
            this.m_iDelta = ((width - num) - this.m_iLastDelta) / this.m_iCalcSpeed;
            if (this.m_iLastDelta > this.m_iDelta)
            {
                this.m_iAdjSpeed = this.m_iCalcSpeed + ((this.m_iLastDelta - (this.m_iLastDelta % this.m_iDelta)) / this.m_iDelta);
                this.m_iLastDelta = this.m_iLastDelta % this.m_iDelta;
            }
        }

        private void CommonConstruction()
        {
            this.InitializeComponent();
            this.m_eventClosed = new AutoResetEvent(false);
            this.m_eventNotifyClosed = new ManualResetEvent(false);
            this.m_direction = AnimateDirection.BottomToTop;
            this.m_iSpeed = 40;
            this.m_colorStart = Color.FromArgb(0xff, 0xa8, 0xa8, 0xff);
            this.m_colorEnd = SystemColors.Window;
            this.m_bgMode = SkyMap.Net.Gui.BackgroundMode.GradientVertical;
            this.m_iDelay = 0x1388;
            this.m_bCloseRequested = false;
            this.m_iInterval = 10;
            this.InitCenterLocation();
            this.m_startLocation = base.Location;
            this.m_bSavedBounds = false;
            this.m_bActivated = false;
            this.m_borderStyle = SkyMap.Net.Gui.BorderStyle.None;
            this.m_iBorderWidth = 0;
            this.m_wndMover = new WndMover(this.DoMove);
            this.m_bAutoDispose = false;
        }

        private void Contract()
        {
            for (int i = 0; i < this.m_iAdjSpeed; i++)
            {
                this.ThreadSafeResize(false, this.m_iDelta);
                Thread.Sleep(this.m_iInterval);
            }
            this.ThreadSafeResize(false, this.m_iLastDelta);
            this.RemoveFromStack();
            if (!(base.IsDisposed || !this.m_bAutoDispose))
            {
                base.Close();
                base.Dispose();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DoMove(int iDelta)
        {
            Rectangle rectangle;
            if (this.Direction == AnimateDirection.LeftToRight)
            {
                rectangle = new Rectangle(base.Bounds.Left, base.Bounds.Top, base.Bounds.Width + iDelta, base.Bounds.Height);
            }
            else
            {
                rectangle = new Rectangle(base.Bounds.Left, base.Bounds.Top - iDelta, base.Bounds.Width, base.Bounds.Height + iDelta);
            }
            base.SetBounds(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
            if (this.Direction == AnimateDirection.LeftToRight)
            {
                base.Invalidate(base.RectangleToClient(new Rectangle(((rectangle.Right - iDelta) - this.m_iBorderWidth) - 2, rectangle.Top, (iDelta + this.m_iBorderWidth) + 2, rectangle.Height)));
            }
            else
            {
                base.Invalidate(base.RectangleToClient(new Rectangle(rectangle.Left, ((rectangle.Bottom - iDelta) - this.m_iBorderWidth) - 2, rectangle.Width, iDelta + this.m_iBorderWidth)));
            }
        }

        private void EndAnimation()
        {
            if (base.Visible)
            {
                try
                {
                    ShowWindow(base.Handle, 0);
                }
                catch
                {
                }
            }
            this.Animating = false;
            this.OnAnimatingDone(EventArgs.Empty);
            if (!(base.IsDisposed || !this.m_bAutoDispose))
            {
                base.Close();
                base.Dispose();
            }
        }

        private void Expand()
        {
            for (int i = 0; i < this.m_iAdjSpeed; i++)
            {
                this.ThreadSafeResize(true, this.m_iDelta);
                Thread.Sleep(this.m_iInterval);
            }
            this.ThreadSafeResize(true, this.m_iLastDelta);
            this.OnExpanded(EventArgs.Empty);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int Offset);
        private void InitCenterLocation()
        {
            AppBarInfo info = new AppBarInfo();
            Rectangle workArea = info.WorkArea;
            base.Location = new Point((workArea.Left + (workArea.Width / 2)) - (this.m_oldBounds.Width / 2), (workArea.Top + (workArea.Height / 2)) + (this.m_oldBounds.Height / 2));
        }

        private void InitializeComponent()
        {
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xd0, 0xb8);
            base.ControlBox = false;
            base.FormBorderStyle = FormBorderStyle.None;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AniForm";
            base.ShowInTaskbar = false;
            this.Text = "AniForm";
        }

        private void InitLocation()
        {
            if (base.Handle != IntPtr.Zero)
            {
                if (this.m_baseForm != null)
                {
                    Point point;
                    if (this.Direction == AnimateDirection.LeftToRight)
                    {
                        point = new Point((this.m_baseForm.StartLocation.X + this.m_oldBounds.Width) + 1, this.m_baseForm.StartLocation.Y);
                    }
                    else
                    {
                        point = new Point(this.m_baseForm.StartLocation.X, (this.m_baseForm.StartLocation.Y - this.m_oldBounds.Height) - 1);
                    }
                    Rectangle bounds = Screen.PrimaryScreen.Bounds;
                    if ((point.X > bounds.Right) || (point.Y < bounds.Top))
                    {
                        throw new OffDisplayException();
                    }
                    base.Location = point;
                }
                else if (this.Placement == FormPlacement.Tray)
                {
                    this.InitTrayLocation();
                }
                else if (this.Placement == FormPlacement.Centered)
                {
                    this.InitCenterLocation();
                }
                else if (this.Placement == FormPlacement.Normal)
                {
                    base.Location = this.m_origLocation;
                }
                this.StartLocation = base.Location;
            }
        }

        private void InitTrayLocation()
        {
            AppBarInfo info = new AppBarInfo();
            info.GetSystemTaskBarPosition();
            Rectangle workArea = info.WorkArea;
            int x = 0;
            int y = 0;
            if (info.Edge == AppBarInfo.ScreenEdge.Left)
            {
                x = workArea.Left + 2;
                y = (workArea.Bottom - base.Bounds.Height) - 5;
            }
            else if (info.Edge == AppBarInfo.ScreenEdge.Bottom)
            {
                x = (workArea.Right - this.m_oldBounds.Width) - 5;
                y = (workArea.Bottom - base.Bounds.Height) - 1;
            }
            else if (info.Edge == AppBarInfo.ScreenEdge.Top)
            {
                x = (workArea.Right - this.m_oldBounds.Width) - 5;
                y = (workArea.Top + this.m_oldBounds.Height) + 1;
            }
            else if (info.Edge == AppBarInfo.ScreenEdge.Right)
            {
                x = (workArea.Right - this.m_oldBounds.Width) - 5;
                y = (workArea.Bottom - base.Bounds.Height) - 1;
            }
            SetWindowPos(base.Handle, (IntPtr) (-1), x, y, 0, 0, 0x91);
        }

        protected override void OnActivated(EventArgs e)
        {
            this.IsActivated = true;
            base.OnActivated(e);
        }

        protected virtual void OnAnimatingDone(EventArgs e)
        {
            if (this.AnimatingDone != null)
            {
                this.AnimatingDone(this, e);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            this.Animating = false;
            this.RemoveFromStack();
            base.OnClosed(e);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            if (!(!this.IsActivated || this.Persistent))
            {
                this.Contract();
                this.IsActivated = false;
                this.EndAnimation();
            }
            base.OnDeactivate(e);
        }

        protected virtual void OnExpanded(EventArgs e)
        {
            if (this.Expanded != null)
            {
                this.Expanded(this, e);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.m_oldBounds = base.Bounds;
            base.OnLoad(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = -e.X;
                int y = -e.Y;
                this.mouseOffset = new Point(x, y);
                this.isMouseDown = true;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.isMouseDown)
            {
                Point mousePosition = Control.MousePosition;
                mousePosition.Offset(this.mouseOffset.X, this.mouseOffset.Y);
                base.Location = mousePosition;
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.isMouseDown = false;
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.BackgroundMode != SkyMap.Net.Gui.BackgroundMode.Normal)
            {
                if (base.DesignMode)
                {
                    if (this.BackgroundMode == SkyMap.Net.Gui.BackgroundMode.GradientVertical)
                    {
                        this.m_iGradientSize = base.ClientRectangle.Height;
                    }
                    else
                    {
                        this.m_iGradientSize = base.ClientRectangle.Width;
                    }
                }
                if ((base.ClientRectangle.Height > 0) && (base.ClientRectangle.Width > 0))
                {
                    Rectangle rect = Rectangle.Inflate(base.ClientRectangle, -this.m_iBorderWidth, -this.m_iBorderWidth);
                    if ((rect.Width > 0) && (rect.Height > 0))
                    {
                        LinearGradientBrush brush;
                        if (this.BackgroundMode == SkyMap.Net.Gui.BackgroundMode.GradientVertical)
                        {
                            brush = new LinearGradientBrush(new Rectangle(base.ClientRectangle.Left, base.ClientRectangle.Top, base.ClientRectangle.Width, this.m_iGradientSize), this.StartColor, this.EndColor, LinearGradientMode.Vertical);
                        }
                        else
                        {
                            brush = new LinearGradientBrush(new Rectangle(base.ClientRectangle.Left, base.ClientRectangle.Top, this.m_iGradientSize, base.ClientRectangle.Height), this.StartColor, this.EndColor, LinearGradientMode.Horizontal);
                        }
                        e.Graphics.FillRectangle(brush, rect);
                        brush.Dispose();
                        brush = null;
                    }
                }
            }
            base.OnPaint(e);
            if (this.BorderStyle != SkyMap.Net.Gui.BorderStyle.None)
            {
                Rectangle rectangle2 = new Rectangle(base.ClientRectangle.Left, base.ClientRectangle.Top, base.ClientRectangle.Width - 1, base.ClientRectangle.Height - 1);
                if (this.BorderStyle == SkyMap.Net.Gui.BorderStyle.FixedSingle)
                {
                    if ((rectangle2.Width > 1) && (rectangle2.Height > 1))
                    {
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black), 1f), rectangle2);
                    }
                }
                else if (this.BorderStyle == SkyMap.Net.Gui.BorderStyle.Raised)
                {
                    if ((rectangle2.Width > 1) && (rectangle2.Height > 1))
                    {
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(SystemColors.ControlDark), 1f), rectangle2);
                        rectangle2.Inflate(-1, -1);
                    }
                    if ((rectangle2.Width > 1) && (rectangle2.Height > 1))
                    {
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(SystemColors.ControlLight), 1f), rectangle2);
                        rectangle2.Inflate(-1, -1);
                    }
                    if ((rectangle2.Width > 1) && (rectangle2.Height > 1))
                    {
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(SystemColors.ControlDark), 1f), rectangle2);
                        rectangle2.Inflate(-1, -1);
                    }
                    if ((rectangle2.Width > 1) && (rectangle2.Height > 1))
                    {
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(SystemColors.ControlDarkDark), 1f), rectangle2);
                    }
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            if (base.DesignMode)
            {
                base.Invalidate();
            }
            base.OnResize(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (!base.Visible)
            {
                this.Animating = false;
                this.RemoveFromStack();
            }
            base.OnVisibleChanged(e);
        }

        private void RemoveFromStack()
        {
            s_currentForms.Pop(this, this.StackMode);
            this.m_baseForm = null;
        }

        public void RequestClose()
        {
            if (base.Handle != IntPtr.Zero)
            {
                if (base.Visible)
                {
                    ShowWindow(base.Handle, 0);
                }
                this.m_bActivated = false;
                this.CloseRequested = true;
                this.SetClosedEvent();
            }
        }

        private void ResetClosedEvent()
        {
            this.m_eventClosed.Reset();
        }

        private void ResetPosition()
        {
            if (this.Direction == AnimateDirection.LeftToRight)
            {
                base.SetBounds(base.Bounds.Left, base.Bounds.Top, 1, base.Bounds.Height);
            }
            else
            {
                base.SetBounds(base.Bounds.Left, base.Bounds.Top, base.Bounds.Width, 1);
            }
        }

        private void SetClosedEvent()
        {
            this.m_eventClosed.Set();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hParent);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int Offset, int newLong);
        [DllImport("user32.dll")]
        private static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int x, int y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, int dwFlags);
        protected void ThreadSafeResize(bool bExpand, int iDiff)
        {
            if (this.CloseRequested)
            {
                throw new CloseRequestedException();
            }
            if (!bExpand)
            {
                iDiff = -iDiff;
            }
            object[] args = new object[] { iDiff };
            base.Invoke(this.m_wndMover, args);
        }

        public bool WaitForClose(int iTime)
        {
            return this.m_eventNotifyClosed.WaitOne(iTime, false);
        }

        private bool WaitForCloseRequest(int iTime)
        {
            return (this.Animating && this.m_eventClosed.WaitOne(iTime, false));
        }

        [Browsable(false)]
        public bool Animating
        {
            get
            {
                return this.m_bAnimating;
            }
            set
            {
                lock (this)
                {
                    this.m_bAnimating = value;
                }
            }
        }

        public bool AutoDispose
        {
            get
            {
                return this.m_bAutoDispose;
            }
            set
            {
                this.m_bAutoDispose = value;
            }
        }

        public SkyMap.Net.Gui.BackgroundMode BackgroundMode
        {
            get
            {
                return this.m_bgMode;
            }
            set
            {
                this.m_bgMode = value;
                base.Invalidate();
            }
        }

        public SkyMap.Net.Gui.BorderStyle BorderStyle
        {
            get
            {
                return this.m_borderStyle;
            }
            set
            {
                this.m_borderStyle = value;
                if (this.m_borderStyle == SkyMap.Net.Gui.BorderStyle.Raised)
                {
                    this.m_iBorderWidth = 4;
                }
                else if (this.m_borderStyle == SkyMap.Net.Gui.BorderStyle.FixedSingle)
                {
                    this.m_iBorderWidth = 1;
                }
                else
                {
                    this.m_iBorderWidth = 0;
                }
            }
        }

        [Browsable(false)]
        public bool CloseRequested
        {
            get
            {
                lock (this)
                {
                    return this.m_bCloseRequested;
                }
            }
            set
            {
                lock (this)
                {
                    this.m_bCloseRequested = value;
                }
            }
        }

        public int Delay
        {
            get
            {
                return this.m_iDelay;
            }
            set
            {
                this.m_iDelay = value;
            }
        }

        public AnimateDirection Direction
        {
            get
            {
                return this.m_direction;
            }
            set
            {
                this.m_direction = value;
            }
        }

        public Color EndColor
        {
            get
            {
                return this.m_colorEnd;
            }
            set
            {
                this.m_colorEnd = value;
            }
        }

        public Size FullSize
        {
            get
            {
                return new Size(this.m_oldBounds.Width, this.m_oldBounds.Height);
            }
        }

        [Browsable(false)]
        public bool IsActivated
        {
            get
            {
                lock (this)
                {
                    return this.m_bActivated;
                }
            }
            set
            {
                lock (this)
                {
                    this.m_bActivated = value;
                }
            }
        }

        public bool Persistent
        {
            get
            {
                return this.m_bPersistent;
            }
            set
            {
                this.m_bPersistent = value;
            }
        }

        public FormPlacement Placement
        {
            get
            {
                return this.m_placement;
            }
            set
            {
                this.m_placement = value;
            }
        }

        public int Speed
        {
            get
            {
                return this.m_iSpeed;
            }
            set
            {
                if ((value < 1) || (value > 0x59))
                {
                    throw new ArgumentOutOfRangeException("Speed");
                }
                this.m_iSpeed = value;
            }
        }

        public SkyMap.Net.Gui.StackMode StackMode
        {
            get
            {
                return this.m_stackMode;
            }
            set
            {
                this.m_stackMode = value;
            }
        }

        public Color StartColor
        {
            get
            {
                return this.m_colorStart;
            }
            set
            {
                this.m_colorStart = value;
            }
        }

        public Point StartLocation
        {
            get
            {
                return this.m_startLocation;
            }
            set
            {
                this.m_startLocation = value;
            }
        }
    }
}

