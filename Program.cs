using System;
using System.Collections.Generic;

namespace PolivalkaTask
{
    class Program
    {
        private static void Main(string[] args)
        {
            // сбор входных данных
            Console.Write("x y a: ");
            var polivalkaData = Console.ReadLine();
            
            var flowersData = new List<string>();
            
            while (true)
            {
                Console.Write("name x y: ");
                var flower = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(flower))
                    break;

                flowersData.Add(flower);
            }
            
            // расчет
            var calculator = new Calculator(polivalkaData, flowersData);
            var result = calculator.Calc();

            // вывод
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
