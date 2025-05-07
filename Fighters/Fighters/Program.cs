using Fighters.Application;
using Fighters.Models.Fighters;

namespace Fighters;

public class Program
{
    public static void Main()
    {
        Console.WriteLine( "Добро пожаловать в игру Fighters!" );

        FightersReader reader = new();
        GameMaster master = new();
        IFighter winner = master.PlayAndGetWinner( reader.ReadFighters() );
        Console.WriteLine( $"Выигрывает {winner.Name}!" );
    }
}