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
        
        //improve debug
        public override string ToString()
        {
            return Index.ToString();
        }
    }
    public class Edge
    {
        public Edge(State state)
        {
            State = state;
        }
        public State State { get; }
        public double Tau { get; set; } = 0.09;
        //improve debug
        public override string ToString()
        {
            return State.Index.ToString();
        }
    }
}
