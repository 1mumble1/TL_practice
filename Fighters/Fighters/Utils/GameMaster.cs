using Fighters.Models.Fighters;

namespace Fighters.Utils;

public class GameMaster
{
    public IFighter PlayAndGetWinner( IReadOnlyList<IFighter> fighters )
    {
        List<IFighter> sortedFighters = fighters.OrderByDescending( fighter => fighter.Skill ).ToList();

        int round = 1;
        while ( true )
        {
            Console.WriteLine( $"Раунд {round++}." );
            for ( int i = 0; i < sortedFighters.Count; i++ )
            {
                List<int> indexesOfAliveFighters = [];
                for ( int j = 0; j < sortedFighters.Count; j++ )
                {
                    if ( j == i )
                    {
                        continue;
                    }
                    indexesOfAliveFighters.Add( j );
                }
                int defenderIndex = indexesOfAliveFighters[ Random.Shared.Next( 0, indexesOfAliveFighters.Count ) ];

                IFighter attacker = sortedFighters[ i ];
                IFighter defender = sortedFighters[ defenderIndex ];

                Console.WriteLine( $"Боец {attacker.Name} атакует бойца {defender.Name}" );

                if ( FightAndCheckIfOpponentDead( attacker, defender ) )
                {
                    Console.WriteLine( $"Боец {defender.Name} убит!" );
                    sortedFighters.Remove( defender );
                    if ( sortedFighters.Count == 1 )
                        return attacker;
                }
            }

            Console.WriteLine();
        }
    }

    private bool FightAndCheckIfOpponentDead( IFighter roundOwner, IFighter opponent )
    {
        int damage = roundOwner.CalculateDamage();
        opponent.TakeDamage( damage );

        Console.WriteLine(
            $"Боец {opponent.Name} получает {damage} урона. " +
            opponent.ToString() );

        return opponent.CurrentHealth < 1;
    }
}