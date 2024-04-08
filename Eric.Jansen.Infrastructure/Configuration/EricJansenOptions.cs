﻿namespace Eric.Jansen.Infrastructure.Configuration;

public class EricJansenOptions
{
    public const string SectionName = "EricJansen";

    public EricJansenQueueNamesOptions QueueNames { get; init; } = new();

    public EricJansenBaseLocationOptions BaseLocations { get; init; } = new();

    public List<EricJansenBackgroundServiceOptions> BackgroundServices { get; init; } = [];
}
