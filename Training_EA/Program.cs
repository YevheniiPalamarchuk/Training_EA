using System;
using System.Collections.Generic;
using System.Linq;

namespace Training_EA
{
    internal class Program
    {
        private static Random random = new Random();
        static double[][] GenerateInitialPopulation(int populationSize, int dimensions, double minValue, double maxValue)
        {
            double[][] population = new double[populationSize][];

            for (int i = 0; i < populationSize; i++)
            {
                population[i] = new double[dimensions];

                for (int j = 0; j < dimensions; j++)
                {
                    population[i][j] = Math.Round(random.NextDouble() * (maxValue - minValue) + minValue, 2);
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

        // Mutation 
        static void ApplyMutation(double[] individual, double mutationRate, double mutationRange)
        {

            for (int i = 0; i < individual.Length; i++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    // Apply mutation to the gene with a random value within the mutation range
                    individual[i] += (random.NextDouble() * 2 - 1) * mutationRange;
                }
            }
        }

        // Replacement
        static double[][] ReplacePopulation(double[][] population, double[][] offspring, double[] fitnessValues)
        {
            int populationSize = population.Length;

            // Combine parents and offspring
            double[][] combinedPopulation = population.Concat(offspring).ToArray();
            double[][] newPopulation = new double[populationSize][];

            // Select individuals with the best fitness values
            var sortedPopulation = combinedPopulation.Zip(fitnessValues, (p, f) => new { Solution = p, Fitness = f })
                                                    .OrderBy(item => item.Fitness)
                                                    .ToList();

            for (int i = 0; i < populationSize; i++)
            {
                newPopulation[i] = sortedPopulation[i].Solution;
            }

            return newPopulation;
        }





        // Body
        static void Main(string[] args)
        {
            // Generalized Rsenbrock values
            int dimensions = 10;
            double minValue = -30;
            double maxValue = 30;
            int populationSize = 20;

            double[][] population = GenerateInitialPopulation(populationSize, dimensions, minValue, maxValue);
            double[] fitnessValues = EvaluateGeneralizedRosenbrock(population);


            //// Display the generated initial population
            //Console.WriteLine("Generated Initial Population:");
            //for (int i = 0; i < population.Length; i++)
            //{
            //    Console.Write($"Solution {i + 1}: [");
            //    for (int j = 0; j < dimensions; j++)
            //    {
            //        Console.Write($"{population[i][j],10:F2}");
            //        if (j < dimensions - 1)
            //            Console.Write(", ");
            //    }
            //    Console.WriteLine("]");
            //}

            //// Display the results for each solution
            //Console.WriteLine("Fitness Values for Each Solution:");
            //for (int i = 0; i < population.Length; i++)
            //{
            //    Console.Write($"Solution {i + 1}: [");
            //    for (int j = 0; j < dimensions; j++)
            //    {
            //        Console.Write($"{population[i][j],10:F2}");
            //        if (j < dimensions - 1)
            //            Console.Write(", ");
            //    }
            //    Console.WriteLine($"] Fitness: {fitnessValues[i]:F6}");
            //}

            // Find the index of the solution with the minimum fitness value
            int optimalSolutionIndex = Array.IndexOf(fitnessValues, fitnessValues.Min());

            // Display the optimal solution
            Console.WriteLine("\nOptimal Solution:");
            Console.Write("Solution: [");
            for (int j = 0; j < dimensions; j++)
            {
                Console.Write($"{population[optimalSolutionIndex][j],10:F2}");
                if (j < dimensions - 1)
                    Console.Write(", ");
            }
            Console.WriteLine($"] Fitness: {fitnessValues[optimalSolutionIndex]:F6}");

            // Variables needed for EA
            int iterations = dimensions * 10000;
            int tournamentSize = 2;
            double crossoverRate = 0.5;
            double mutationRate = 0.1;
            double mutationRange = 1.0;
            int offspringSize = populationSize;


            // Variables for tracking the best solution
            double bestFitness = double.MaxValue; // Initialize with a large value for minimization
            List<double> bestFitnessList = new List<double>();

            // Main loop
            for (int iteration = 0; iteration < iterations; iteration++)
            {
                // Tournament Selection
                int selectedParentIndex = TournamentSelection(population, fitnessValues, tournamentSize);

                // Apply Crossover
                population = ApplyCrossover(population, dimensions, crossoverRate);

                // Apply Mutation
                for (int i = 0; i < population.Length; i++)
                {
                    ApplyMutation(population[i], mutationRate, mutationRange);
                }

                // Evaluate fitness of the offspring
                double[] offspringFitnessValues = EvaluateGeneralizedRosenbrock(population);

                // Create offspring based on the crossover and mutation
                double[][] offspring = ApplyCrossover(population, dimensions, crossoverRate);
                for (int i = 0; i < offspring.Length; i++)
                {
                    ApplyMutation(offspring[i], mutationRate, mutationRange);
                }

                // Replace the population
                population = ReplacePopulation(population, offspring, offspringFitnessValues);

                // Retrieve the selected solution values
                double[] selectedSolution = population[selectedParentIndex];
                double[] fitnessValuesLoop = EvaluateGeneralizedRosenbrock(population);
                double selectedFitness = fitnessValuesLoop[selectedParentIndex];

                // Update the best solution if a better one is found
                if (selectedFitness < bestFitness)
                {
                    bestFitness = selectedFitness;
                    // Add the best fitness to the list
                    bestFitnessList.Add(bestFitness);
                }


                // Display the selected parent index and its solution values
                Console.Write($"Iteration {iteration + 1}: Selected Parent Index = {selectedParentIndex}, Solution Values = [");
                for (int j = 0; j < dimensions; j++)
                {
                    Console.Write($"{selectedSolution[j],10:F2}");
                    if (j < dimensions - 1)
                        Console.Write(", ");
                }
                Console.WriteLine($"] Fitness: {selectedFitness:F6}, Best Fitness: {bestFitness:F6}");
            }
            // Display the list of best fitness values
            Console.WriteLine("\nBest Fitness Values:");
            foreach (var fitness in bestFitnessList)
            {
                Console.WriteLine($"Fitness: {fitness:F6}");
            }
        }
    }
}
