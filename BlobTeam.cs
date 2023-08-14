using System;
using System.Collections.Generic;
using System.Linq;

namespace BlobSouls;

internal class BlobTeam
{
    public List<Blob> Blobs { get; }

    public int Count => Blobs.Count;

    public int NbBlobs { get; }

    public int TeamNumber { get; }

    private readonly Func<float> groupHelperDistribution;

    private readonly IDistribution<float> soulDistribution;

    public int Construction { get; set; }

    private float SoulCoefMean => Blobs.Average(blob => blob.Soul.Coef);

    public float AverageHealth => Blobs.Average(blob => (float)blob.Health);

    public string Stats()
    {
        return $"""
            Team {TeamNumber} :
                Number of blobs : {Blobs.Count}
                Average health : {Blobs.Average(blob => blob.Health)}
                Construction : {Construction}
                SoulCoefMean : {SoulCoefMean}
            """;
    }

    public BlobTeam(
        int nbBlobs,
        Func<float> groupHelperDistribution,
        IDistribution<float> soulDistribution)
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
                new Soul(soulDistribution.GetValue()),
                groupHelperDistribution(),
                TeamNumber));
        }
    }

    public static int NumberOfTeams { get; set; }
}
