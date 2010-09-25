namespace SkyMap.Net.Core
{
    using SkyMap.Net.Gui;
    using System;

    public static class ViewContentService
    {
        public static IViewContent GetView(Type type)
        {
            foreach (IViewContent content in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (content.GetType().Equals(type))
                {
                    return content;
                }
            }
            return null;
        }
    }
}

