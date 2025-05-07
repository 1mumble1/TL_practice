using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.Application.Factory;

public class FightersFactory : AbstractFactory
{
    public override IFighter CreateFighter()
    {
        Console.WriteLine( "Введите имя для бойца:" );
        string? name;
        while ( ( name = Console.ReadLine() ) is null )
        {
            Console.WriteLine( "Невалидное имя для бойца, попробуйте еще раз" );
        }

        IRace race = ChooseRace();
        IClass fighterClass = ChooseClass();
        IWeapon weapon = ChooseWeapon();
        IArmor armor = ChooseArmor();

        IFighter newFighter = new Fighter( name, race, fighterClass, weapon, armor );

        Console.WriteLine(
$@"Боец '{name}' был успешно добавлен!
Характеристики:
    здоровье: {newFighter.MaxHealth}
    броня: {newFighter.MaxArmor}
    урон: {newFighter.Damage}
    скилл: {newFighter.Skill}"
            );

        Console.WriteLine();

        return newFighter;
    }

    private static IRace ChooseRace()
    {
        Console.WriteLine(
@"Выберите расу из списка:
    1 - Человек
    2 - Эльф
    3 - Ведьмак"
            );

        return GetOption( 1, 3 ) switch
        {
            1 => new Human(),
            2 => new Elf(),
            3 => new Witcher(),
            _ => new Human()
        };
    }

    private static IClass ChooseClass()
    {
        Console.WriteLine(
@"Выберите класс из списка:
    1 - Воин
    2 - Рыцарь
    3 - Убийца"
            );

        return GetOption( 1, 3 ) switch
        {
            1 => new Warrior(),
            2 => new Knight(),
            3 => new Assassin(),
            _ => new Warrior()
        };
    }

    private static IWeapon ChooseWeapon()
    {
        Console.WriteLine(
@"Выберите оружие из списка:
    1 - Меч
    2 - Нож
    3 - Молот
    4 - Без оружия"
            );

        return GetOption( 1, 4 ) switch
        {
            1 => new Sword(),
            2 => new Knife(),
            3 => new Hammer(),
            4 => new NoWeapon(),
            _ => new NoWeapon()
        };
    }

    private static IArmor ChooseArmor()
    {
        Console.WriteLine(
@"Выберите броню из списка:
    1 - Нагрудник
    2 - Поножи
    3 - Шлем
    4 - Без брони"
            );

        return GetOption( 1, 4 ) switch
        {
            1 => new Breastplate(),
            2 => new Greaves(),
            3 => new Helmet(),
            4 => new NoArmor(),
            _ => new NoArmor()
        };
    }

    private static int GetOption( int minValue, int maxValue )
    {
        while ( true )
        {
            if ( !int.TryParse( Console.ReadLine(), out int choice ) || choice < minValue || choice > maxValue )
            {
                Console.WriteLine( $"Необходимо ввести число от {minValue} до {maxValue}!" );
                continue;
            }
            return choice;
        }
    }
}
