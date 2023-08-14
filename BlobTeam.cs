using System.Text;

namespace BlobSouls;
internal class BlobTeam
{
    public List<Blob> Blobs;
    public int Count => Blobs.Count;
    public int nbBlobs { get; private set; }
    public int teamNumber { get; private set; }

    private Func<float> groupHelperDistribution;
    private Func<Soul> soulDistribution;

    public int Construction { get; set; }
    private float SoulCoefMean => Blobs.Average(blob => blob.Soul.Coef);

    public float AverageHealth => Blobs.Average(blob => (float)blob.health);

    public string Stats()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"Team {teamNumber} : \n");
        sb.Append($"Number of blobs : {Blobs.Count} \n");
        sb.Append($"Average health : {Blobs.Average(blob => blob.health)} \n");
        sb.Append($"Construction : {Construction} \n");
        sb.Append($"SoulCoefMean : {SoulCoefMean} \n");
        return sb.ToString();
    }

    public BlobTeam(
        int nbBlobs,
        Func<float> groupHelperDistribution,
        Func<Soul> soulDistribution
    )
    {
        teamNumber = numberOfTeams++;
        Construction = 0;
        this.nbBlobs = nbBlobs;
        Blobs = new();
        this.groupHelperDistribution = groupHelperDistribution;
        this.soulDistribution = soulDistribution;
        CreateBlobs();
    }

    public void RemoveDeadBlobs()
    {
        Blobs.RemoveAll(blob => blob.health <= 0);
    }

    public void CreateBlobs()
    {
        while (Blobs.Count < nbBlobs)
        {
            Blobs.Add(new Blob(soulDistribution(),
                groupHelperDistribution(), teamNumber));
        }
    }

    static public int numberOfTeams { get; set; } = 0;
}
