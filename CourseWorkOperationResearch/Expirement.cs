using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseWorkOperationResearch
{
    public class Expirement
    {
        public void SolveExpirementProblem()
        {
            var import = new Import.Import();
            uint pathCount = 1;
            uint peakCount = 1;

            Console.Write("Введіть кількість вершин:");


            while (true)
            {
                if (!uint.TryParse(Console.ReadLine(), out peakCount))
                {
                    Console.Write("Введіть НАТУРАЛЬНЕ число (до 1000):");
                    continue;
                }


                if (peakCount < 3)
                {
                    Console.Write("Введіть НАТУРАЛЬНЕ число більше 2:");
                    continue;
                }


                if (peakCount <= 1000 && peakCount != 0)
                {
                    Console.Write("Введіть довжину шляху:");
                    while (true)
                    {
                        if (!uint.TryParse(Console.ReadLine(), out pathCount))
                        {
                            Console.Write("Введіть НАТУРАЛЬНЕ число менше ніж кількість вершин на 2 :");
                            continue;
                        }

                        if (pathCount < 1)
                        {
                            Console.Write("Введіть НАТУРАЛЬНЕ число більше 0:");
                            continue;
                        }

                        if (pathCount <= peakCount - 1)
                        {
                            var kek = Convert.ToInt32(pathCount);
                            var state = import.GenerateOrpGraph(Convert.ToInt32(peakCount), kek);
                            var states = import.Graph;
                            var ideal = import.Graph.OrderByDescending(y => y.Cost).Take(kek).Sum(y => y.Cost);

                            Console.WriteLine($"Ідеальний шлях: {ideal}");

                            var greedySearch = new GreedySearch.GreedySearch();
                            var aco = new AntColonyOptimizationAlgorithm.AntColonyOptimization();
                            var hills = new HillClimbing.HillClimbing();
                            var greedySearchResult = greedySearch.Run(state, kek);
                            var path = greedySearchResult.Select(x => x.Index);

                            Console.WriteLine($"Результат жадібного алгоритму:{greedySearchResult.Skip(1).Sum(g => g.Cost)}");
                            Console.Write($"Шлях жадібного алгоритму: ");
                            foreach (var p in path)
                            {
                                Console.Write($"{p} ");
                            }
                            Console.WriteLine();
                            Console.WriteLine();

                            var acoResult = aco.Run(states, state, kek);
                            Console.WriteLine($"Результат алгоритму мурашиной колонії:{acoResult.Max(y => y.weight)}");
                            var maxResult = acoResult.OrderByDescending(result => result.weight).First();
                            Console.Write($"Шлях мурашиного алгоритму: ");
                            foreach (var result in maxResult.state)
                            {
                                Console.Write($"{result} ");
                            }
                            Console.WriteLine();
                            Console.WriteLine();

                            var hillsResult = hills.FindSolution(state, kek);
                            Console.WriteLine($"Результат алгоритму Hills:{hillsResult.Max(localPath => localPath.Sum(g => g.Cost))}");
                            var kke = hillsResult.OrderByDescending(k => k.Sum(y => y.Cost)).First();

                            Console.Write($"Шлях алгоритму HILLS: ");
                            foreach (var p in kke)
                            {
                                Console.Write($"{p.Index} ");
                            }
                            Console.WriteLine();
                            Console.WriteLine();


                            Console.WriteLine($"Час виконання жадібного алгоритму: {greedySearch.AlgorithmExecutionTime.Milliseconds} мс");
                            Console.WriteLine($"Час виконання мурашиного алгоритму: {aco.AlgorithmExecutionTime.Milliseconds} мс");
                            Console.WriteLine($"Час виконання алгоритму Hills: {hills.AlgorithmExecutionTime.Milliseconds} мс");
                            Console.WriteLine();
                            Console.WriteLine("Різниця між алгоритмами та ідеальним шляхом:");
                            Console.WriteLine($"Жадібний алгоритм:{ideal - greedySearchResult.Skip(1).Sum(g => g.Cost)}");
                            Console.WriteLine($"Hills алгоритм:{ideal - hillsResult.Max(localPath => localPath.Sum(g => g.Cost))}");
                            Console.WriteLine($"Мурашиний алгоритм :{ideal - acoResult.Max(y => y.weight)}");
                            Console.ReadLine();
                            break;
                        }
                        else
                            continue;
                    }
                }
                break;
            }
        }
    }

}

