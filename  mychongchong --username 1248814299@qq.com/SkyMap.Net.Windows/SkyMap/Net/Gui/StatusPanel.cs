namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class StatusPanel : UserControl
    {
        private Font boldFont = ResourceService.LoadFont("Tahoma", 14, FontStyle.Bold, GraphicsUnit.World);
        private Font normalFont = ResourceService.LoadFont("Tahoma", 14, GraphicsUnit.World);
        private Font smallFont = ResourceService.LoadFont("Tahoma", 14, GraphicsUnit.World);
        private WizardDialog wizard;

        public StatusPanel(WizardDialog wizard)
        {
            this.wizard = wizard;
            this.BackgroundImage = ResourceService.GetBitmap("GeneralWizardBackground");
            base.Size = new Size(0xc6, 400);
            base.ResizeRedraw = false;
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics graphics = pe.Graphics;
            graphics.DrawString(ResourceService.GetString("SharpDevelop.Gui.Dialogs.WizardDialog.StepsLabel"), this.smallFont, SystemBrushes.WindowText, 10f, (float) (0x18 - this.smallFont.Height));
            graphics.DrawLine(SystemPens.WindowText, 10, 0x18, base.Width - 10, 0x18);
            int num = 0;
            for (int i = 0; i < this.wizard.WizardPanels.Count; i = this.wizard.GetSuccessorNumber(i))
            {
                Font font = (this.wizard.ActivePanelNumber == i) ? this.boldFont : this.normalFont;
                IDialogPanelDescriptor descriptor = this.wizard.WizardPanels[i];
                graphics.DrawString((1 + num) + ". " + descriptor.Label, font, SystemBrushes.WindowText, 10f, (float) (40 + (num * font.Height)));
                num++;
            }
        }
    }
}

