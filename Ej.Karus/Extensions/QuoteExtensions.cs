using Ej.Karus.Models;
using System.Globalization;

namespace Ej.Karus.Extensions;

public static class QuoteExtensions
{
    public static string GetTimestamp(this Quote? quote)
    {
        if (quote is null)
        {
            return string.Empty;
        }

        if (quote.Date.HasValue)
        {
            return quote.Date.Value.ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture);
        }

        if (quote.Year.HasValue)
        {
            return quote.Year.Value.ToString(CultureInfo.InvariantCulture);
        }

        return string.Empty;
    }


    public static bool HasTimestamp(this Quote? quote)
    {
        if (quote is null)
        {
            return false;
        }

        return quote.Date.HasValue || quote.Year.HasValue;
    }


    public static bool HasAuthor(this Quote? quote)
    {
        if (quote is null)
        {
            return false;
        }

        return !string.IsNullOrEmpty(quote.Author);
    }


    public static bool HasLocation(this Quote? quote)
    {
        if (quote is null)
        {
            return false;
        }

        return !string.IsNullOrEmpty(quote.Location);
    }

    public static string? GetSignature(this Quote? quote)
    {
        if (quote is null)
        {
            return string.Empty;
        }


        List<string?> signatureItems = [];

        if (quote.HasAuthor())
        {
            signatureItems.Add(quote.Author);
        }

        if (quote.HasTimestamp())
        {
            signatureItems.Add(quote.GetTimestamp());
        }

        if (quote.HasLocation())
        {
            signatureItems.Add(quote.Location);
        }

        return string.Join(", ", signatureItems);

    }
}
