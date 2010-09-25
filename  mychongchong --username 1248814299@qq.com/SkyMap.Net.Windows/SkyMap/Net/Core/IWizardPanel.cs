namespace SkyMap.Net.Core
{
    using System;
    using System.Runtime.CompilerServices;

    public interface IWizardPanel : IDialogPanel
    {
        event EventHandler EnableCancelChanged;

        event EventHandler EnableNextChanged;

        event EventHandler EnablePreviousChanged;

        event EventHandler FinishPanelRequested;

        event EventHandler IsLastPanelChanged;

        event EventHandler NextWizardPanelIDChanged;

        bool EnableCancel { get; }

        bool EnableNext { get; }

        bool EnablePrevious { get; }

        bool IsLastPanel { get; }

        string NextWizardPanelID { get; }
    }
}

