using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CompilationApi
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var tree = CSharpSyntaxTree.ParseText("class Foo { void Bar(int x) {} }");

            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);

            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            var comp = CSharpCompilation.Create("Demo").AddSyntaxTrees(tree).AddReferences(mscorlib)
                .WithOptions(options);

            var res = comp.Emit("Demo.dll");

            if (!res.Success)
                foreach (var diagnostic in res.Diagnostics)
                    Console.WriteLine(diagnostic.GetMessage());
        }
    }
}