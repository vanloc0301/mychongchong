namespace SkyMap.Net.Internal.ExternalTool
{
    using System;
    using System.Xml;

    public class ExternalTool
    {
        private string arguments;
        private string command;
        private string initialDirectory;
        private string menuCommand;
        private bool promptForArguments;
        private bool useOutputPad;

        public ExternalTool()
        {
            this.menuCommand = "New Tool";
            this.command = "";
            this.arguments = "";
            this.initialDirectory = "";
            this.promptForArguments = false;
            this.useOutputPad = false;
        }

        public ExternalTool(XmlElement el)
        {
            this.menuCommand = "New Tool";
            this.command = "";
            this.arguments = "";
            this.initialDirectory = "";
            this.promptForArguments = false;
            this.useOutputPad = false;
            if (el == null)
            {
                throw new ArgumentNullException("ExternalTool(XmlElement el) : el can't be null");
            }
            if ((((el["INITIALDIRECTORY"] == null) || (el["ARGUMENTS"] == null)) || ((el["COMMAND"] == null) || (el["MENUCOMMAND"] == null))) || (el["PROMPTFORARGUMENTS"] == null))
            {
                throw new Exception("ExternalTool(XmlElement el) : INITIALDIRECTORY and ARGUMENTS and COMMAND and MENUCOMMAND and PROMPTFORARGUMENTS attributes must exist.(check the ExternalTool XML)");
            }
            this.InitialDirectory = el["INITIALDIRECTORY"].InnerText;
            this.Arguments = el["ARGUMENTS"].InnerText;
            this.Command = el["COMMAND"].InnerText;
            this.MenuCommand = el["MENUCOMMAND"].InnerText;
            this.PromptForArguments = bool.Parse(el["PROMPTFORARGUMENTS"].InnerText);
            if (el["USEOUTPUTPAD"] != null)
            {
                this.UseOutputPad = bool.Parse(el["USEOUTPUTPAD"].InnerText);
            }
        }

        public override string ToString()
        {
            return this.menuCommand;
        }

        public XmlElement ToXmlElement(XmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("ExternalTool.ToXmlElement(XmlDocument doc) : doc can't be null");
            }
            XmlElement element = doc.CreateElement("TOOL");
            XmlElement newChild = doc.CreateElement("INITIALDIRECTORY");
            newChild.InnerText = this.InitialDirectory;
            element.AppendChild(newChild);
            newChild = doc.CreateElement("ARGUMENTS");
            newChild.InnerText = this.Arguments;
            element.AppendChild(newChild);
            newChild = doc.CreateElement("COMMAND");
            newChild.InnerText = this.command;
            element.AppendChild(newChild);
            newChild = doc.CreateElement("MENUCOMMAND");
            newChild.InnerText = this.MenuCommand;
            element.AppendChild(newChild);
            newChild = doc.CreateElement("PROMPTFORARGUMENTS");
            newChild.InnerText = this.PromptForArguments.ToString();
            element.AppendChild(newChild);
            newChild = doc.CreateElement("USEOUTPUTPAD");
            newChild.InnerText = this.UseOutputPad.ToString();
            element.AppendChild(newChild);
            return element;
        }

        public string Arguments
        {
            get
            {
                return this.arguments;
            }
            set
            {
                this.arguments = value;
            }
        }

        public string Command
        {
            get
            {
                return this.command;
            }
            set
            {
                this.command = value;
            }
        }

        public string InitialDirectory
        {
            get
            {
                return this.initialDirectory;
            }
            set
            {
                this.initialDirectory = value;
            }
        }

        public string MenuCommand
        {
            get
            {
                return this.menuCommand;
            }
            set
            {
                this.menuCommand = value;
            }
        }

        public bool PromptForArguments
        {
            get
            {
                return this.promptForArguments;
            }
            set
            {
                this.promptForArguments = value;
            }
        }

        public bool UseOutputPad
        {
            get
            {
                return this.useOutputPad;
            }
            set
            {
                this.useOutputPad = value;
            }
        }
    }
}

