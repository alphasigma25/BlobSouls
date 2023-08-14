using System;

namespace BlobSouls;

internal interface IDistribution
{
    float GetValue();
}

internal class UniformDistribution : IDistribution
{
    internal UniformDistribution(float min, float max)
    {
        this.min = min;
        this.max = max;

        rnd = new Random(Random.Shared.Next());
    }

    public float GetValue() => ((float)rnd.NextDouble() * (max - min)) + min;

    private readonly float min;
    private readonly float max;
    private readonly Random rnd;
}

internal class GaussianDistribution : IDistribution
{
    internal GaussianDistribution(float mean, float stdDev)
    {
        this.mean = mean;
        this.stdDev = stdDev;
        rnd = new Random(Random.Shared.Next());
    }

    public float GetValue()
    {
        double u1 = 1.0 - rnd.NextDouble(); // uniform(0,1] random doubles

        double u2 = 1.0 - rnd.NextDouble();

        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                     Math.Sin(2.0 * Math.PI * u2); // random normal(0,1)

        double randNormal = mean + (stdDev * randStdNormal); // random normal(mean,stdDev^2)

        return (float)randNormal;
    }

    private readonly float mean;
    private readonly float stdDev;
    private readonly Random rnd;
}