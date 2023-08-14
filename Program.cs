using System;

namespace BlobSouls;

internal static class Program
{
    private static void Main() => TestBlobSouls();

    private static void TestBlobSouls()
    {
        Blob b1 = new(new(-0.5f), 1, 0);
        Console.WriteLine(b1.Stats);
    }

    /* TODO : Clean
    private static void TestSimulation()
    {
        Distribution rndGen = new();

        const float vari = 0.2f;
        const float groupHelper = 0.7f;

        BlobTeam t1 = new(
            10,
            () => groupHelper,
            () => new Soul(rndGen.GetGaussian(0.5f, vari)));

        BlobTeam t2 = new(
            10,
            () => groupHelper,
            () => new Soul(rndGen.GetGaussian(0, vari)));

        BlobTeam t3 = new(
            10,
            () => groupHelper,
            () => new Soul(rndGen.GetGaussian(-0.5f, vari)));

        BlobSimulationManager bsm = new(10, new[] { t1, t2, t3 }, 0.2f);

        bsm.RunSimulation(500);
    }*/
}