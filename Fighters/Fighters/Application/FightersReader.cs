using Fighters.Application.Factory;
using Fighters.Models.Fighters;

namespace Fighters.Application;

public class FightersReader
{
    private const string AddFighter = "add-fighter";
    private const string End = "end";
    private readonly List<IFighter> _fighters = [];
    private readonly AbstractFactory _factory = new FightersFactory();

    public IReadOnlyList<IFighter> ReadFighters()
    {
        bool isFightersAdded = false;
        while ( !isFightersAdded )
        {
            Console.WriteLine( "Введите команду:" );
            Console.WriteLine( "add-fighter - Добавить нового бойца на арену" );
            Console.WriteLine( "end - Завершить добавление бойцов" );
            string? commandInput = Console.ReadLine();

            switch ( commandInput )
            {
                case AddFighter:
                    IFighter fighter = _factory.CreateFighter();
                    _fighters.Add( fighter );
                    break;

                case End:
                    isFightersAdded = !( _fighters.Count < 2 );
                    if ( !isFightersAdded )
                    {
                        Console.WriteLine( "Вы создали недостаточно бойцов для игры!" );
                    }
                    break;

                default:
                    Console.WriteLine( "Неизвестная команда, попробуйте еще раз" );
                    break;
            }
        }

        return _fighters;
    }
}
