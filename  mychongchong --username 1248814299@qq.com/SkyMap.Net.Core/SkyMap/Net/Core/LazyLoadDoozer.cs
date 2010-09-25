namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;

    public class LazyLoadDoozer : IDoozer
    {
        private AddIn addIn;
        private string className;
        private string name;

        public LazyLoadDoozer(AddIn addIn, Properties properties)
        {
            this.addIn = addIn;
            this.name = properties["name"];
            this.className = properties["class"];
        }

        public object BuildItem(object caller, Codon codon, ArrayList subItems)
        {
            IDoozer doozer = (IDoozer) this.addIn.CreateObject(this.className);
            if (doozer == null)
            {
                return null;
            }
            AddInTree.Doozers[this.name] = doozer;
            return doozer.BuildItem(caller, codon, subItems);
        }

        public override string ToString()
        {
            return string.Format("[LazyLoadDoozer: className = {0}, name = {1}]", this.className, this.name);
        }

        public string ClassName
        {
            get
            {
                return this.className;
            }
        }

        public bool HandleConditions
        {
            get
            {
                IDoozer doozer = (IDoozer) this.addIn.CreateObject(this.className);
                if (doozer == null)
                {
                    return false;
                }
                AddInTree.Doozers[this.name] = doozer;
                return doozer.HandleConditions;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}

