using Import.Helpers;
using System;
using System.Linq;

namespace CourseWorkOperationResearch
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.Unicode;
            IndividualProblem individualProblem = new IndividualProblem();
            Expirement exp = new Expirement();

            while (true)
            {
                Console.Clear();
                //Выводим меню, его пункты с соответствующими цифрами\символами
                Console.WriteLine("### MENU ###");
                Console.WriteLine("1. Індивідуальна задача");
                Console.WriteLine("2. Експериментальне дослідження");
                Console.WriteLine("3. Вихід");
                Console.Write("\n" + "Введите команду: ");

                uint.TryParse(Console.ReadLine(), out uint ch);
                if (ch == 3)
                    break;
                switch (ch)
                {
                    case 1:

                        IndividualProblemMenu();
                        break;
                    case 2:
                        exp.SolveExpirementProblem();
                        break;
                    case 3:
                        break;
                    default:
                        Console.WriteLine("Введіть цифру від 1 до 8");
                        Console.ReadLine();
                        break;

                }
            }
        }


        private static void IndividualProblemMenu()
        {
            IndividualProblem individualProblem = new IndividualProblem();
            while (true)
            {
                Console.Clear();
                //Выводим меню, его пункты с соответствующими цифрами\символами
                Console.WriteLine("### MENU ###");
                Console.WriteLine("1. Встановити цінності міст");
                Console.WriteLine("2. Встановити випадкові значення задачі");
                Console.WriteLine("3. Змінити значення міста");
                Console.WriteLine("4. Змінити значення шляху");
                Console.WriteLine("5. Зберегти у individual.json");
                Console.WriteLine("6. Зчитати з invdidual.json");
                Console.WriteLine("7. Вирішити індивідуальну задачу");
                Console.WriteLine("8. Вихід");
                Console.Write("\n" + "Введіть команду: ");

                uint.TryParse(Console.ReadLine(), out uint ch); //Тут желательно сделать проверку, или считывать всю строку, и в switch уже отсеивать
                if (ch == 8)
                    break;
                
                switch (ch)
                {
                    case 1:

                        individualProblem.SetCitiesPrices();
                        break;
                    case 2:
                        individualProblem.RandomIndividualProblem();
                        break;
                    case 3:
                        individualProblem.EditCityPrices();
                        break;
                    case 4:
                        individualProblem.SetPathLength();
                        break;
                    case 5:
                        individualProblem.SaveToJson();
                        break;
                    case 6:
                        individualProblem.ReadFromJson();
                        break;
                    case 7:
                        individualProblem.SolveIndividualProblem();
                        break;
                    default:
                        Console.WriteLine("Введіть цифру від 1 до 8");
                        Console.ReadLine();
                        break;

                }
            }
        }
    }
}
