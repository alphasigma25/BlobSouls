namespace BlobSouls;
internal class Soul
{
    public float Coef = 0;

    public Soul(float soulCoef) => Coef = soulCoef switch
    {
        > 1 => 1,
        < -1 => -1,
        _ => soulCoef,
    };

    public float getHeal => Coef switch
    {
        < 0.1f => 0,
        _ => Coef
    };
    public float getAttack => Coef switch
    {
        > -0.1f => 0,
        _ => -Coef,
    };
}

