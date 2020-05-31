using System;
using System.Linq;

namespace CourseWorkOperationResearch
{
    class Program
    {
        static void Main(string[] args)
        {
            var importedStructure = new Import.Import();
            //var state = importedStructure.GetFullGraphFormFile(Import.Import.ReadImport(
            //    @"D:\RiderProjects\CourseWorkOperationResearch\Import\import.json"));
            var state = importedStructure.GenerateGraph(30, 21, true);

            var greedySearchResult = new GreedySearch.GreedySearch().Run(state, 21);
            var c = greedySearchResult.Sum(x => x.Cost);
            var acoResult = new AntColonyOptimizationAlgorithm.AntColonyOptimization().Run(importedStructure.Graph, state, 22);
        }
    }
}
