namespace App;

public struct Markets
{
    public readonly string Marketname;
    public double CurrontPrice;

    public Markets(string name, double currontPrice = 0)
    {
        Marketname = name;
        CurrontPrice = currontPrice;
    }
}