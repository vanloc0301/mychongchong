namespace SkyMap.Net.Core
{
    using System;
    using System.Runtime.CompilerServices;

    public abstract class AbstractCommand : ICommand
    {
        private SkyMap.Net.Core.Codon codon;
        private string id;
        private object owner = null;

        public event EventHandler OwnerChanged;

        protected AbstractCommand()
        {
        }

        protected virtual void OnOwnerChanged(EventArgs e)
        {
            if (this.OwnerChanged != null)
            {
                this.OwnerChanged(this, e);
            }
        }

        public abstract void Run();

        public SkyMap.Net.Core.Codon Codon
        {
            get
            {
                return this.codon;
            }
            set
            {
                this.codon = value;
            }
        }

        public string ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public virtual object Owner
        {
            get
            {
                return this.owner;
            }
            set
            {
                this.owner = value;
                this.OnOwnerChanged(EventArgs.Empty);
            }
        }
    }
}

