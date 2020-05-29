using System;
using System.Collections.Generic;
using System.Text;

namespace Import.Helpers
{
    class ImportStructureDto
    {
        public int PathLength { get; set; }
        public IReadOnlyCollection<int> PeaksWeight { get; set; }
        public IReadOnlyCollection<string> AdjacencyMatrix { get; set; }
    }
}
