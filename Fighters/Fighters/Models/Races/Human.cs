namespace Fighters.Models.Races;

public class Human : IRace
{
    public int Damage => 11;

    public int Health => 100;

    public int Armor => 10;

    public int Skill => 20;
}