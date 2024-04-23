namespace Ej.Infrastructure.Configuration;

public class BgServiceIntervalOptions
{
    public int Value { get; init; } = 0;

    public IntervalBetweenOptions ValueBetween { get; init; } = new();

}


public class IntervalBetweenOptions
{
    public int Min { get; init; } = 0;

    public int Max { get; init; } = 0;
}