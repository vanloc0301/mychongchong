namespace SkyMap.Net.Core
{
    using System;

    public class ResourceNotFoundException : CoreException
    {
        public ResourceNotFoundException(string resource) : base("Resource not found : " + resource)
        {
        }
    }
}

