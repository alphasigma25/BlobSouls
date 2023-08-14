using System;

namespace BlobSouls;

internal static class Program
{
    private static void Main()
    {
        TestBlobSouls();
        TestSimulation();
    }

    private static void TestBlobSouls()
    {
        Blob b1 = new(new(-0.5f), 1, 0);
        Console.WriteLine(b1.Stats);
    }

    private static void TestSimulation()
    {
        const float groupHelper = 0.7f;
        const float vari = 0.2f;

        BlobTeam t1 = new(10, () => groupHelper, new GaussianDistribution(0.5f, vari));

        BlobTeam t2 = new(10, () => groupHelper, new GaussianDistribution(0, vari));

        BlobTeam t3 = new(10, () => groupHelper, new GaussianDistribution(-0.5f, vari));

        BlobSimulationManager bsm = new(10, new[] { t1, t2, t3 }, 0.2f);

        bsm.RunSimulation(500);
    }
}