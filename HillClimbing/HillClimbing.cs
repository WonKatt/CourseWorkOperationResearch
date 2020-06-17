using Import.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HillClimbing
{
    public class HillClimbing
    {
        private Random random = new Random();
        public TimeSpan AlgorithmExecutionTime { get; set; }

        public List<List<State>> FindSolution(State startCity, int pathLength)
        {
            Stopwatch stopwatch = new Stopwatch();
            var path = new List<List<State>>();
            stopwatch.Start();
            for (int i = 0; i < 100; i++)
            {
                var localPath = new List<State>() { startCity };
                var closedCities = new List<State>();
                var currentCity = startCity;


                while (localPath.Count <= pathLength)
                {
                    if (currentCity == null)
                    {
                        currentCity = startCity;
                        closedCities.Clear();
                        localPath.Clear();
                        localPath.Add(currentCity);
                    }

                    currentCity = GetNextCity(currentCity, closedCities, localPath);

                    localPath.Add(currentCity);
                }

                path.Add(localPath);
            }

            stopwatch.Stop();
            AlgorithmExecutionTime = stopwatch.Elapsed;
            return path;
        }

        private State GetNextCity(State currentCity, List<State> closedCities, List<State> path)
        {
            closedCities.Add(currentCity);
            var closedCityIndexes = closedCities.Select(cities => cities.Index);

            var adjacentCities = currentCity.Edges.
                Where(city => !closedCityIndexes.Contains(city.State.Index)).ToList();

            if (!adjacentCities.Any())
                return BackTrack(path, closedCities);

            var AdjacentCityWithMostValue = adjacentCities.Max(city => city.State.Cost);

            if (currentCity.Cost <= AdjacentCityWithMostValue)
            {
                return adjacentCities
                    .First(city => city.State.Cost == AdjacentCityWithMostValue).State;
            }
            else
            {
                var rand = random.Next(0, 1);
                if (rand == 0)
                {
                    return adjacentCities
                        .ElementAt(random.Next(0, adjacentCities.Count())).State;
                }
                else
                {
                    return adjacentCities.First(x => x.State.Cost == AdjacentCityWithMostValue).State;
                }
            }


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
            return GetNextCity(backTracked, closed, path);
        }

    }
}

