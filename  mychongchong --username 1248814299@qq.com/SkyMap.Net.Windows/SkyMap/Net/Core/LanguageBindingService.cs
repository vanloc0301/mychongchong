namespace SkyMap.Net.Core
{
    using SkyMap.Net.Project;
    using System;
    using System.IO;

    public class LanguageBindingService
    {
        private static LanguageBindingDescriptor[] bindings = null;

        static LanguageBindingService()
        {
            try
            {
                bindings = (LanguageBindingDescriptor[]) AddInTree.GetTreeNode("/Workbench/LanguageBindings").BuildChildItems(null).ToArray(typeof(LanguageBindingDescriptor));
            }
            catch (TreePathNotFoundException)
            {
                bindings = new LanguageBindingDescriptor[0];
            }
        }

        public static ILanguageBinding GetBindingPerFileName(string filename)
        {
            LanguageBindingDescriptor codonPerFileName = GetCodonPerFileName(filename);
            return ((codonPerFileName == null) ? null : codonPerFileName.Binding);
        }

        public static ILanguageBinding GetBindingPerLanguageName(string languagename)
        {
            LanguageBindingDescriptor codonPerLanguageName = GetCodonPerLanguageName(languagename);
            return ((codonPerLanguageName == null) ? null : codonPerLanguageName.Binding);
        }

        public static ILanguageBinding GetBindingPerProjectFile(string filename)
        {
            LanguageBindingDescriptor codonPerProjectFile = GetCodonPerProjectFile(filename);
            return ((codonPerProjectFile == null) ? null : codonPerProjectFile.Binding);
        }

        public static LanguageBindingDescriptor GetCodonPerFileName(string filename)
        {
            foreach (LanguageBindingDescriptor descriptor in bindings)
            {
                if (descriptor.Binding.CanCompile(filename))
                {
                    return descriptor;
                }
            }
            return null;
        }

        public static LanguageBindingDescriptor GetCodonPerLanguageName(string languagename)
        {
            foreach (LanguageBindingDescriptor descriptor in bindings)
            {
                if (descriptor.Binding.Language == languagename)
                {
                    return descriptor;
                }
            }
            return null;
        }

        public static LanguageBindingDescriptor GetCodonPerProjectFile(string fileName)
        {
            string str = Path.GetExtension(fileName).ToUpperInvariant();
            foreach (LanguageBindingDescriptor descriptor in bindings)
            {
                if (descriptor.ProjectFileExtension.ToUpperInvariant() == str)
                {
                    return descriptor;
                }
            }
            return null;
        }

        public static string GetProjectFileExtension(string languageName)
        {
            LanguageBindingDescriptor codonPerLanguageName = GetCodonPerLanguageName(languageName);
            return ((codonPerLanguageName == null) ? null : codonPerLanguageName.ProjectFileExtension);
        }
    }
}

