namespace SkyMap.Net.Workflow.Client
{
    using System;

    [Serializable]
    public class BeforePassEventArgs : EventArgs
    {
        private string selectedTranId;
        private string toStaffId;
        private string toStaffName;

        public string SelectedTranId
        {
            get
            {
                return this.selectedTranId;
            }
            set
            {
                this.selectedTranId = value;
            }
        }

        public string ToStaffId
        {
            get
            {
                return this.toStaffId;
            }
            set
            {
                this.toStaffId = value;
            }
        }

        public string ToStaffName
        {
            get
            {
                return this.toStaffName;
            }
            set
            {
                this.toStaffName = value;
            }
        }
    }
}

