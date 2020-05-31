using Import.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GreedySearch
{
    public class GreedySearch
    {
        public List<State> Run(State initState, int depth)
        {
            var path = new List<State>() { initState};
            var closed = new List<State>();

            var currState = initState;

            while (path.Count <= depth)
            {
                currState = GetNextState(currState,closed,path);

                if (currState == null) return null;
                path.Add(currState);
            }

            return path;
        }

        private State GetNextState(State currState, List<State> closed, List<State> path)
        {
            var closedIndexes = closed.Union(path).Select(x => x.Index);

            var availableEdges = currState.Edges
                .Where(x => !closedIndexes.Contains(x.State.Index)).ToList();

            if (availableEdges.Count == 0)
                return BackTrack(path, closed);

            var max = availableEdges.Max(x => x.State.Cost);
            return availableEdges.First(x => x.State.Cost == max).State;
        }
        private State BackTrack(List<State> path, List<State> closed)
        {
            State backTracked = path.Last();
            closed.Add(backTracked);

            while (closed.Contains(backTracked))
            {
                if (path.Count == 1) return null;

                path.Remove(path.Last());
                backTracked = path.Last();
            }

            return GetNextState(backTracked, closed, path);
        }
    }
}
