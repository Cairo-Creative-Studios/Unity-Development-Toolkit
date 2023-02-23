//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;

public static class StringExtensions
{
    /// <summary>
    /// Gets the Token at the specified Index in a string
    /// </summary>
    /// <returns>The <see cref="T:System.String"/>.</returns>
    /// <param name="checkedString">The string we're searching for a Token.</param>
    /// <param name="index">The Index to find the token at.</param>
    /// <param name="itemDelimiter">What character Seperated Tokens in the String?.</param>
    public static string TokenAt(this string checkedString, int index, char itemDelimiter = ',')
    {
        char[] characters = checkedString.ToCharArray();

        int token = 0;
        string returnValue = "";

        foreach (char character in characters)
        {
            if (token == index)
            {
                if (character == itemDelimiter)
                    break;
                else
                    returnValue += character;
            }
            else
            {
                if (character == itemDelimiter)
                    token++;
            }
        }

        return returnValue;
    }

    /// <summary>
    /// Counts all tokens seperated by the Delimiter
    /// </summary>
    /// <returns>The count.</returns>
    /// <param name="checkedString">Checked string.</param>
    /// <param name="itemDelimiter">Seperater.</param>
    public static int TokenCount(this string checkedString, char itemDelimiter = ',')
    {
        char[] characters = checkedString.ToCharArray();

        int token = 1;

        foreach (char character in characters)
        {
            if (character == itemDelimiter)
                token++;
        }

        return token;
    }

    /// <summary>
    /// Gets an Array of Tokens from the given String
    /// </summary>
    /// <returns>The array.</returns>
    /// <param name="listString">Checked string.</param>
    /// <param name="itemDelimiter">Seperater.</param>
    public static string[] TokenArray(this string listString, char itemDelimiter = ',')
    {
        List<string> tokens = new List<string>();

        for (int i = 0; i < TokenCount(listString, itemDelimiter); i++)
        {
            tokens.Add(listString.TokenAt(i, itemDelimiter));
        }

        return tokens.ToArray();
    }

    /// <summary>
    /// Replaces the Token at the Index with the passed Value
    /// </summary>
    /// <param name="listString">List string.</param>
    /// <param name="itemDelimiter">Item delimiter.</param>
    public static string SetToken(this string listString, int index, string value, char itemDelimiter = ',')
    {
        string returnedString = "";

        for (int i = 0; i < listString.TokenCount(itemDelimiter); i++)
        {
            if (i == index)
                returnedString = returnedString + value;
            else
                returnedString = returnedString + listString.TokenAt(i, itemDelimiter);

            if (i != listString.TokenCount(itemDelimiter) - 1)
                returnedString = returnedString + itemDelimiter;

            i++;
        }

        return returnedString;
    }

    /// <summary>
    /// Pops the Token at the given Index out of the List String
    /// </summary>
    /// <returns>The at index.</returns>
    /// <param name="listString">List string.</param>
    /// <param name="index">Index.</param>
    /// <param name="itemDelimiter">Item delimiter.</param>
    public static string PopAtIndex(this string listString, int index, char itemDelimiter = ',')
    {
        string returnedString = "";

        for (int i = 0; i < listString.TokenCount(itemDelimiter); i++)
        {
            if (i != index)
            {
                returnedString = returnedString + listString.TokenAt(i, itemDelimiter);

                if (i != listString.TokenCount(itemDelimiter) - 1)
                    returnedString = returnedString + itemDelimiter;
            }
        }

        return returnedString;
    }

    /// <summary>
    /// Parses a string as Dictionary Items
    /// </summary>
    /// <returns>The to dictionary.</returns>
    /// <param name="parsedString">Parsed string.</param>
    /// <param name="itemDelimiter">Item delimiter.</param>
    /// <param name="pairDelimiter">Pair delimiter.</param>
    public static SerializableDictionary<string, string> ParseToDictionary(this string parsedString, char itemDelimiter = ',', char pairDelimiter = ':')
    {
        SerializableDictionary<string, string> result = new SerializableDictionary<string, string>();

        for (int i = 0; i < parsedString.TokenCount(itemDelimiter); i++)
        {
            result.Add(parsedString.TokenAt(i, itemDelimiter).TokenAt(0, pairDelimiter), parsedString.TokenAt(i, itemDelimiter).TokenAt(1, pairDelimiter));
        }

        return result;
    }

    /// <summary>
    /// Converts the Dictionary to a String
    /// </summary>
    /// <returns>The to string.</returns>
    /// <param name="dictionary">Dictionary.</param>
    /// <typeparam name="T1">The 1st type parameter.</typeparam>
    /// <typeparam name="T2">The 2nd type parameter.</typeparam>
    public static string ConvertToString<T1, T2>(this SerializableDictionary<T1, T2> dictionary)
    {
        string result = "";
        bool start = true;

        foreach (T1 item in dictionary.Keys)
        {
            if (!start)
                result = result + ", ";
            result = result + item.ToString() + ":" + dictionary[item].ToString();
            start = false;
        }

        return result;
    }

    /// <summary>
    /// Parses a string as a list of strings
    /// </summary>
    /// <returns>The to list.</returns>
    /// <param name="parsedString">Parsed string.</param>
    /// <param name="itemDelimiter">Item delimiter.</param>
    public static List<string> ParseToList(this string parsedString, char itemDelimiter = ',')
    {
        List<string> result = new List<string>();

        for (int i = 0; i < parsedString.TokenCount(itemDelimiter); i++)
        {
            result.Add(parsedString.TokenAt(i, itemDelimiter));
        }

        return result;
    }

    public static string ConvertToString(this object[] ConvertedArray, char itemDelimiter = ',')
    {
        string result = "";

        foreach (object item in ConvertedArray)
        {
            if (result == "")
                result = item.ToString();
            else
                result = result + itemDelimiter + item.ToString();
        }

        return result;
    }

    public static string ConvertToString(this int[] ConvertedArray, char itemDelimiter = ',')
    {
        string result = "";

        foreach (object item in ConvertedArray)
        {
            if (result == "")
                result = item.ToString();
            else
                result = result + itemDelimiter + item.ToString();
        }

        return result;
    }

    public static string MultiplyString(this string String, int count)
    {
        string result = "";
        for (int i = 0; i < count; i++)
        {
            result = result + String;
        }
        return result;
    }
}
