// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class MoneyItem
{
    public string Title { get; set; }
    public double Amount { get; set; }
    public string Month { get; set; }
    public bool IsExpense { get; set; }
}

class MoneyTrackingApp
{
    private List<MoneyItem> items = new List<MoneyItem>();

    //// Methods for adding, displaying, editing, and removing items

    public void AddItem(string title, double amount, string month, bool isExpense)
    {
        var item = new MoneyItem { Title = title, Amount = amount, Month = month, IsExpense = isExpense };
        items.Add(item);
    }

    public void DisplayItems(string sortKey = "month", bool descending = false, bool showExpenses = true, bool showIncomes = true)
    {
        var filteredItems = items
            .Where(item => (showExpenses && item.IsExpense) || (showIncomes && !item.IsExpense))
            .ToList();

        var sortedItems = descending
            ? filteredItems.OrderByDescending(item => GetPropertyValue(item, sortKey)).ToList()
            : filteredItems.OrderBy(item => GetPropertyValue(item, sortKey)).ToList();

        foreach (var item in sortedItems)
        {
            Console.WriteLine($"{item.Title} - {item.Amount} - {item.Month} - {(item.IsExpense ? "Expense" : "Income")}");
        }
    }

    public void EditItem(int index, string title, double amount, string month)
    {
        if (index >= 0 && index < items.Count)
        {
            var item = items[index];
            item.Title = title;
            item.Amount = amount;
            item.Month = month;
        }
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            items.RemoveAt(index);
        }
    }

    public double CalculateBalance()

    // Method to calculate the account balance
    {
        double income = items.Where(item => !item.IsExpense).Sum(item => item.Amount);
        double expenses = items.Where(item => item.IsExpense).Sum(item => item.Amount);

        return income - expenses;
    }

    public void SaveToFile(string filename = "money_data.txt")

    //// Method to save items to a file
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (var item in items)
            {
                writer.WriteLine($"{item.Title},{item.Amount},{item.Month},{item.IsExpense}");
            }
        }
    }

    public void LoadFromFile(string filename = "money_data.txt")

    //Method to load items from a file
    {
        try
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        var item = new MoneyItem
                        {
                            Title = parts[0],
                            Amount = double.Parse(parts[1]),
                            Month = parts[2],
                            IsExpense = bool.Parse(parts[3])
                        };
                        items.Add(item);
                    }
                }
            }
        }
        catch (FileNotFoundException)
        {
            items = new List<MoneyItem>(); // Start with an empty list if the file is not found
        }
    }

    private object GetPropertyValue(MoneyItem item, string propertyName)

    //Helper method to get the value of a property dynamically
    {
        return typeof(MoneyItem).GetProperty(propertyName)?.GetValue(item, null);
    }
}

class Program
{
    static void Main()
    {
        var app = new MoneyTrackingApp();

        // Load previous data if available

        app.LoadFromFile();

        // Display menu options and get user input

        while (true)
        {
            Console.WriteLine("\nMoney Tracking Application");
            Console.WriteLine("1. Add Item");
            Console.WriteLine("2. Display Items");
            Console.WriteLine("3. Edit Item");
            Console.WriteLine("4. Remove Item");
            Console.WriteLine("5. Show Balance");
            Console.WriteLine("6. Save and Quit");

            Console.Write("Enter your choice (1-6): ");
            var choice = Console.ReadLine();

            switch (choice)

            // Implement cases for each menu option
            {
                case "1":
                    Console.Write("Enter title: ");
                    var title = Console.ReadLine();
                    Console.Write("Enter amount: ");
                    var amount = double.Parse(Console.ReadLine());
                    Console.Write("Enter month: ");
                    var month = Console.ReadLine();
                    Console.Write("Is it an expense? (y/n): ");
                    var isExpense = Console.ReadLine().ToLower() == "y";
                    app.AddItem(title, amount, month, isExpense);
                    break;
                case "2":
                    Console.Write("Sort by (title/amount/month): ");
                    var sortKey = Console.ReadLine().ToLower();
                    Console.Write("Sort in descending order? (y/n): ");
                    var descending = Console.ReadLine().ToLower() == "y";
                    Console.Write("Show expenses? (y/n): ");
                    var showExpenses = Console.ReadLine().ToLower() == "y";
                    Console.Write("Show incomes? (y/n): ");
                    var showIncomes = Console.ReadLine().ToLower() == "y";
                    app.DisplayItems(sortKey, descending, showExpenses, showIncomes);
                    break;
                case "3":
                    Console.Write("Enter index to edit: ");
                    var editIndex = int.Parse(Console.ReadLine());
                    Console.Write("Enter new title: ");
                    title = Console.ReadLine();
                    Console.Write("Enter new amount: ");
                    amount = double.Parse(Console.ReadLine());
                    Console.Write("Enter new month: ");
                    month = Console.ReadLine();
                    app.EditItem(editIndex, title, amount, month);
                    break;
                case "4":
                    Console.Write("Enter index to remove: ");
                    var removeIndex = int.Parse(Console.ReadLine());
                    app.RemoveItem(removeIndex);
                    break;
                case "5":
                    Console.WriteLine($"Current Account Balance: {app.CalculateBalance()}");
                    break;
                case "6":
                    app.SaveToFile();
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 6.");
                    break;
            }
        }
    }
}
