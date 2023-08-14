using System;

namespace BlobSouls;

internal abstract class IDistribution<T>
{
    internal abstract T GetValue();

    public static implicit operator IDistribution<T>(T val) => new ConstDistribution<T>(val);
}

internal abstract class RandomDistribution<T> : IDistribution<T>
{
    private protected RandomDistribution()
    {
        Rnd = new Random(Random.Shared.Next());
    }

    private protected Random Rnd { get; }
}

internal class ConstDistribution<T> : IDistribution<T>
{
    public ConstDistribution(T val)
    {
        this.val = val;
    }

    internal override T GetValue() => val;

    private readonly T val;
}

internal class UniformDistribution : RandomDistribution<float>
{
    internal UniformDistribution(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    internal override float GetValue() => ((float)Rnd.NextDouble() * (max - min)) + min;

    private readonly float min;
    private readonly float max;
}

internal class GaussianDistribution : RandomDistribution<float>
{
    internal GaussianDistribution(float mean, float stdDev)
    {
        this.mean = mean;
        this.stdDev = stdDev;
    }

    internal override float GetValue()
    {
        double u1 = 1.0 - Rnd.NextDouble(); // uniform(0,1] random doubles

        double u2 = 1.0 - Rnd.NextDouble();

        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                     Math.Sin(2.0 * Math.PI * u2); // random normal(0,1)

        double randNormal = mean + (stdDev * randStdNormal); // random normal(mean,stdDev^2)

        return (float)randNormal;
    }

    private readonly float mean;
    private readonly float stdDev;
}

internal class SoulDistribution : IDistribution<Soul>
{
    internal SoulDistribution(IDistribution<float> distrib)
    {
        this.distrib = distrib;
    }

    internal override Soul GetValue() => new(distrib.GetValue());

    private readonly IDistribution<float> distrib;
}