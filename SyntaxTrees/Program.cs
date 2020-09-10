using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxTrees
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var tree = CSharpSyntaxTree.ParseText("class Foo { void Bar() {} }");
            var node = (CompilationUnitSyntax) tree.GetRoot();

            //Using object model

            foreach (var member in node.Members)
                if (member.Kind() == SyntaxKind.ClassDeclaration)
                {
                    var @class = (ClassDeclarationSyntax) member;

                    foreach (var member2 in @class.Members)
                        if (member2.Kind() == SyntaxKind.MethodDeclaration)
                        {
                            var method = (MethodDeclarationSyntax) member2;
                            //do stuff
                            Console.WriteLine("1:-" + member2);
                        }
                }

            //Using LINQ query methods
            var bars = from member in node.Members.OfType<ClassDeclarationSyntax>()
                from member2 in member.Members.OfType<MethodDeclarationSyntax>()
                where member2.Identifier.Text == "Bar"
                select member2;

            Console.WriteLine("2:-" + bars.First());

            //Using Visitors
            new MyVisitor().Visit(node);


            //Create Syntax
            var res = SyntaxFactory.ClassDeclaration("Foo")
                .WithMembers(new SyntaxList<MemberDeclarationSyntax>(SyntaxFactory.List(new[]
                {
                    SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(
                            SyntaxFactory.Token(SyntaxKind.VoidKeyword)
                        ), "Bar")
                        .WithBody(SyntaxFactory.Block())
                }))).NormalizeWhitespace();

            Console.WriteLine(res);

            Console.ReadKey();
        }

        private class MyVisitor : CSharpSyntaxWalker
        {
            public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                Console.WriteLine("3-" + node.Identifier.Text);

                base.VisitMethodDeclaration(node);
            }
        }
    }
}