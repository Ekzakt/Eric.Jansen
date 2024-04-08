using Ekzakt.Utilities.Helpers;
using Eric.Jansen.Infrastructure.Configuration;

namespace Eric.Jansen.Infrastructure.Extensions;

public static class EricJansenBackgroundServiceIntervalOptionsExtensions
{
    public static int GetInterval(this EricJansenBackgroundServiceIntervalOptions options)
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
