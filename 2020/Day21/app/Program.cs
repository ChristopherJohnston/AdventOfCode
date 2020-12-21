using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> allIngredients = new List<string>();            
            Dictionary<string, HashSet<string>> allergenIngredients = new Dictionary<string, HashSet<string>>();

            foreach (string food in ParseInput()) {
                Match m = Regex.Match(food, @"(.*) \(contains (.*)\)");
                var ingredients = m.Groups[1].Value.Split(' ');
                var allergens = m.Groups[2].Value.Split(", ");

                foreach (string ingredient in ingredients) {
                    allIngredients.Add(ingredient);
                }

                foreach (string allergen in allergens) {
                    if (!allergenIngredients.ContainsKey(allergen))
                        allergenIngredients[allergen] = ingredients.ToHashSet();
                    else {
                        // Remove anything from the allergen's existing ingredients
                        // that aren't in the current food's ingredients
                        foreach (string ingredient in allergenIngredients[allergen]) {
                            if (!ingredients.Contains(ingredient))
                                allergenIngredients[allergen].RemoveWhere((i) => i == ingredient);
                        }
                    }
                }
            }

            List<string> ingredientsContainingAllergens = new List<string>();
            foreach (KeyValuePair<string, HashSet<string>> kv in allergenIngredients) {
                ingredientsContainingAllergens.AddRange(kv.Value.ToList());
            }

            // Count ingredients that aren't in any allergens
            Console.WriteLine(allIngredients.Count(c => !ingredientsContainingAllergens.Contains(c)));            
        }

        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }
}
