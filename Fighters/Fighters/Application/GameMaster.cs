using Fighters.Models.Fighters;

namespace Fighters.Application;

public class GameMaster
{
    public IFighter PlayAndGetWinner( IReadOnlyList<IFighter> fighters )
    {
        List<IFighter> fightersBySkillDescending = fighters.OrderByDescending( fighter => fighter.Skill ).ToList();

        int round = 1;
        bool hadWinner = false;
        while ( !hadWinner )
        {
            Console.WriteLine( $"Раунд {round++}." );
            for ( int i = 0; i < fightersBySkillDescending.Count; i++ )
            {
                List<int> possibleDefenderFighterIndexes = [];
                for ( int j = 0; j < fightersBySkillDescending.Count; j++ )
                {
                    if ( j == i )
                    {
                        continue;
                    }
                    possibleDefenderFighterIndexes.Add( j );
                }
                int defenderIndex = possibleDefenderFighterIndexes[ Random.Shared.Next( 0, possibleDefenderFighterIndexes.Count ) ];

                IFighter attacker = fightersBySkillDescending[ i ];
                IFighter defender = fightersBySkillDescending[ defenderIndex ];

                Console.WriteLine( $"Боец {attacker.Name} атакует бойца {defender.Name}" );
                Fight( attacker, defender );

                if ( CheckDeath( defender ) )
                {
                    Console.WriteLine( $"Боец {defender.Name} убит!" );
                    fightersBySkillDescending.Remove( defender );
                    if ( fightersBySkillDescending.Count == 1 )
                    {
                        hadWinner = true;
                    }
                }
            }

            Console.WriteLine();
        }
        return fightersBySkillDescending[ 0 ];
    }

    private void Fight( IFighter roundOwner, IFighter opponent )
    {
        int damage = roundOwner.CalculateDamage();
        opponent.TakeDamage( damage );

        Console.WriteLine(
            $"Боец {opponent.Name} получает {damage} урона. " +
            opponent.ToString() );
    }

    private bool CheckDeath( IFighter fighter )
    {
        return fighter.CurrentHealth < 1;
    }
}