using Fighters.Models.Fighters;
using Fighters.Models.Armors;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Fighters.Models.Classes;

namespace Fighters.Utils;

public class FightersCreator
{
    public static IFighter CreateFighter()
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

        Console.WriteLine( $"Боец '{name}' был успешно добавлен!" );
        Console.WriteLine( "Характеристики:" );
        Console.WriteLine( $"     здоровье: {newFighter.MaxHealth}" );
        Console.WriteLine( $"     броня: {newFighter.MaxArmor}" );
        Console.WriteLine( $"     урон: {newFighter.Damage}" );
        Console.WriteLine( $"     скилл: {newFighter.Skill}" );
        Console.WriteLine();

        return newFighter;
    }

    private static IRace ChooseRace()
    {
        Console.WriteLine( "Выберите расу из списка:\n" +
            "1 - Человек\n" +
            "2 - Эльф\n" +
            "3 - Ведьмак" );

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
        Console.WriteLine( "Выберите класс из списка:\n" +
            "1 - Воин\n" +
            "2 - Рыцарь\n" +
            "3 - Убийца" );

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
        Console.WriteLine( "Выберите оружие из списка:\n" +
            "1 - Меч\n" +
            "2 - Нож\n" +
            "3 - Молот\n" +
            "4 - Без оружия" );

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
        Console.WriteLine( "Выберите броню из списка:\n" +
            "1 - Нагрудник\n" +
            "2 - Поножи\n" +
            "3 - Шлем\n" +
            "4 - Без брони" );

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