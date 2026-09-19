using System;
using System.Collections.Generic;
using System.Text;

namespace RandomLibrary
{
    public enum NumericVariationOptions
    {
        WithRepetition,
        UniqueInOneVariation,
        UniqueInAllVariations
    }
    public static class RandomGenerator
    {
        private static readonly Random random= new();
        public static int[,] GenerateNumericVariations(int min, int max,
            int variationLength, int variationsAmount, NumericVariationOptions option)
        {
            ValidateParameters(min, max, variationLength, variationsAmount, option);
            int[,] generatedVariations;
            if(option == NumericVariationOptions.WithRepetition)
            {
                generatedVariations = new int[variationsAmount, variationLength];
                for (int i = 0; i < variationsAmount; i++)
                {
                    int[] variation = RepeatingVariation(min, max, variationLength);
                    for (int j = 0; j < variationLength; j++)
                    {
                        generatedVariations[i, j] = variation[j];
                    }
                }
            }
            else if(option == NumericVariationOptions.UniqueInOneVariation)
            {
                generatedVariations = new int[variationsAmount, variationLength];
                for (int i = 0; i < variationsAmount; i++)
                {
                    int[] variation = UniqueVariation(min, max, variationLength);
                    for (int j = 0; j < variationLength; j++)
                    {
                        generatedVariations[i, j] = variation[j];
                    }
                }
            }
            else generatedVariations = UniqueVariations(min, max, variationLength, variationsAmount);
            return generatedVariations;
        }
        private static void ValidateParameters(int min, int max, int variationLength, int variationsAmount, NumericVariationOptions option)
        {
            if (min > max)
            {
                throw new ArgumentException("Min value cannot be greater than max value.");
            }
            if (variationLength <= 0)
            {
                throw new ArgumentException("Variation length must be greater than zero.");
            }
            if (variationsAmount <= 0)
            {
                throw new ArgumentException("Variations amount must be greater than zero.");
            }
            if (option == NumericVariationOptions.UniqueInAllVariations && variationsAmount * variationLength > (max - min + 1))
            {
                throw new ArgumentException("Not enough unique numbers available for the requested variations.");
            }
            if(option == NumericVariationOptions.UniqueInOneVariation && variationLength > (max - min + 1))
            {
                throw new ArgumentException("Not enough unique numbers available for the requested variation length.");
            }
        }
        private static int[] RepeatingVariation(int min, int max, int length)
        {
            int[] variation = new int[length];
            for (int i = 0; i < length; i++)
            {
                variation[i] = random.Next(min, max + 1);
            }
            return variation;
        }
        private static int[] UniqueVariation(int min, int max, int length)
        {
            List<int> numbers = [];
            for (int i = min; i <= max; i++)
            {
                numbers.Add(i);
            }
            int[] variation = new int[length];
            for(int i = 0; i < length; i++)
            {
                int index = random.Next(0, numbers.Count);
                int number = numbers[index];
                variation[i] = number;
                numbers.RemoveAt(index);
            }
            return variation;
        }
        private static int[,] UniqueVariations(int min, int max, int variationLength, int variationsAmount)
        {
            List<int> numbers = [];
            for (int i = min; i <= max; i++)
            {
                numbers.Add(i);
            }
            int[,] variations = new int[variationsAmount, variationLength];
            for (int i = 0; i < variationsAmount; i++)
            {
                int[] variation = new int[variationLength];
                for (int j = 0; j < variationLength; j++)
                {
                    int index = random.Next(0, numbers.Count);
                    int number = numbers[index];
                    variation[j] = number;
                    numbers.RemoveAt(index);
                }
                for (int k = 0; k < variationLength; k++)
                {
                    variations[i, k] = variation[k];
                }
            }
            return variations;
        }
    }
}
