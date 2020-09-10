namespace Tests

open System
open Xunit
open FsUnit
open FsUnit.Xunit
open SyntaxTrees


type ``Not a Number tests``() =
    [<Fact>]
    let ``My test`` () =
        Assert.True(true)

    [<Fact>]
    member __.``Number 1 should be a number``() =
        1 |> should not' (be NaN)

    [<Fact>]
    member __.``Using Object Model``() =
        SyntaxTrees.UsingVisitors()
        SyntaxTrees.UsingObjectModel() |> should equal "void Bar() {},,"
        
    [<Fact>]
    member __.``Create Syntax``() =
        SyntaxTrees.CreateSyntax() |> should equal "class Foo
{
    void Bar()
    {
    }
}"

    [<Fact>]
    member __.``Using Compilation Api``() =
        CompilationApi.testCompilation() |> should equal ""
