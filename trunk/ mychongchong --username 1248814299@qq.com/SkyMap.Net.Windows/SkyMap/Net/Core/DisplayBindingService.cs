namespace SkyMap.Net.Core
{
    using SkyMap.Net.Gui;
    using System;

    public static class DisplayBindingService
    {
        private static DisplayBindingDescriptor[] bindings = null;
        private static readonly string displayBindingPath = "/Workbench/DisplayBindings";

        static DisplayBindingService()
        {
            bindings = (DisplayBindingDescriptor[]) AddInTree.GetTreeNode(displayBindingPath).BuildChildItems(null).ToArray(typeof(DisplayBindingDescriptor));
        }

        public static void AttachSubWindows(IViewContent viewContent, bool isReattaching)
        {
            foreach (DisplayBindingDescriptor descriptor in bindings)
            {
                if (descriptor.IsSecondary && descriptor.CanAttachToFile(viewContent.FileName ?? viewContent.UntitledName))
                {
                    ISecondaryDisplayBinding secondaryBinding = descriptor.SecondaryBinding;
                    if (((secondaryBinding != null) && (!isReattaching || secondaryBinding.ReattachWhenParserServiceIsReady)) && secondaryBinding.CanAttachTo(viewContent))
                    {
                        ISecondaryViewContent[] collection = descriptor.SecondaryBinding.CreateSecondaryViewContent(viewContent);
                        if (collection != null)
                        {
                            viewContent.SecondaryViewContents.AddRange(collection);
                        }
                        else
                        {
                            MessageService.ShowError(string.Concat(new object[] { "Can't attach secondary view content. ", descriptor.SecondaryBinding, " returned null for ", viewContent, ".\n(should never happen)" }));
                        }
                    }
                }
            }
        }

        public static IDisplayBinding GetBindingPerFileName(string filename)
        {
            DisplayBindingDescriptor codonPerFileName = GetCodonPerFileName(filename);
            return ((codonPerFileName == null) ? null : codonPerFileName.Binding);
        }

        public static IDisplayBinding GetBindingPerLanguageName(string languagename)
        {
            DisplayBindingDescriptor codonPerLanguageName = GetCodonPerLanguageName(languagename);
            return ((codonPerLanguageName == null) ? null : codonPerLanguageName.Binding);
        }

        private static DisplayBindingDescriptor GetCodonPerFileName(string filename)
        {
            foreach (DisplayBindingDescriptor descriptor in bindings)
            {
                if ((!descriptor.IsSecondary && descriptor.CanAttachToFile(filename)) && ((descriptor.Binding != null) && descriptor.Binding.CanCreateContentForFile(filename)))
                {
                    return descriptor;
                }
            }
            return null;
        }

        private static DisplayBindingDescriptor GetCodonPerLanguageName(string languagename)
        {
            foreach (DisplayBindingDescriptor descriptor in bindings)
            {
                if ((!descriptor.IsSecondary && descriptor.CanAttachToLanguage(languagename)) && ((descriptor.Binding != null) && descriptor.Binding.CanCreateContentForLanguage(languagename)))
                {
                    return descriptor;
                }
            }
            return null;
        }
    }
}

