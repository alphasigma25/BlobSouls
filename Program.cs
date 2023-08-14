using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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

    [SuppressMessage("Blocker Bug", "S2190:Loops and recursions should not be infinite", Justification = "")]
    private static void TestSimulation()
    {
        const float groupHelper = 0.7f;

        static IEnumerable<Soul> Generator(float mean)
        {
            const float vari = 0.2f;
            Distribution rndGen = new();
            while (true)
                yield return new Soul(rndGen.GetGaussian(mean, vari));
        }

        BlobTeam t1 = new(10, () => groupHelper, Generator(0.5f));

        BlobTeam t2 = new(10, () => groupHelper, Generator(0));

        BlobTeam t3 = new(10, () => groupHelper, Generator(-0.5f));

        BlobSimulationManager bsm = new(10, new[] { t1, t2, t3 }, 0.2f);

        bsm.RunSimulation(500);
    }
}