using Ekzakt.Utilities.Helpers;
using Ej.Infrastructure.Configuration;

namespace Ej.Infrastructure.Extensions;

public static class BgServiceIntervalOptionsExtensions
{
    public static int GetInterval(this BgServiceIntervalOptions options)
    {
        var DEFAULT_INTERVAL = 60000;

        if (options is null)
        {
            return DEFAULT_INTERVAL;
        }


        if (options?.ValueBetween.Min > 0 && options?.ValueBetween.Max > 0)
        {
            var min = Math.Min(options.ValueBetween.Min, options.ValueBetween.Max);
            var max = Math.Max(options.ValueBetween.Min, options.ValueBetween.Max);

            return IntHelpers.GetRandomIntBetween(min, max);
        }


        if (options?.Value > 0)
        {
            return options.Value;
        }

        return DEFAULT_INTERVAL;
    }
}
