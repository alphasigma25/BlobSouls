using BlobSouls;
using System;
using System.Text;


namespace BlobSouls;
internal class BlobSimulationManager
{
    private BlobTeam[] Teams;

    private List<Blob>[,] BlobGrid;
    private bool[,] MaterialGrid;

    private int GridSize;
    private float MaterialSpawnProb;

    public BlobSimulationManager(int gridSize, BlobTeam[] teams, float materialSpawnProb)
    {
        GridSize = gridSize;
        MaterialSpawnProb = materialSpawnProb;
        Teams = teams;

        Random rnd = new Random();

        BlobGrid = new List<Blob>[gridSize, gridSize];
        MaterialGrid = new bool[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                BlobGrid[i, j] = new List<Blob>();
                MaterialGrid[i, j] = rnd.NextDouble() > MaterialSpawnProb;
            }
        }
    }

    public string Stats()
    {
        StringBuilder sb = new StringBuilder();
        // for each team : number of blobs, average health, value of Construction
        sb.Append("Global Stats : \n");
        sb.Append($"Total number of blobs : {Teams.Sum(team => team.Count)} \n");
        sb.Append($"Average construction : {Teams.Average(team => team.Construction)} \n");
        sb.Append($"Average health : {Teams.Average(team => team.AverageHealth)} \n");
        return sb.ToString();
    }

    public string TeamsStats()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Teams Stats :\n");
        foreach (BlobTeam team in Teams)
        {
            sb.Append(team.Stats());
        }
        return sb.ToString();
    }

    private void MoveBlobsOnGrid()
    {
        Random rnd = new Random();
        foreach (BlobTeam team in Teams)
        {
            foreach (Blob blob in team.Blobs)
            {
                BlobGrid[rnd.Next(GridSize), rnd.Next(GridSize)].Add(blob);
            }
        }
    }

    private void PerformAttacks()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                if (BlobGrid[i, j].Count > 1)
                {
                    for (int k = 0; k < BlobGrid[i, j].Count; k++)
                    {
                        for (int l = k + 1; l < BlobGrid[i, j].Count; l++)
                        {
                            ManageInteraction(BlobGrid[i, j][k], BlobGrid[i, j][l]);
                        }
                    }
                    BlobGrid[i, j].RemoveAll(blob => blob.health <= 0);
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
        Random rnd = new Random();
        bool areAllies = b1.teamNumber == b2.teamNumber;

        bool b1Attack = areAllies && rnd.NextDouble() > b1.attackAlly || !areAllies && rnd.NextDouble() > b1.attackEnemy;
        bool b2Attack = areAllies && rnd.NextDouble() > b2.attackAlly || !areAllies && rnd.NextDouble() > b2.attackEnemy;
        bool b1Heal = areAllies && rnd.NextDouble() > b1.healAlly || !areAllies && rnd.NextDouble() > b1.healEnemy;
        bool b2Heal = areAllies && rnd.NextDouble() > b2.healAlly || !areAllies && rnd.NextDouble() > b2.healEnemy;

        if (b1Attack && b2Attack)
        {
            b1.getAttacked(rnd.Next(0, 100));
            b2.getAttacked(rnd.Next(0, 100));
            if (areAllies)
            {
                numberOfAlliesAttacked += 2;
            } else
            {
                numberOfEnemiesAttacked += 2;
            }
        } else
        if (b1Attack)
        {
            b2.getAttacked(rnd.Next(30, 100));
            b1.getAttacked(rnd.Next(0, 70));
            if (areAllies)
            {
                numberOfAlliesAttacked++;
            } else
            {
                numberOfEnemiesAttacked++;
            }
        } else
        if (b2Attack)
        {
            b1.getAttacked(rnd.Next(30, 100));
            b2.getAttacked(rnd.Next(0, 70));
            if (areAllies)
            {
                numberOfAlliesAttacked++;
            } else
            {
                numberOfEnemiesAttacked++;
            }
        } else
        {
            if (b1Heal)
            {
                b2.getHealed();
                if (areAllies)
                {
                    numberOfAlliesHealed++;
                } else
                {
                    numberOfEnemiesHealed++;
                }
            }
            if (b2Heal)
            {
                b1.getHealed();
                if (areAllies)
                {
                    numberOfAlliesHealed++;
                } else
                {
                    numberOfEnemiesHealed++;
                }
            } else
            {
                numberOfPassiveInteractions++;
            }
        }
        if (b1.health <= 0) numberOfKilled++;
        if (b2.health <= 0) numberOfKilled++;
    }

    private int numberOfKilled = 0;

    private void BlobCollectMaterial()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                if (MaterialGrid[i, j])
                {
                    foreach (Blob blob in BlobGrid[i, j])
                    {
                        Teams[blob.teamNumber].Construction += 1 / BlobGrid[i, j].Count;
                    }
                    MaterialGrid[i, j] = false;
                }
            }
        }
    }

    private void GoBackHome()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                BlobGrid[i, j].Clear();
            }
        }
        foreach (BlobTeam team in Teams)
        {
            team.RemoveDeadBlobs();
        }
    }

    private void EndOfDay()
    {
        GoBackHome();
        foreach (BlobTeam team in Teams)
        {
            team.RemoveDeadBlobs();
            team.CreateBlobs();
            foreach (Blob blob in team.Blobs)
            {
                if (blob.health <= 90) blob.health += 10;
            }
        }
        MaterialGrowth();
    }

    private void MaterialGrowth()
    {
        Random rnd = new Random();
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                MaterialGrid[i, j] = MaterialGrid[i, j] ? true : rnd.NextDouble() > MaterialSpawnProb;
            }
        }
    }

    public void SimulationStep(int numberOfHours)
    {
        for (int i = 0; i < numberOfHours; i++)
        {
            MoveBlobsOnGrid();
            PerformAttacks();
            foreach (BlobTeam team in Teams)
                team.RemoveDeadBlobs();
            BlobCollectMaterial();
            GoBackHome();
        }
        EndOfDay();

    }

    public void RunSimulation(int nbSteps, bool verbose = false)
    {
        Console.WriteLine("Initial state : ");
        Console.WriteLine(Stats());
        Console.WriteLine(TeamsStats());
        for (int i = 0; i < nbSteps; i++)
        {
            if (verbose) Console.WriteLine($"Step {i} : ");
            SimulationStep(20);
            if (verbose) Console.WriteLine(Stats());
        }
        Console.WriteLine("Final state : ");
        SimulationStep(5);
        Console.WriteLine(Stats());
        Console.WriteLine(TeamsStats());

        Console.WriteLine($"Number of allies healed : {numberOfAlliesHealed}");
        Console.WriteLine($"Number of enemies healed : {numberOfEnemiesHealed}");
        Console.WriteLine($"Number of allies attacked : {numberOfAlliesAttacked}");
        Console.WriteLine($"Number of enemies attacked : {numberOfEnemiesAttacked}");
        Console.WriteLine($"Number of passive interactions : {numberOfPassiveInteractions}");
        Console.WriteLine($"Number of killed blobs : {numberOfKilled}");
    }
}

