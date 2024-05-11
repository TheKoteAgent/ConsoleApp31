using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Recipe
{
    public string Name { get; set; }
    public string Cuisine { get; set; }
    public List<string> Ingredients { get; set; }
    public int CookingTime { get; set; }
    public string Instructions { get; set; }
}

class RecipeManager
{
    private List<Recipe> recipes;

    public RecipeManager()
    {
        recipes = new List<Recipe>();
    }

    public void AddRecipe(Recipe recipe)
    {
        recipes.Add(recipe);
    }

    public void RemoveRecipe(string name)
    {
        recipes.RemoveAll(r => r.Name == name);
    }

    public void UpdateRecipe(string name, Recipe updatedRecipe)
    {
        var index = recipes.FindIndex(r => r.Name == name);
        if (index != -1)
        {
            recipes[index] = updatedRecipe;
        }
        else
        {
            Console.WriteLine("Recipe not found!");
        }
    }

    public List<Recipe> SearchRecipesByCuisine(string cuisine)
    {
        return recipes.Where(r => r.Cuisine == cuisine).ToList();
    }

    public List<Recipe> SearchRecipesByIngredient(string ingredient)
    {
        return recipes.Where(r => r.Ingredients.Contains(ingredient)).ToList();
    }

    public List<Recipe> SearchRecipesByCookingTime(int maxCookingTime)
    {
        return recipes.Where(r => r.CookingTime <= maxCookingTime).ToList();
    }

    public void SaveRecipesToFile(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var recipe in recipes)
            {
                writer.WriteLine($"{recipe.Name},{recipe.Cuisine},{string.Join("|", recipe.Ingredients)},{recipe.CookingTime},{recipe.Instructions}");
            }
        }
    }

    public void LoadRecipesFromFile(string filePath)
    {
        recipes.Clear();
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                Recipe recipe = new Recipe
                {
                    Name = parts[0],
                    Cuisine = parts[1],
                    Ingredients = parts[2].Split('|').ToList(),
                    CookingTime = int.Parse(parts[3]),
                    Instructions = parts[4]
                };
                recipes.Add(recipe);
            }
        }
    }
    public void GenerateReportByCuisine(string cuisine, string filePath = null)
    {
        List<Recipe> filteredRecipes = recipes.Where(r => r.Cuisine == cuisine).ToList();
        GenerateReport(filteredRecipes, filePath);
    }

    public void GenerateReportByCookingTime(int maxCookingTime, string filePath = null)
    {
        List<Recipe> filteredRecipes = recipes.Where(r => r.CookingTime <= maxCookingTime).ToList();
        GenerateReport(filteredRecipes, filePath);
    }

    public void GenerateReportByIngredient(string ingredient, string filePath = null)
    {
        List<Recipe> filteredRecipes = recipes.Where(r => r.Ingredients.Contains(ingredient)).ToList();
        GenerateReport(filteredRecipes, filePath);
    }

    public void GenerateReportByName(string name, string filePath = null)
    {
        List<Recipe> filteredRecipes = recipes.Where(r => r.Name == name).ToList();
        GenerateReport(filteredRecipes, filePath);
    }

    private void GenerateReport(List<Recipe> recipes, string filePath)
    {
        if (recipes.Count == 0)
        {
            Console.WriteLine("No recipes found for the given criteria.");
            return;
        }

        if (filePath != null)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var recipe in recipes)
                {
                    writer.WriteLine($"Recipe: {recipe.Name}, Cuisine: {recipe.Cuisine}, Cooking Time: {recipe.CookingTime} mins");
                    writer.WriteLine("Ingredients:");
                    foreach (var ingredient in recipe.Ingredients)
                    {
                        writer.WriteLine($"- {ingredient}");
                    }
                    writer.WriteLine("Instructions:");
                    writer.WriteLine(recipe.Instructions);
                    writer.WriteLine();
                }
            }
            Console.WriteLine($"Report saved to {filePath}");
        }
        else
        {
            foreach (var recipe in recipes)
            {
                Console.WriteLine($"Recipe: {recipe.Name}, Cuisine: {recipe.Cuisine}, Cooking Time: {recipe.CookingTime} mins");
                Console.WriteLine("Ingredients:");
                foreach (var ingredient in recipe.Ingredients)
                {
                    Console.WriteLine($"- {ingredient}");
                }
                Console.WriteLine("Instructions:");
                Console.WriteLine(recipe.Instructions);
                Console.WriteLine();
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        RecipeManager manager = new RecipeManager();

        Recipe lasagnaRecipe = new Recipe
        {
            Name = "Lasagna",
            Cuisine = "Italian",
            Ingredients = new List<string> { "pasta", "tomato sauce", "cheese", "ground beef" },
            CookingTime = 60,
            Instructions = "1. Cook pasta. 2. Brown beef. 3. Layer pasta, sauce, beef, and cheese. 4. Bake at 350°F for 30 minutes."
        };
        manager.AddRecipe(lasagnaRecipe);

        List<Recipe> italianRecipes = manager.SearchRecipesByCuisine("Italian");

        List<Recipe> pastaRecipes = manager.SearchRecipesByIngredient("pasta");

        List<Recipe> quickRecipes = manager.SearchRecipesByCookingTime(30);
        Recipe updatedLasagnaRecipe = new Recipe
        {
            Name = "Lasagna",
            Cuisine = "Italian",
            Ingredients = new List<string> { "pasta", "tomato sauce", "cheese", "ground beef", "spinach" },
            CookingTime = 70,
            Instructions = "1. Cook pasta. 2. Brown beef. 3. Layer pasta, sauce, beef, spinach, and cheese. 4. Bake at 350°F for 40 minutes."
        };
        manager.UpdateRecipe("Lasagna", updatedLasagnaRecipe);

        manager.SaveRecipesToFile("recipes.txt");

     
        manager.LoadRecipesFromFile("recipes.txt");
 
        manager.GenerateReportByCuisine("Italian", "italian_recipes.txt");

        manager.GenerateReportByIngredient("cheese", "cheese_recipes.txt");

        manager.GenerateReportByCookingTime(70, "quick_recipes.txt");

        manager.GenerateReportByName("Lasagna", "Lasagna.txt");

        manager.RemoveRecipe("Lasagna");
    }
}