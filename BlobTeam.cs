using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlobSouls;

internal class BlobTeam
{
    public List<Blob> Blobs { get; }

    public int Count => Blobs.Count;

    public int NbBlobs { get; }

    public int TeamNumber { get; }

    private readonly Func<float> groupHelperDistribution;

    private readonly Func<Soul> soulDistribution;

    public int Construction { get; set; }

    private float SoulCoefMean => Blobs.Average(blob => blob.Soul.Coef);

    public float AverageHealth => Blobs.Average(blob => (float)blob.Health);

    public string Stats()
    {
        StringBuilder sb = new();
        sb.Append("Team ")
            .Append(TeamNumber)
            .Append(" : \n")
            .Append("Number of blobs : ")
            .Append(Blobs.Count)
            .Append(" \n")
            .Append("Average health : ")
            .Append(Blobs.Average(blob => blob.Health))
            .Append(" \n")
            .Append("Construction : ")
            .Append(Construction)
            .Append(" \n")
            .Append("SoulCoefMean : ")
            .Append(SoulCoefMean)
            .Append(" \n");

        return sb.ToString();
    }

    public BlobTeam(
        int nbBlobs,
        Func<float> groupHelperDistribution,
        Func<Soul> soulDistribution)
    {
        TeamNumber = NumberOfTeams++;
        Construction = 0;
        NbBlobs = nbBlobs;
        Blobs = new();
        this.groupHelperDistribution = groupHelperDistribution;
        this.soulDistribution = soulDistribution;
        CreateBlobs();
    }

    public void RemoveDeadBlobs() => Blobs.RemoveAll(blob => blob.Health <= 0);

    public void CreateBlobs()
    {
        while (Blobs.Count < NbBlobs)
        {
            Blobs.Add(new Blob(
                soulDistribution(),
                groupHelperDistribution(),
                TeamNumber));
        }
    }

    public static int NumberOfTeams { get; set; }
}
