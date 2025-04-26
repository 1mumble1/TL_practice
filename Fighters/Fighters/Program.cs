using Fighters.Models.Fighters;
using Fighters.Utils;

namespace Fighters;

public class Program
{
    public static void Main()
    {
        Console.WriteLine( "Добро пожаловать в игру Fighters!" );

        CommandHandler handler = new();
        GameMaster master = new();
        IFighter winner = master.PlayAndGetWinner( handler.GetFighters() );
        Console.WriteLine( $"Выигрывает {winner.Name}!" );
    }
}