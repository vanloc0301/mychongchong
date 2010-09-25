namespace SkyMap.Net.Project
{
    using System;
    using System.CodeDom.Compiler;

    public interface ILanguageBinding
    {
        bool CanCompile(string fileName);
        CompilerResults CompileFile(string fileName);
        void Execute(string fileName, bool debug);
        string GetCompiledOutputName(string fileName);

        string Language { get; }
    }
}

