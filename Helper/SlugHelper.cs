using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

public static class SlugHelper
{
    public static string StringToSlug(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        // Chuyển đổi sang chữ thường và loại bỏ các dấu
        string normalizedString = input.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new StringBuilder();

        foreach (char c in normalizedString)
        {
            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory == UnicodeCategory.LowercaseLetter || 
                unicodeCategory == UnicodeCategory.UppercaseLetter ||
                unicodeCategory == UnicodeCategory.DecimalDigitNumber)
            {
                stringBuilder.Append(c);
            }
            else if (unicodeCategory == UnicodeCategory.SpaceSeparator ||
                     unicodeCategory == UnicodeCategory.ConnectorPunctuation ||
                     unicodeCategory == UnicodeCategory.DashPunctuation)
            {
                stringBuilder.Append('-');
            }
        }

        string slug = stringBuilder.ToString();

        // Loại bỏ các dấu gạch ngang thừa
        slug = Regex.Replace(slug, @"[-]+", "-").Trim('-');

        return slug;
    }
}






[Serializable]
public class MyClass
{

}


