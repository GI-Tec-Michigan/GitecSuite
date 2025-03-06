using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Gitec.Utilities;

    public static class StringExtensions
    {
        /// <summary>
        /// Truncates the string to the specified maximum length.
        /// Optionally appends an ellipsis ("...") if truncated.
        /// </summary>
        public static string Truncate(this string value, int maxLength, bool addEllipsis = false)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            if (value.Length <= maxLength)
                return value;
            
            if (addEllipsis && maxLength > 3)
                return value[..(maxLength - 3)] + "...";
            
            return value[..maxLength];
        }
        
        /// <summary>
        /// Converts the string to camelCase.
        /// It splits the string on spaces, underscores, and dashes, lowercases the first word, 
        /// and title-cases subsequent words.
        /// </summary>
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            
            // Split on spaces, underscores, or dashes
            var parts = Regex.Split(value, @"[\s_\-]+")
                             .Where(part => !string.IsNullOrEmpty(part))
                             .ToArray();
            if (parts.Length == 0) return string.Empty;
            
            var firstPart = parts[0].ToLowerInvariant();
            var restParts = parts.Skip(1)
                                 .Select(part => char.ToUpperInvariant(part[0]) + part.Substring(1).ToLowerInvariant());
            
            return firstPart + string.Concat(restParts);
        }
        
        /// <summary>
        /// Converts the string to PascalCase.
        /// It splits the string on spaces, underscores, and dashes, then title-cases each part.
        /// </summary>
        public static string ToPascalCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            
            var parts = Regex.Split(value, @"[\s_\-]+")
                             .Where(part => !string.IsNullOrEmpty(part))
                             .ToArray();
            if (parts.Length == 0) return string.Empty;
            
            return string.Concat(parts.Select(part => char.ToUpperInvariant(part[0]) + part.Substring(1).ToLowerInvariant()));
        }
        
        /// <summary>
        /// Sanitizes the string to be a valid file name by removing invalid file name characters and trimming spaces.
        /// </summary>
        public static string ToFileName(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            // Separate the file name and its extension
            string fileName = Path.GetFileNameWithoutExtension(value);
            string extension = Path.GetExtension(value);

            // Remove invalid file name characters from the file name part only
            var invalidChars = Path.GetInvalidFileNameChars();
            string sanitizedFileName = invalidChars.Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty))
                .Trim();

            // Return the sanitized file name with the original extension appended
            return sanitizedFileName + extension;
        }
        
        /// <summary>
        /// Converts the string into a URL-friendly slug.
        /// This involves lowercasing the string, removing diacritics, and replacing spaces with dashes.
        /// </summary>
        public static string ToSlug(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            // Convert to lower case and remove diacritics.
            string normalized = value.ToLowerInvariant().RemoveDiacritics();

            // Remove invalid characters.
            normalized = Regex.Replace(normalized, @"[^a-z0-9\s-]", string.Empty);

            // Convert multiple spaces into one space and trim.
            normalized = Regex.Replace(normalized, @"\s+", " ").Trim();

            // Replace spaces with dashes.
            normalized = Regex.Replace(normalized, @"\s", "-");
            
            return normalized;
        }
        
        /// <summary>
        /// Removes diacritics (accent marks) from the string.
        /// </summary>
        public static string RemoveDiacritics(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            
            var normalized = value.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
        
        /// <summary>
        /// Reverses the string.
        /// </summary>
        public static string Reverse(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            
            var array = value.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }
    }