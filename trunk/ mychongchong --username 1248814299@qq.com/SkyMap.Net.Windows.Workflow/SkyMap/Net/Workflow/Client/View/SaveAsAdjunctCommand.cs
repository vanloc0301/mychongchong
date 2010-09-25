namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Instance;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class SaveAsAdjunctCommand : AdjunctEditCommand
    {
        public override void Run()
        {
            List<WfAdjunct> selectedAdjuncts = base.ae.GetSelectedAdjuncts();
            if (selectedAdjuncts.Count != 0)
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    WaitDialogHelper.Show();
                    try
                    {
                        string selectedPath = dialog.SelectedPath;
                        foreach (WfAdjunct adjunct in selectedAdjuncts)
                        {
                            base.SaveFile(base.GetAdjunctFile(adjunct), selectedPath);
                        }
                    }
                    finally
                    {
                        WaitDialogHelper.Close();
                    }
                }
            }
        }

        public override bool IsEnabled
        {
            get
            {
                return (base.ae.GetFocusAdjunct() != null);
            }
            set
            {
            }
        }
    }
}

