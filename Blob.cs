namespace BlobSouls;

internal class Blob
{
    public float healEnemy { get; }
    public float healAlly { get; }
    public float attackEnemy { get; }
    public float attackAlly { get; }
    public int teamNumber { get; }

    public int health { get; set; }

    public Soul Soul { get; private set; }

    public Blob(Soul soul, float groupHelper, int teamNumber)
    {
        Soul = soul;
        healEnemy = soul.getHeal + groupHelper;
        healEnemy = healEnemy < 0 ? 0 : healEnemy;

        healAlly = soul.getHeal + groupHelper;
        healAlly = healAlly > 1 ? 1 : healAlly;

        attackEnemy = soul.getAttack + groupHelper;
        attackEnemy = attackEnemy > 1 ? 1 : attackEnemy;

        attackAlly = soul.getAttack + groupHelper;
        attackAlly = attackAlly < 0 ? 0 : attackAlly;

        health = 100;
        this.teamNumber = teamNumber;
    }

    public void getAttacked(int lifeLoss)
    {
        health -= lifeLoss;
    }
    public void getHealed()
    {
        health = 100;
    }
}