using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlobSouls;

internal class BlobSimulationManager
{
    private readonly BlobTeam[] teams;

    private readonly List<Blob>[,] blobGrid;

    private readonly bool[,] materialGrid;

    private readonly int gridSize;

    private readonly float materialSpawnProb;

    public BlobSimulationManager(int gridSize, float materialSpawnProb, params BlobTeam[] teams)
    {
        this.gridSize = gridSize;
        this.materialSpawnProb = materialSpawnProb;
        this.teams = teams;

        Random rnd = new(Random.Shared.Next());

        blobGrid = new List<Blob>[gridSize, gridSize];
        materialGrid = new bool[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                blobGrid[i, j] = new List<Blob>();
                materialGrid[i, j] = rnd.NextDouble() > this.materialSpawnProb;
            }
        }
    }

    public string Stats()
    {
        return $"""
            Global Stats :
                Total number of blobs : {teams.Sum(team => team.Count)}
                Average construction : {teams.Average(team => team.Construction)}
                Average health : {teams.Average(team => team.AverageHealth)}
            """;
    }

    public string TeamsStats()
    {
        StringBuilder sb = new();

        sb.Append("Teams Stats :\n");

        foreach (BlobTeam team in teams)
            sb.Append(team.Stats());

        return sb.ToString();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1235:Optimize method call.", Justification = "<En attente>")]
    private void MoveBlobsOnGrid()
    {
        Random rnd = new();

        foreach (BlobTeam team in teams)
        {
            foreach (Blob blob in team.Blobs)
                blobGrid[rnd.Next(gridSize), rnd.Next(gridSize)].Add(blob);
        }
    }

    private void PerformAttacks()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (blobGrid[i, j].Count > 1)
                {
                    for (int k = 0; k < blobGrid[i, j].Count; k++)
                    {
                        for (int l = k + 1; l < blobGrid[i, j].Count; l++)
                            ManageInteraction(blobGrid[i, j][k], blobGrid[i, j][l]);
                    }
                    blobGrid[i, j].RemoveAll(blob => blob.Health <= 0);
                }
            }
        }
    }

    private int numberOfAlliesHealed;
    private int numberOfEnemiesHealed;
    private int numberOfEnemiesAttacked;
    private int numberOfAlliesAttacked;
    private int numberOfPassiveInteractions;

    private void ManageInteraction(Blob b1, Blob b2)
    {
        Random rnd = new();
        bool areAllies = b1.TeamNumber == b2.TeamNumber;

        bool b1Attack = (areAllies && rnd.NextDouble() > b1.AttackAlly) || (!areAllies && rnd.NextDouble() > b1.AttackEnemy);
        bool b2Attack = (areAllies && rnd.NextDouble() > b2.AttackAlly) || (!areAllies && rnd.NextDouble() > b2.AttackEnemy);
        bool b1Heal = (areAllies && rnd.NextDouble() > b1.HealAlly) || (!areAllies && rnd.NextDouble() > b1.HealEnemy);
        bool b2Heal = (areAllies && rnd.NextDouble() > b2.HealAlly) || (!areAllies && rnd.NextDouble() > b2.HealEnemy);

        if (b1Attack && b2Attack)
        {
            b1.GetAttacked(rnd.Next(0, 100));
            b2.GetAttacked(rnd.Next(0, 100));
            if (areAllies)
                numberOfAlliesAttacked += 2;
            else
                numberOfEnemiesAttacked += 2;
        }
        else if (b1Attack)
        {
            b2.GetAttacked(rnd.Next(30, 100));
            b1.GetAttacked(rnd.Next(0, 70));
            if (areAllies)
                numberOfAlliesAttacked++;
            else
                numberOfEnemiesAttacked++;
        }
        else if (b2Attack)
        {
            b1.GetAttacked(rnd.Next(30, 100));
            b2.GetAttacked(rnd.Next(0, 70));
            if (areAllies)
                numberOfAlliesAttacked++;
            else
                numberOfEnemiesAttacked++;
        }
        else
        {
            if (b1Heal)
            {
                b2.GetHealed();
                if (areAllies)
                    numberOfAlliesHealed++;
                else
                    numberOfEnemiesHealed++;
            }
            if (b2Heal)
            {
                b1.GetHealed();
                if (areAllies)
                    numberOfAlliesHealed++;
                else
                    numberOfEnemiesHealed++;
            }
            else
            {
                numberOfPassiveInteractions++;
            }
        }

        if (b1.Health <= 0)
            numberOfKilled++;

        if (b2.Health <= 0)
            numberOfKilled++;
    }

    private int numberOfKilled;

    private void BlobCollectMaterial()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (materialGrid[i, j])
                {
                    foreach (Blob blob in blobGrid[i, j])
                        teams[blob.TeamNumber].Construction += 1 / blobGrid[i, j].Count;

                    materialGrid[i, j] = false;
                }
            }
        }
    }

    private void GoBackHome()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
                blobGrid[i, j].Clear();
        }

        foreach (BlobTeam team in teams)
            team.RemoveDeadBlobs();
    }

    private void EndOfDay()
    {
        GoBackHome();
        foreach (BlobTeam team in teams)
        {
            team.RemoveDeadBlobs();
            team.CreateBlobs();
            foreach (Blob blob in team.Blobs)
            {
                if (blob.Health <= 90)
                    blob.Health += 10;
            }
        }
        MaterialGrowth();
    }

    private void MaterialGrowth()
    {
        Random rnd = new();
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
                materialGrid[i, j] = materialGrid[i, j] || rnd.NextDouble() > materialSpawnProb;
        }
    }

    public void SimulationStep(int numberOfHours)
    {
        for (int i = 0; i < numberOfHours; i++)
        {
            MoveBlobsOnGrid();
            PerformAttacks();
            foreach (BlobTeam team in teams)
                team.RemoveDeadBlobs();

            BlobCollectMaterial();
            GoBackHome();
        }
        EndOfDay();
    }

    public void RunSimulation(int nbSteps, bool verbose = false)
    {
        Console.WriteLine(
            $"""
            Initial state :
            {Stats()}
            {TeamsStats()}
            """);

        for (int i = 0; i < nbSteps; i++)
        {
            if (verbose)
                Console.WriteLine($"Step {i} : ");

            SimulationStep(20);

            if (verbose)
                Console.WriteLine(Stats());
        }

        Console.WriteLine("Final state : ");
        SimulationStep(5);

        Console.WriteLine(
            $"""
            {Stats()}
            {TeamsStats()}
            Number of allies healed : {numberOfAlliesHealed}
            Number of enemies healed : {numberOfEnemiesHealed}
            Number of allies attacked : {numberOfAlliesAttacked}
            Number of enemies attacked : {numberOfEnemiesAttacked}
            Number of passive interactions : {numberOfPassiveInteractions}
            Number of killed blobs : {numberOfKilled}
            """);
    }
}