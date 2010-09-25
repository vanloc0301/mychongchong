namespace SkyMap.Net.Workflow.XPDL
{
    using System;

    [Serializable]
    public class ProdefRow
    {
        public string Id;
        public string Name;

        public ProdefRow(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}

