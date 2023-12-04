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

        // Selection: Tournament Selection
        static int TournamentSelection(double[][] population, double[] fitnessValues, int tournamentSize)
        {
            Random random = new Random();

            int populationSize = population.Length;
            int selectedParentIndex = random.Next(populationSize); // Initialize with a random individual.

            for (int i = 1; i < tournamentSize; i++)
            {
                int competitorIndex = random.Next(populationSize); // Choose another random individual.

                // Compare fitness values and select the one with better fitness.
                if (fitnessValues[competitorIndex] < fitnessValues[selectedParentIndex])
                {
                    selectedParentIndex = competitorIndex;
                }
            }

            return selectedParentIndex;
        }
        // Recombination (Arithmetic crossover)
        static double[][] ApplyCrossover(double[][] population, int dimensions, double crossoverRate)
        {
            Random random = new Random();
            int populationSize = population.Length;

            for (int i = 0; i < populationSize - 1; i += 2)
            {
                // Apply crossover with a certain probability
                if (random.NextDouble() < crossoverRate)
                {
                    double alpha = random.NextDouble();

                    // Perform arithmetic crossover
                    for (int j = 0; j < dimensions; j++)
                    {
                        double temp = alpha * population[i][j] + (1 - alpha) * population[i + 1][j];
                        population[i + 1][j] = alpha * population[i + 1][j] + (1 - alpha) * population[i][j];
                        population[i][j] = temp;
                    }
                }
            }

            return population;
        }




        // Body
        static void Main(string[] args)
        {
            int dimensions = 5;
            double minValue = -30;
            double maxValue = 30;
            int populationSize = 20;

            double[][] population = GenerateInitialPopulation(populationSize, dimensions, minValue, maxValue);
            double[] fitnessValues = EvaluateGeneralizedRosenbrock(population);


            // Display the generated initial population
            Console.WriteLine("Generated Initial Population:");
            for (int i = 0; i < population.Length; i++)
            {
                Console.Write($"Solution {i + 1}: [");
                for (int j = 0; j < dimensions; j++)
                {
                    Console.Write($"{population[i][j],10:F4}");
                    if (j < dimensions - 1)
                        Console.Write(", ");
                }
                Console.WriteLine("]");
            }

            // Display the results for each solution
            Console.WriteLine("Fitness Values for Each Solution:");
            for (int i = 0; i < population.Length; i++)
            {
                Console.Write($"Solution {i + 1}: [");
                for (int j = 0; j < dimensions; j++)
                {
                    Console.Write($"{population[i][j],10:F4}");
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
                Console.Write($"{population[optimalSolutionIndex][j],10:F4}");
                if (j < dimensions - 1)
                    Console.Write(", ");
            }
            Console.WriteLine($"] Fitness: {fitnessValues[optimalSolutionIndex]:F6}");

            int iterations = dimensions * 10000;
            int tournamentSize = 3;
            double crossoverRate = 0.7;


            for (int iteration = 0; iteration < iterations; iteration++)
            {
                // Tournament Selection
                int selectedParentIndex = TournamentSelection(population, fitnessValues, tournamentSize);

                // Apply Crossover
                population = ApplyCrossover(population, dimensions, crossoverRate);

                // Retrieve the selected solution values
                double[] selectedSolution = population[selectedParentIndex];

                // Display the selected parent index and its solution values
                Console.Write($"Iteration {iteration + 1}: Selected Parent Index = {selectedParentIndex}, Solution Values = [");
                for (int j = 0; j < dimensions; j++)
                {
                    Console.Write($"{selectedSolution[j],10:F4}");
                    if (j < dimensions - 1)
                        Console.Write(", ");
                }
                Console.WriteLine("]");
            }
        }
    }
}
