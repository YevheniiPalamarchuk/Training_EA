using System;
using System.Linq;

namespace Training_EA
{
    internal class Program
    {
        static double[][] GenerateInitialPopulation(int populationSize, int dimensions, double minValue, double maxValue)
        {
            double[][] population = new double[populationSize][];

            Random random = new Random();

            for (int i = 0; i < populationSize; i++)
            {
                population[i] = new double[dimensions];

                for (int j = 0; j < dimensions; j++)
                {
                    population[i][j] = random.NextDouble() * (maxValue - minValue) + minValue;
                }
            }

            return population;
        }
        static double[] EvaluateGeneralizedRosenbrock(double[][] population)
        {
            int populationSize = population.Length;
            int dimensions = population[0].Length;

            double[] fitnessValues = new double[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                double sum = 0;

                for (int j = 0; j < dimensions - 1; j++)
                {
                    double term1 = 100 * Math.Pow((population[i][j + 1] - Math.Pow(population[i][j], 2)), 2);
                    double term2 = Math.Pow((population[i][j] - 1), 2);
                    sum += term1 + term2;
                }

                fitnessValues[i] = sum;
            }

            return fitnessValues;
        }

        static void Main(string[] args)
        {
            int dimensions = 5;
            double minValue = -30;
            double maxValue = 30;

            double[][] initialPopulation = GenerateInitialPopulation(20, dimensions, minValue, maxValue);

            double[] fitnessValues = EvaluateGeneralizedRosenbrock(initialPopulation);

            // Display the generated initial population
            Console.WriteLine("Generated Initial Population:");
            for (int i = 0; i < initialPopulation.Length; i++)
            {
                Console.Write($"Solution {i + 1}: [");
                for (int j = 0; j < dimensions; j++)
                {
                    Console.Write($"{initialPopulation[i][j],10:F4}");
                    if (j < dimensions - 1)
                        Console.Write(", ");
                }
                Console.WriteLine("]");
            }

            // Display the results for each solution
            Console.WriteLine("Fitness Values for Each Solution:");
            for (int i = 0; i < initialPopulation.Length; i++)
            {
                Console.Write($"Solution {i + 1}: [");
                for (int j = 0; j < dimensions; j++)
                {
                    Console.Write($"{initialPopulation[i][j],10:F4}");
                    if (j < dimensions - 1)
                        Console.Write(", ");
                }
                Console.WriteLine($"] Fitness: {fitnessValues[i]:F6}");
            }

            // Find the index of the solution with the minimum fitness value
            int optimalSolutionIndex = Array.IndexOf(fitnessValues, fitnessValues.Min());

            // Display the optimal solution
            Console.WriteLine("\nOptimal Solution:");
            Console.Write("Solution: [");
            for (int j = 0; j < dimensions; j++)
            {
                Console.Write($"{initialPopulation[optimalSolutionIndex][j],10:F4}");
                if (j < dimensions - 1)
                    Console.Write(", ");
            }
            Console.WriteLine($"] Fitness: {fitnessValues[optimalSolutionIndex]:F6}");
        }
    }
}
