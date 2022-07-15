using System;

namespace Codecool.BookDb.View;

public class UserInterface
{
    public void PrintLn(Object obj)
    {
        Console.WriteLine(obj);
    }

    public void PrintTitle(string title)
    {
        Console.WriteLine($"{Environment.NewLine} -- {title} --");
    }

    public void PrintOption(char option, string description)
    {
        Console.WriteLine($"({option}) {description}");
    }

    public void Clear()
    {
        Console.Clear();
    }

    /// <summary>
    /// Keep asking user for input until it is one of the provided chars
    /// </summary>
    /// <param name="options">Available characters as options like "abcd"</param>
    /// <returns>Choosen option</returns>
    public char Choice(string options)
    {
        string userInput;
        do
        {
            Console.WriteLine($"Choose [{options}]: ");
            userInput = Console.ReadLine();
        } while ((string.IsNullOrEmpty(userInput)
                || userInput.Length != 1)
                && !options.Contains(userInput));

        return userInput[0];
    }

    /// <summary>
    /// Ask user for data. If no data was provided use default value.
    /// </summary>
    /// <param name="prompt">Text to show before cursor</param>
    /// <param name="defaultValue">If only hit enter this option will be chosen.</param>
    /// <returns>Read string</returns>
    public string ReadString(string prompt, string defaultValue)
    {
        PrintPrompt(prompt, defaultValue);
        var userInput = Console.ReadLine();
        return string.IsNullOrEmpty(userInput) ? defaultValue : userInput;
    }

    private void PrintPrompt(string prompt, object defaultValue)
    {
        Console.WriteLine($"{prompt} [{defaultValue}]: ");
    }

    /// <summary>
    /// Ask user for a date. 
    /// User must be informed what the default value is.
    /// If provided date is in invalid format, ask user again.
    /// </summary>
    /// <param name="prompt">Text to show before cursor</param>
    /// <param name="defaultValue">If no data was provided use default value.</param>
    /// <returns>Date value received fomr the user or default</returns>
    public DateOnly ReadDate(string prompt, DateOnly defaultValue)
    {
        while (true)
        {
            PrintPrompt(prompt, defaultValue.ToString("MM/dd/yyyy"));
            var userInput = Console.ReadLine();

            if (string.IsNullOrEmpty(userInput))
                return defaultValue;

            try
            {
                return DateOnly.Parse(userInput);
            }
            catch (FormatException)
            {
                Console.WriteLine("Bad date format! Specify in the following way: dd/mm/yyyy");
            }
        }
    }

    /// <summary>
    /// Ask user for a number. User must be informed what the default value is.
    /// </summary>
    /// <param name="prompt">Text to show before cursor</param>
    /// <param name="defaultValue">If no data was provided this value used.</param>
    /// <returns></returns>
    public int ReadInt(string prompt, int defaultValue)
    {
        while (true)
        {
            PrintPrompt(prompt, defaultValue);
            var userInput = Console.ReadLine();

            if (string.IsNullOrEmpty(userInput))
                return defaultValue;

            try
            {
                return int.Parse(userInput);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Enter an integer!");
            }
        }
    }
}