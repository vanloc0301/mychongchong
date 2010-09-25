namespace SkyMap.Net.DataForms
{
    using System;

    public class DataFormCallBackEventArgs
    {
        private object _dataFormCallBackInfo;

        public object DataFormCallBackInfo
        {
            get
            {
                return this._dataFormCallBackInfo;
            }
            set
            {
                this._dataFormCallBackInfo = value;
            }
        }
    }
}

