using Import.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CourseWorkOperationResearch
{
    public class IndividualProblem
    {
        private ImportStructureDto IndividualProblemData = new ImportStructureDto()
        {
            AdjacencyMatrix = new List<string> {
                    "0,1,1,1,0,0,0",
                    "1,0,1,0,0,0,1",
                    "1,1,0,1,1,1,1",
                    "1,0,1,0,1,1,1",
                    "0,0,1,1,0,1,0",
                    "0,0,1,1,1,0,1",
                    "0,1,1,1,0,1,0"},
            PeaksWeight = new List<int>() { 0, 10, 25, 32, 15, 19, 30 },
            PathLength = 4
        };


        public void SetPathLength()
        {
            Console.Write("Введіть нове значення шляху:");
            while (true)
            {
                if (!uint.TryParse(Console.ReadLine(), out uint inp))
                {
                    Console.WriteLine("Будь-ласка, спробуйте ще раз (введіть число від 1 до 5):");
                }

                if (inp <= 6 && inp > 0)
                {
                    Console.WriteLine($"Довжина шляху змінена на {inp}");
                    IndividualProblemData.PathLength = Convert.ToInt32(inp);
                    break;
                }

            }
        }

        public void SetCitiesPrices()
        {
            int count = 0;
            var peaksWeight = new List<int>();

            foreach (var city in IndividualProblemData.PeaksWeight)
            {
                if (count == 0)
                {
                    peaksWeight.Add(0);
                    count++;
                }
                else
                {
                    Console.WriteLine($"Введіть цінність міста №{count}: ");
                    while (true)
                    {
                        if (!uint.TryParse(Console.ReadLine(), out uint input))
                            Console.WriteLine("Будь-ласка, введіть натуральне число");
                        if (input != 0)
                        {
                            peaksWeight.Add(Convert.ToInt32(input));
                            Console.WriteLine($"Цінність міста №{count} додана");
                            count++;
                            break;
                        }
                    }

                }

            }


            IndividualProblemData.PeaksWeight = peaksWeight;


            Console.WriteLine("Нові данні для індивідуальної задачі додані");
            Console.ReadLine();
        }

        public void RandomIndividualProblem()
        {
            bool kekes = false;
            Random random = new Random();
            Console.WriteLine("Введіть верхню межу генерування випадкових чисел для цінності міст:");
            uint range;
            var peaksWeight = new List<int>();

            while (true)
            {
                if (!uint.TryParse(Console.ReadLine(), out range));
                    Console.WriteLine("Будь-ласка, введіть натуральне число:");
                if (range != 0)
                    break;
            }

            foreach (var city in IndividualProblemData.PeaksWeight)
            {
                if (!kekes)
                {
                    kekes = true;
                    peaksWeight.Add(0);
                }
                else
                {
                    peaksWeight.Add(Convert.ToInt32(random.Next(1, Convert.ToInt32(range))));
                }
            }



            IndividualProblemData.PathLength = random.Next(1, peaksWeight.Count);
            IndividualProblemData.PeaksWeight = peaksWeight;


            Console.WriteLine("Цінність міст індивідуальної задачі випадково сгенерована");
            Console.ReadLine();
        }

        public void EditCityPrices()
        {
            uint newCityValue;

            var peaksWeight = new List<int>();
            foreach (var city in IndividualProblemData.PeaksWeight)
            {
                peaksWeight.Add(city);
            }

            Console.WriteLine("Введіть номер міста, в якому ви хочете поміняти значення(від 1 до 6):");
            while (true)
            {

                if (!uint.TryParse(Console.ReadLine(), out uint cityNumber))
                    Console.WriteLine("Введіть натуральне число від 1 до 6:");


                if (cityNumber <= 6 && cityNumber >= 1)
                {
                    Console.WriteLine($"Введіть нову цінність міста №{cityNumber}:");
                    while (true)
                    {
                        while (!uint.TryParse(Console.ReadLine(), out newCityValue))
                        {
                            Console.WriteLine("Будь-ласка, введіть натуральне число:");
                        }

                        peaksWeight[Convert.ToInt32(cityNumber)] = Convert.ToInt32(newCityValue);
                        Console.WriteLine($"Значення міста {cityNumber} було замінено на {newCityValue}");

                        IndividualProblemData.PeaksWeight = peaksWeight;

                        Console.ReadLine();
                        return;
                    }
                }
            }
        }


        public void SaveToJson()
        {

            var json = JsonConvert.SerializeObject(IndividualProblemData, Formatting.Indented);
            File.WriteAllText($@"../../../../Import/individual.json", json);
            Console.WriteLine("Індивідуальну задачу було збережено до json файлу: individual.json");
            Console.ReadLine();
        }

        public ImportStructureDto ReadFromJson()
        {
            IndividualProblemData =
                Import.Import.ReadImport($@"../../../../Import/individual.json");
            Console.WriteLine("Індивідуальна задача була зчитана з individual.json");
            Console.ReadLine();
            return IndividualProblemData;
        }

        public void SolveIndividualProblem()
        {
            var import = new Import.Import();
            var state = import.GetFullGraphFormFile(IndividualProblemData);
            var states = import.Graph;

            Console.WriteLine($"Ідеальний шлях: {import.Graph.OrderByDescending(y => y.Cost).Take(IndividualProblemData.PathLength).Sum(y => y.Cost)}");

            var greedySearchResult = new GreedySearch.GreedySearch().Run(state, IndividualProblemData.PathLength);
            var path = greedySearchResult.Select(x => x.Index);

            Console.WriteLine($"Результат жадібного алгоритму:{greedySearchResult.Skip(1).Sum(g => g.Cost)}");
            Console.Write($"Шлях жадібного алгоритму: ");
            foreach (var p in path)
            {
                Console.Write($"{p} ");
            }
            Console.WriteLine();

            var acoResult = new AntColonyOptimizationAlgorithm.AntColonyOptimization().Run(states, state, IndividualProblemData.PathLength);
            Console.WriteLine($"Результат алгоритму мурашиной колонії:{acoResult.Max(y => y.weight)}");
            var maxResult = acoResult.OrderByDescending(result => result.weight).First();
            Console.Write($"Шлях мурашиного алгоритму: ");
            foreach (var result in maxResult.state)
            {
                Console.Write($"{result} ");
            }
            Console.WriteLine();

            var hillsResult = new HillClimbing.HillClimbing().FindSolution(state, IndividualProblemData.PathLength);
            Console.WriteLine($"Результат алгоритму Hills:{hillsResult.Max(localPath => localPath.Sum(g => g.Cost))}");
            var kke = hillsResult.OrderByDescending(k => k.Sum(y => y.Cost)).First();

            Console.Write($"Шлях алгоритму HILLS: ");
            foreach (var p in kke)
            {
                Console.Write($"{p.Index} ");
            }
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
