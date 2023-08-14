namespace BlobSouls;

internal class Blob
{
    public float HealEnemy { get; }

    public float HealAlly { get; }

    public float AttackEnemy { get; }

    public float AttackAlly { get; }

    public int TeamNumber { get; }

    public string Stats =>
        $"healEnemy % {HealEnemy}\nhealAlly % {HealAlly}\nattackEnemy % {AttackEnemy}\nattackAlly % {AttackAlly}";

    public int Health { get; set; }

    public Soul Soul { get; }

    public Blob(Soul soul, float groupHelper, int teamNumber)
    {
        Soul = soul;
        HealEnemy = soul.GetHeal;
        HealEnemy = HealEnemy < 0 ? 0 : HealEnemy;

        HealAlly = soul.GetHeal + groupHelper;
        HealAlly = HealAlly > 1 ? 1 : HealAlly;

        AttackEnemy = soul.GetAttack + groupHelper;
        AttackEnemy = AttackEnemy > 1 ? 1 : AttackEnemy;

        AttackAlly = soul.GetAttack;
        AttackAlly = AttackAlly < 0 ? 0 : AttackAlly;

        Health = 100;
        TeamNumber = teamNumber;
    }

    public void GetAttacked(int lifeLoss) => Health -= lifeLoss;

    public void GetHealed() => Health = 100;
}