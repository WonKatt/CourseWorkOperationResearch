using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Import.Helpers;
using Newtonsoft.Json;

namespace Import
{
    public class Import
    {
        public State GetFullGraphFormFile(string filePath)
        {

            var import = ReadImport(filePath);
            var firstLineElCount = import.PeaksWeight.Count;

            var states = new List<State>(firstLineElCount);

            for (var i = 0; i < firstLineElCount; i++)
                states.Add(new State(i, import.PeaksWeight.ElementAt(i)));

            for (var i = 0; i < import.AdjacencyMatrix.Count; i++)
            {
                var currLine = import.AdjacencyMatrix.ElementAt(i).Split(',').Select(int.Parse).ToList();
                var currPeak = states.ElementAt(i);

                currPeak.Edges.Capacity = firstLineElCount;

                for (var j = 0; j < currLine.Count(); j++)
                {
                    if (j == i || currLine[j] == default) continue;
                    var currEdge = new Edge(states.ElementAt(j));
                    currPeak.Edges.Add(currEdge);
                }
            }

            return states.First();
        }
        private static ImportStructureDto ReadImport(string path)
        {
            using (var fileReader = new StreamReader(path))
            {
                return JsonConvert.DeserializeObject<ImportStructureDto>(fileReader.ReadToEnd());
            }
        }

    }
}
