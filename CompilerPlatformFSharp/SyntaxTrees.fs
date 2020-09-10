module SyntaxTrees
    
    open System.Collections.Generic
    open Microsoft.CodeAnalysis;
    open Microsoft.CodeAnalysis.CSharp
    open Microsoft.CodeAnalysis.CSharp.Syntax

    let UsingVisitors() =
        let tree = CSharpSyntaxTree.ParseText("class Foo { void Bar() {} }")
        let node = tree.GetRoot() :?> CompilationUnitSyntax

        let object1 = { new CSharpSyntaxWalker() with
            override this.ToString() = "This overrides object.ToString()"
            override this.VisitMethodDeclaration(node) = 
                let nodetext = node.Identifier.Text
                base.VisitMethodDeclaration(node)
        }
        object1.Visit(node);
    let UsingObjectModel () =

        let tree = CSharpSyntaxTree.ParseText("class Foo { void Bar() {} }")
        let node = tree.GetRoot() :?> CompilationUnitSyntax
        

        node.Members
        |> Seq.filter (fun x -> x.Kind() = SyntaxKind.ClassDeclaration)
        |> Seq.map (fun x -> x :?> ClassDeclarationSyntax)    
        |> Seq.map (fun x -> x.Members
                                |> Seq.filter (fun x -> x.Kind() = SyntaxKind.MethodDeclaration)
                                |> Seq.map (fun x -> 
                                                        sprintf "%s" ((x :?> MethodDeclarationSyntax).ToString())
                                                    )
                                |> Seq.fold (fun acc x ->  sprintf "%s,%s" x acc ) "")
        |> Seq.fold (fun acc x ->  sprintf "%s,%s" x acc ) ""


    let CreateSyntax() =
        let methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)), "Bar")
                                                .WithBody(SyntaxFactory.Block())
        let method = seq {  methodDeclaration } :?> IEnumerable<MemberDeclarationSyntax>
        let members = SyntaxList<MemberDeclarationSyntax>(SyntaxFactory.List(method))
        
        let res = SyntaxFactory.ClassDeclaration("Foo")
                    .WithMembers(members)
                    .NormalizeWhitespace()

        res.ToString()




