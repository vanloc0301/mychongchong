namespace SkyMap.Net.Criteria.Client
{
    using System;
    using System.Data;

    public class CriteriaEventArgs : EventArgs
    {
        private System.Data.DataSet dataSet;

        public System.Data.DataSet DataSet
        {
            get
            {
                return this.dataSet;
            }
            set
            {
                this.dataSet = value;
            }
        }
    }
}

