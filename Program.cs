namespace BlobSouls;

internal class Program
{
    static void Main(string[] args)
    {
        testBlobSouls();
    }

    static void testBlobSouls()
    {
        Blob b1 = new(new(-0.5f), 1, 0);
        Console.WriteLine(b1.Stats);
    }
    static void testSimulation()
    {
        Distribution rndGen = new();

        float vari = 0.2f;
        float groupHelper = 0.7f;

        BlobTeam t1 = new(
            10,
            () => groupHelper,
            () => new Soul(rndGen.getGaussian(0.5f, vari))
        );

        BlobTeam t2 = new(
            10,
            () => groupHelper,
            () => new Soul(rndGen.getGaussian(0, vari))
        );

        BlobTeam t3 = new(
            10,
            () => groupHelper,
            () => new Soul(rndGen.getGaussian(-0.5f, vari))
        );

        BlobSimulationManager bsm = new(10, new[] { t1, t2, t3 }, 0.2f);

        bsm.RunSimulation(500);
    }
}
