namespace SkyMap.Net.Gui.Components
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class SmForm : XtraForm
    {
        private bool haveAttachIme = false;
        private const int IME_CHOTKEY_SHAPE_TOGGLE = 0x11;
        private const int IME_CMODE_FULLSHAPE = 8;

        static SmForm()
        {
            Localizer.Active = new ChineseControlLocalizer();
        }

        public SmForm()
        {
            base.LookAndFeel.UseDefaultLookAndFeel = false;
            base.LookAndFeel.UseWindowsXPTheme = false;
            base.LookAndFeel.SetSkinStyle(LookAndFeelHelper.SkinStyle);
            this.DoubleBuffered = true;
            try
            {
                base.ImeMode = ImeMode.OnHalf;
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                base.ImeModeChanged += new EventHandler(this.SmForm_ImeModeChanged);
            }
        }

        protected virtual InputLanguage GetInputLanguage()
        {
            string str = PropertyService.Get<string>("InputLanguage", string.Empty);
            if (!string.IsNullOrEmpty(str))
            {
                foreach (InputLanguage language in InputLanguage.InstalledInputLanguages)
                {
                    if (language.LayoutName == str)
                    {
                        return language;
                    }
                }
            }
            if (InputLanguage.CurrentInputLanguage != null)
            {
                return InputLanguage.CurrentInputLanguage;
            }
            return InputLanguage.DefaultInputLanguage;
        }

        [DllImport("imm32.dll")]
        public static extern IntPtr ImmGetContext(IntPtr hwnd);
        [DllImport("imm32.dll")]
        public static extern bool ImmGetConversionStatus(IntPtr himc, ref int lpdw, ref int lpdw2);
        [DllImport("imm32.dll")]
        public static extern bool ImmGetOpenStatus(IntPtr himc);
        [DllImport("imm32.dll")]
        public static extern bool ImmSetOpenStatus(IntPtr himc, bool b);
        [DllImport("imm32.dll")]
        public static extern int ImmSimulateHotKey(IntPtr hwnd, int lngHotkey);
        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.ClientSize = new Size(0x124, 0x111);
            base.Name = "SmForm";
            base.ResumeLayout(false);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        private void SmForm_ImeModeChanged(object sender, EventArgs e)
        {
            try
            {
                IntPtr himc = ImmGetContext(base.Handle);
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("输入法BUG补丁");
                }
                if (ImmGetOpenStatus(himc))
                {
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("打开了输入法");
                    }
                    int lpdw = 0;
                    int num2 = 0;
                    if (ImmGetConversionStatus(himc, ref lpdw, ref num2))
                    {
                        if (LoggingService.IsInfoEnabled)
                        {
                            LoggingService.Info("检索输入法信息成功");
                        }
                        if ((lpdw & 8) > 0)
                        {
                            if (LoggingService.IsInfoEnabled)
                            {
                                LoggingService.Info("状态是全角，要将其改为半角");
                            }
                            ImmSimulateHotKey(base.Handle, 0x11);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }
    }
}

