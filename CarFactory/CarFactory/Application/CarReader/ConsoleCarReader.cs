using CarFactory.Models.BodyShapes;
using CarFactory.Models.Colors;
using CarFactory.Models.Engines;
using CarFactory.Models.Transmissions;

namespace CarFactory.Application.CarReader;

public class ConsoleCarReader : ICarReader
{
    public string ReadCarName()
    {
        Console.Write( "Введите название вашего автомобиля: " );
        return ReadString();
    }

    public ITransmission ReadTransmission()
    {
        Console.WriteLine(
@"Выберите тип трансмиссии: 
    1 - Механический
    2 - Автоматический
    3 - Роботизированный"
);

        return ReadOption( 1, 4 ) switch
        {
            1 => new Mechanical(),
            2 => new Automatic(),
            3 => new Robotic(),
            _ => new Mechanical(),
        };
    }

    public IEngine ReadEngine()
    {
        Console.WriteLine(
@"Выберите тип двигателя: 
    1 - V6
    2 - V8
    3 - V12
    4 - Электрический"
);

        return ReadOption( 1, 4 ) switch
        {
            1 => new V6(),
            2 => new V8(),
            3 => new V12(),
            4 => new Electric(),
            _ => new V6(),
        };
    }

    public IColor ReadColor()
    {
        Console.WriteLine(
@"Выберите цвет кузова: 
    1 - Черный
    2 - Синий
    3 - Красный
    4 - Серебристый
    5 - Белый"
);

        return ReadOption( 1, 5 ) switch
        {
            1 => new Black(),
            2 => new Blue(),
            3 => new Red(),
            4 => new Silver(),
            5 => new White(),
            _ => new Black(),
        };
    }

    public IBodyShape ReadBodyShape()
    {
        Console.WriteLine(
@"Выберите форму кузова: 
    1 - Купе
    2 - Хэтчбэк
    3 - Лифтбэк
    4 - Седан"
);

        return ReadOption( 1, 4 ) switch
        {
            1 => new Coupe(),
            2 => new Hatchback(),
            3 => new Liftback(),
            4 => new Sedan(),
            _ => new Coupe(),
        };
    }

    private int ReadOption( int minValue, int maxValue )
    {
        int choice;
        while ( !int.TryParse( Console.ReadLine(), out choice ) || choice < minValue || choice > maxValue )
        {
            Console.WriteLine( $"Необходимо ввести число от {minValue} до {maxValue}!" );
        }

        return choice;
    }

    private static string ReadString()
    {
        string? str = Console.ReadLine();
        while ( string.IsNullOrWhiteSpace( str ) )
        {
            Console.WriteLine( "Данное поле не может быть пустым! Попробуйте еще раз" );
            str = Console.ReadLine();
        }
        return str;
    }
}
