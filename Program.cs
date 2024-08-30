using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

class Program
{
    static string[] lines;

    static void Main()
    {
        string filePath = "input.csv";

        while (true)
        {
            //reads from file each loop
            lines = File.ReadAllLines(filePath); 
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Display Characters");
            Console.WriteLine("2. Add Character");
            Console.WriteLine("3. Level Up Character");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayAllCharacters(lines);
                    break;
                case "2":
                    AddCharacter(ref lines, filePath);
                    break;
                case "3":
                    LevelUpCharacter(lines, filePath);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void DisplayAllCharacters(string[] lines)
    {
        // Skip the header row
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            string name;
            string fields;

            // Check if the name is quoted
            if (line.StartsWith('"'))
            {

                var firstQuotePos = lines[i].IndexOf('"');
                name = lines[i].Substring(firstQuotePos + 1);
                var lastQuotePos = name.IndexOf('"');

                name = name.Substring(firstQuotePos, lastQuotePos-firstQuotePos);
                fields = line.Substring(lastQuotePos + 1);
            }
            else
            {
                name = lines[i].Split(",")[0];
                int firstComma = line.IndexOf(",");
                fields = line.Substring(firstComma);
            }
            //writing fields
            Console.WriteLine("Name: " + name);
            Console.WriteLine("Class: " + fields.Split(",")[1]);
            Console.WriteLine("Level: " + fields.Split(",")[2]);
            Console.WriteLine("HP: " + fields.Split(",")[3]);
            string equipmentLine = fields.Split(",")[4];
            string[] equipment = equipmentLine.Split('|');
            Console.WriteLine("Equipment List:");
            for (int j = 0; j < equipment.Length; j++)
            {
                Console.WriteLine(" - " + equipment[j]);
            }
        }
    }

    static void AddCharacter(ref string[] lines, string filePath)
    {
        Console.Write("\nEnter your character's name: ");
        string name = Console.ReadLine();

        Console.Write("\nEnter your character's class: ");
        string characterClass = Console.ReadLine();

        int level;
        while (true)
        {
            try
            {
                Console.Write("\nEnter your character's level: ");
                level = Convert.ToInt32(Console.ReadLine());
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter an integer.");
            }
        }

        int health;
        while (true)
        {
            try
            {
                Console.Write("\nEnter your character's HP: ");
                health = Convert.ToInt32(Console.ReadLine());
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter an integer.");
            }
        }

        var equipment = new List<String>();
        int equipmentIndex = 0;
        while (true)
        {
            Console.WriteLine("Enter new equipment name. Type 0 to end: ");
            Console.Write("> ");
            string equipmentText = Console.ReadLine();
            if (equipmentText == "0")
            {
                break;
            }
            else
            {
                equipment.Add(equipmentText);
            }
        }

        Console.WriteLine($"\nWelcome, {name} the {characterClass}! You are level {level} and your equipment includes: {string.Join(", ", equipment)}.");

        string character = string.Format("\n{0},{1},{2},{3},{4}", name, characterClass, level, health, String.Join("|", equipment));

        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(character);
            writer.Close();
        }
        

    }

    static void LevelUpCharacter(string[] lines, string filePath)
    {
        Console.Write("Enter the name of the character to level up: ");
        string nameToLevelUp = Console.ReadLine();

        // Loop through characters to find the one to level up
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            String characterName = line.Split(",")[0];

            // TODO: Check if the name matches the one to level up
            // Do not worry about case sensitivity at this point
            if (line.Contains(nameToLevelUp))
            {
                int commaIndex;
                commaIndex = line.IndexOf(',');
                string level = lines[i].Split(",")[2];
                level = (int.Parse(level) + 1).ToString();
                string[] UpdatedCharacter = new string[] { lines[i].Split(",")[0], lines[i].Split(",")[1], level, lines[i].Split(",")[3], lines[i].Split(",")[4] };
                lines[i] = String.Join(",", UpdatedCharacter);

                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    for (int w = 1; w < lines.Length; w++)
                    {
                        writer.WriteLine(lines[w]);
                    }
                    writer.Close();
                }

                Console.WriteLine("\n" + characterName + " is now level " + level);

                break;
            }
        }
    }
}