namespace BlobSouls;

internal class Soul
{
    public Soul(float soulCoef)
    {
        Coef = soulCoef switch
        {
            > 1 => 1,
            < -1 => -1,
            _ => soulCoef,
        };
    }

    public float Coef { get; }

    public float GetHeal => Coef switch
    {
        < 0.1f => 0,
        _ => Coef
    };

    public float GetAttack => Coef switch
    {
        > -0.1f => 0,
        _ => -Coef,
    };
}