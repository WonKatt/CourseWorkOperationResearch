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
        public List<State> Graph { get; private set; }
        public State GenerateOrpGraph(int peakCount, int maxPath = 0, bool saveAsFile = true)
        {
            var c = new byte[peakCount, peakCount];
            var random = new Random();
            var peaksWeight = new int[peakCount];

            for (int i = 0; i < peakCount; i++)
            {
                peaksWeight[i] = random.Next(0, 100);
                for (int j = 0; j < i; j++)
                {
                    var isEdged = random.Next(1, 13) % 13 == 1;
                    if (!isEdged) continue;

                    c[i, j] = 1;
                    c[j, i] = 1;
                }
            }

            if (maxPath == 0) maxPath = (peakCount * 2) / 3;

            var adjencyMatrixAsStringArray = new List<string>(peakCount);

            for (var i = 0;  i < peakCount; i++)
            {
                var row = new int[peakCount];
                for (int j = 0; j < peakCount; j++)
                    row[j] = c[i, j];
                adjencyMatrixAsStringArray.Add(string.Join(',', row));
            }
            var import = new ImportStructureDto()
            {
                AdjacencyMatrix = adjencyMatrixAsStringArray,
                PathLength = maxPath,
                PeaksWeight = peaksWeight
            };

            if(saveAsFile)
            {
                var json = JsonConvert.SerializeObject(import, Formatting.Indented);
                File.WriteAllText($@"../../../../Import/Jsons/{Guid.NewGuid()}.json", json);
            }

            return GetFullGraphFormFile(import);
        }
        public State GenerateGraph(int peakCount, int maxPath = 0, bool saveAsFile = true)
        {
            var c = new int[peakCount, peakCount];
            var random = new Random();
            var peaksWeight = new int[peakCount];

            for (int i = 0; i < peakCount; i++)
            {
                for (int j = 0; j < peakCount; j++)
                {
                    if (i == j) continue;
                
                    var isEdged = random.Next(1, 13) % 13 == 1;
                    if (!isEdged) continue;

                    c[i, j] = random.Next(1,100);
                }
            }

            if (maxPath == 0) maxPath = (peakCount * 2) / 3;

            var adjencyMatrixAsStringArray = new List<string>(peakCount);

            for (var i = 0; i < peakCount; i++)
            {
                var row = new int[peakCount];
                for (int j = 0; j < peakCount; j++)
                    row[j] = c[i, j];
                adjencyMatrixAsStringArray.Add(string.Join(',', row));
            }
            var import = new ImportStructureDto()
            {
                AdjacencyMatrix = adjencyMatrixAsStringArray,
                PathLength = maxPath,
                PeaksWeight = peaksWeight
            };

            if (saveAsFile)
            {
                var json = JsonConvert.SerializeObject(import, Formatting.Indented);
                File.WriteAllText($@"../../../../Import/Jsons/{Guid.NewGuid()}.json", json);
            }

            return GetFullGraphFormFile(import);
        }
        public State GetFullGraphFormFile(ImportStructureDto import)
        {

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
                    var currEdge = new Edge(states[j]);
                    currPeak.Edges.Add(currEdge);
                }
            }

            Graph = states;
            return states.First();
        }
        public static ImportStructureDto ReadImport(string path)
        {
            using (var fileReader = new StreamReader(path))
            {
                return JsonConvert.DeserializeObject<ImportStructureDto>(fileReader.ReadToEnd());
            }
        }

    }
}
