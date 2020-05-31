using Import.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntColonyOptimizationAlgorithm
{
    public class AntColonyOptimization
    {
        private int maxWeight;
        public (List<State> state, int weight)[] Run(List<State> graph, State initState, int depth)
        {
            var costsOrdered = graph.Select(x => x.Cost).OrderByDescending(x => x);
            maxWeight = costsOrdered.First();
            double perfectPathLength = costsOrdered.Take(depth).Sum();
            //Коэффициент испарения феромона
            var rho = 0.09;

            int countOfAnt = 7;
            int generationCount = 500;

            var display = new (List<State> state, int weight)[generationCount];

            
            var alpha = 1;
            var beta = 1.5;

            //Количество муравьёв, которые пройдут путь

            for (int generation = 0; generation < generationCount; generation++)
            {
                var currGenerationResults = new (List<State> state, int weight)[countOfAnt];

                //Инициализация рандомайзера
                Random rnd = new Random();
                for (int ant = 0; ant < countOfAnt; ant++)
                {
                    List<State> path = new List<State>();
                    //Запрещённые пункты, где муравей уже был
                    var marked = new List<State>();

                    var wayLength = 0;

                    //Первая итерация, добавляем в путь первую вершину
                    path.Add(initState);
                    var nextWay = GetNextState(initState, alpha, beta, path, marked, rnd);
                    path.Add(nextWay);

                    //Пока не закончатся города, в которые можно ходить
                    while (path.Count != depth)
                    {
                        //Вызываем метод для последней вершины, куда перешли
                        nextWay = GetNextState(nextWay, alpha, beta, path, marked, rnd);

                        if (nextWay == null) break;

                        path.Add(nextWay);
                        marked.Add(nextWay);
                        wayLength += nextWay.Cost;
                    }
                    currGenerationResults[ant] = (path, wayLength);
                }

                var bestPath = currGenerationResults.OrderByDescending(x => x.weight).First();
                double deltaTau = bestPath.weight / perfectPathLength;
                display[generation] = bestPath;
                //Обновляем феромоны в зависимости от результатов
                UpdatePheromone(bestPath.state, graph, deltaTau, rho);
            }

          
            return display;
        }

        private void UpdatePheromone(List<State> path, List<State> graph, double deltaTau, double rho)
        {
            var oneMinusRho = 1 - rho;
            var pathIndexes = path.Select(x => x.Index).ToList();
            foreach (var state in graph)
            {
                int toIndex = -1;
                int inIndex = -1;

                if (pathIndexes.Contains(state.Index))
                {
                    var nextIndex = path.FindIndex(x => x.Index == state.Index) + 1;

                    if (nextIndex < path.Count)
                        toIndex = nextIndex;

                    inIndex = nextIndex - 2;
                }

                foreach (var edge in state.Edges)
                {
                    edge.Tau = oneMinusRho * edge.Tau;

                    if (edge.State.Index == toIndex || edge.State.Index == inIndex)
                        edge.Tau += deltaTau;
                }
            }
        }

        private State GetNextState(State state, double alpha, double beta, List<State> path, List<State> markedStates, Random random)
        {
            var probs = new List<(State state, double prob)>();
            var markedIndexes = markedStates.Union(path).Select(x => x.Index).ToList();
            var availableEdges = state.Edges.Where(x => !markedIndexes.Contains(x.State.Index)).ToList();
            if (availableEdges.Count == 0)
                return BackTrack(path, markedStates, alpha, beta, random);

            var max = CalculateAllProbabilities(availableEdges, alpha, beta);

            foreach (var item in availableEdges)
            {
                var currProbability = Math.Pow((double)1/(maxWeight +1 - item.State.Cost), beta) * Math.Pow(item.Tau, alpha) / max;
                probs.Add((item.State, currProbability));
            }
            var rand = random.NextDouble();
            int curr = 0;
            //Нахождение интересующего интервала. Если он найден, выходим из цикла
            for (double i = 0; ; i += probs[curr].prob, curr++)
            {
                if (i < rand && rand <= (i + probs[curr].prob))
                    break;
            }

            //Возвращаем путь, по которому предлагается двигаться далее
            return probs[curr].state;

        }
        private State BackTrack(List<State> path, List<State> closed, double alpha, double beta, Random random)
        {
            State backTracked = path.Last();
            closed.Add(backTracked);

            while (closed.Contains(backTracked))
            {
                if (path.Count == 1) return null;

                path.Remove(path.Last());
                backTracked = path.Last();
            }

            return GetNextState(backTracked, alpha, beta, path, closed, random);
        }


        public double CalculateAllProbabilities(List<Edge> availableEdges, double alpha, double beta)
        {
            double commonProb = 0;
            foreach (var item in availableEdges)
                commonProb += Math.Pow((double)1 / (maxWeight + 1 - item.State.Cost), beta) * Math.Pow(item.Tau, alpha);
            return commonProb;
        }
    }
}
