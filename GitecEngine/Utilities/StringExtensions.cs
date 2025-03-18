using System.Text;
using System.Text.RegularExpressions;

namespace GitecEngine.Utilities;

public static class StringExtensions
{
    public static string ToSlug(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;
        var normalized = str.Normalize(NormalizationForm.FormD);
        normalized = Regex.Replace(normalized, @"\p{M}", string.Empty); 
        normalized = Regex.Replace(normalized, @"[^a-zA-Z0-9\s-]", string.Empty);
        normalized = Regex.Replace(normalized, @"\s+", " ").Trim();
        normalized = Regex.Replace(normalized, @"\s", "-");
        return normalized.ToLowerInvariant();
    }
    
}