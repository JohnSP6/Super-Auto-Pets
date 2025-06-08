//Creates objects known as animals
public class Animals
{
    public string animalName { get; set; }
    public int animalHealth { get; set; }
    public int animalDmg { get; set; }

    public Animals(string animName, int animHealth, int animDmg)
    {
        this.animalName = animName;
        this.animalHealth = animHealth;
        this.animalDmg = animDmg;
    }
}