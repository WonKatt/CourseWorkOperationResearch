using System;
using System.Collections.Generic;
using System.Text;

namespace Import.Helpers
{
    public class State
    {
        public State(int index, int cost)
        {
            Index = index;
            Cost = cost;
        }
        public int Index { get; }
        public int Cost { get; }
        public List<Edge> Edges { get; set; } = new List<Edge>();
    }
    public class Edge
    {
        public Edge(State state)
        {
            State = state;
        }
        public State State { get; }
    }
}
