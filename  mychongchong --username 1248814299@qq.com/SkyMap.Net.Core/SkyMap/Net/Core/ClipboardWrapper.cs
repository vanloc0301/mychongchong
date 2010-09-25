namespace SkyMap.Net.Core
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public static class ClipboardWrapper
    {
        public static IDataObject GetDataObject()
        {
            IDataObject dataObject;
            try
            {
                dataObject = Clipboard.GetDataObject();
            }
            catch (ExternalException)
            {
                try
                {
                    dataObject = Clipboard.GetDataObject();
                }
                catch (ExternalException)
                {
                    dataObject = new DataObject();
                }
            }
            return dataObject;
        }

        public static string GetText()
        {
            try
            {
                return Clipboard.GetText();
            }
            catch (ExternalException)
            {
                return Clipboard.GetText();
            }
        }

        public static void SetDataObject(object data)
        {
            try
            {
                Clipboard.SetDataObject(data, true, 10, 50);
            }
            catch (ExternalException)
            {
                Application.DoEvents();
                try
                {
                    Clipboard.SetDataObject(data, true, 10, 50);
                }
                catch (ExternalException)
                {
                }
            }
        }

        public static void SetText(string text)
        {
            DataObject data = new DataObject();
            data.SetData(DataFormats.UnicodeText, true, text);
            SetDataObject(data);
        }

        public static bool ContainsText
        {
            get
            {
                try
                {
                    LoggingService.Debug("ContainsText called");
                    return Clipboard.ContainsText();
                }
                catch (ExternalException)
                {
                    return false;
                }
            }
        }
    }
}

