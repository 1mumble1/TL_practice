using Fighters.Application;
using Fighters.Application.Factory;
using Fighters.Models.Fighters;

namespace Fighters;

public class Program
{
    public static void Main()
    {
        Console.WriteLine( "Добро пожаловать в игру Fighters!" );

        IFactory factory = new FightersFactory();
        FightersReader reader = new( factory );
        GameMaster master = new();
        IFighter winner = master.PlayAndGetWinner( reader.ReadFighters() );
        Console.WriteLine( $"Выигрывает {winner.Name}!" );
    }
}