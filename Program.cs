namespace BlobSouls;

internal class Program
{
    static void Main(string[] args)
    {
        Distribution rndGen = new();

        float vari = 0.3f;

        BlobTeam t1 = new(
            10,
            () => rndGen.getUniform(0, 1),
            () => new Soul(rndGen.getGaussian(0.5f, vari))
        );

        BlobTeam t2 = new(
            10,
            () => rndGen.getUniform(0, 1),
            () => new Soul(rndGen.getGaussian(0, vari))
        );

        BlobTeam t3 = new(
            10,
            () => rndGen.getUniform(0, 1),
            () => new Soul(rndGen.getGaussian(-0.5f, vari))
        );

        BlobSimulationManager bsm = new(10, new[] { t1, t2, t3 }, 0.2f);

        bsm.RunSimulation(500);

    }
}
