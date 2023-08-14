using System;

namespace BlobSouls;

internal class Distribution
{
    private readonly Random rnd;

    public Distribution()
    {
        rnd = new Random();
    }

    public float GetUniform(float min, float max) => ((float)rnd.NextDouble() * (max - min)) + min;

    public float GetGaussian(float mean, float stdDev)
    {
        double u1 = 1.0 - rnd.NextDouble(); // uniform(0,1] random doubles

        double u2 = 1.0 - rnd.NextDouble();

        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                     Math.Sin(2.0 * Math.PI * u2); // random normal(0,1)

        double randNormal = mean + (stdDev * randStdNormal); // random normal(mean,stdDev^2)

        return (float)randNormal;
    }
}