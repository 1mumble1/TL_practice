using Fighters.Models.Fighters;

namespace Fighters.Utils;

public class CommandHandler
{
    private const string AddFighter = "add-fighter";
    private const string Play = "play";
    private readonly List<IFighter> _fighters = [];

    public IReadOnlyList<IFighter> GetFighters()
    {
        while ( true )
        {
            Console.WriteLine( "Введите команду:" );
            Console.WriteLine( "add-fighter - Добавить нового бойца на арену" );
            Console.WriteLine( "play - Начать битву" );
            string? commandInput = Console.ReadLine();
            if ( commandInput is null || commandInput != AddFighter && commandInput != Play )
            {
                Console.WriteLine( "Неизвестная команда, попробуйте еще раз" );
                continue;
            }

            if ( commandInput == AddFighter )
            {
                IFighter fighter = FightersCreator.CreateFighter();
                _fighters.Add( fighter );
                continue;
            }

            if ( commandInput == Play )
            {
                if ( _fighters.Count < 2 )
                {
                    Console.WriteLine( "Вы создали недостаточно бойцов для игры!" );
                    continue;
                }
                else
                {
                    break;
                }
            }
        }

        return _fighters;
    }
}
