namespace SkyMap.Net.DataForms
{
    using System;
    using System.Collections.Generic;

    public class CategoryIdentities
    {
        public List<Identify> Identifies = new List<Identify>();
        public string Name;

        public CategoryIdentities(string name)
        {
            this.Name = name;
        }

        public void Add(string name, string picPath)
        {
            Identify item = new Identify {
                Name = name,
                PicPath = picPath
            };
            this.Identifies.Add(item);
        }

        public void Add(string name, string picPath, string id)
        {
            Identify item = new Identify {
                Name = name,
                PicPath = picPath,
                Id = id
            };
            this.Identifies.Add(item);
        }

        public class Identify
        {
            public string Id;
            public string Name;
            public string PicPath;
        }
    }
}

