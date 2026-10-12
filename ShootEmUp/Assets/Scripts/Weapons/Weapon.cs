public class Weapon
{
    private SOWeapon weaponSpecs;

    public int CurrentLevel { get; private set; } = 1;

    public Weapon(SOWeapon specs)
    {
        weaponSpecs = specs;
    }

    public int Damage
    {
        get { return weaponSpecs.levels[CurrentLevel - 1].damage; }
    }

    public void IncreaseLevel()
    {
        if (CurrentLevel < weaponSpecs.levels.Length)
        {
            CurrentLevel++;
        }
    }

    public void ResetLevel()
    {
        CurrentLevel = 1;
    }
}