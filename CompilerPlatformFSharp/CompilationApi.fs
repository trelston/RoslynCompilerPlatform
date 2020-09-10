module CompilationApi

    open System
    open Microsoft.CodeAnalysis
    open Microsoft.CodeAnalysis.CSharp

    let testCompilation () =
        
        let tree = CSharpSyntaxTree.ParseText("class Foo { void Bar(int x) {} }")
        let mscorlib = MetadataReference.CreateFromFile(Object().GetType().Assembly.Location)
        
        let options = CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        let comp = CSharpCompilation.Create("Demo")
                       .AddSyntaxTrees(tree)
                       .AddReferences(mscorlib)
                       .WithOptions(options)
                       
        let res = comp.Emit("Demo.dll")
        
        if not res.Success then
            let arr:string[] = res.Diagnostics
                                |> Seq.map(fun x -> x.GetMessage())
                                |> Seq.toArray
            String.Join (',', arr)
        else
            ""