namespace SkyMap.Net.Gui
{
    using System;

    public interface IProgressMonitor
    {
        void BeginTask(string name, int totalWork);
        void Done();

        string TaskName { get; set; }

        int WorkDone { get; set; }
    }
}

