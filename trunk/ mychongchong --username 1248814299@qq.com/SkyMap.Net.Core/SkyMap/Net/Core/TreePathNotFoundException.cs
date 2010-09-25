namespace SkyMap.Net.Core
{
    using System;

    public class TreePathNotFoundException : CoreException
    {
        public TreePathNotFoundException(string path) : base("Treepath not found: " + path)
        {
        }
    }
}

