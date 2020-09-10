using System;
using System.IO;
using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;

namespace WorkingWithWorkspaces
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sln = new AnalyzerManager(@"..\..\..\..\RoslynCompilerPlatform.sln")
                .GetWorkspace()
                .CurrentSolution;
            PrintSolution(sln);
        }

        private static void PrintSolution(Solution sln)
        {
            //Print the root of the solution
            Console.WriteLine(Path.GetFileName(sln.FilePath));

            //get dependency graph to perform a sort
            var g = sln.GetProjectDependencyGraph();
            var ps = g.GetTopologicallySortedProjects();

            //print all projects, their documents and references
            foreach (var p in ps)
            {
                var proj = sln.GetProject(p);
                Console.WriteLine("> " + proj.Name);

                Console.WriteLine("  > References");
                foreach (var r in proj.ProjectReferences)
                    Console.WriteLine("    - " + sln.GetProject(r.ProjectId).Name);

                foreach (var d in proj.Documents) Console.WriteLine("  - " + d.Name);
            }
        }
    }
}