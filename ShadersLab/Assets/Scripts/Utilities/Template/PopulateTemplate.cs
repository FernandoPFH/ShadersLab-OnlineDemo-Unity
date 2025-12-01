using System.Collections.Generic;

public static class StringExtension
{
    public static string PopulateTemplate(this string template, Dictionary<string, string> replaces)
    {
        string result = template;

        foreach (string replaceKey in replaces.Keys)
            result = result.Replace("{{" + replaceKey + "}}", replaces[replaceKey]);

        return result;
    }
}