using CarFactory.BodyShapes;
using CarFactory.Cars;
using CarFactory.Colors;
using CarFactory.Engines;
using CarFactory.Transmissions;

namespace CarFactory.Factories;

public class CarFactory : AbstractFactory
{
    public override ICar CreateCar()
    {
        Console.Write( "Введите название вашего автомобиля: " );
        string name = GetString();

        IBodyShape bodyShape = ChooseBodyShape();
        IColor color = ChooseColor();
        IEngine engine = ChooseEngine();
        ITransmission transmission = ChooseTransmission();

        return new Car( name, bodyShape, color, engine, transmission );
    }

    private ITransmission ChooseTransmission()
    {
        Console.WriteLine(
@"Выберите тип трансмиссии: 
    1 - Механический
    2 - Автоматический
    3 - Роботизированный"
);

        return GetOption( 1, 4 ) switch
        {
            1 => new Mechanical(),
            2 => new Automatic(),
            3 => new Robotic(),
            _ => new Mechanical(),
        };
    }

    private IEngine ChooseEngine()
    {
        Console.WriteLine(
@"Выберите тип двигателя: 
    1 - V6
    2 - V8
    3 - V12
    4 - Электрический"
);

        return GetOption( 1, 4 ) switch
        {
            1 => new V6(),
            2 => new V8(),
            3 => new V12(),
            4 => new Electric(),
            _ => new V6(),
        };
    }

    private IColor ChooseColor()
    {
        Console.WriteLine(
@"Выберите цвет кузова: 
    1 - Черный
    2 - Синий
    3 - Красный
    4 - Серебристый
    5 - Белый"
    );

        return GetOption( 1, 5 ) switch
        {
            1 => new Black(),
            2 => new Blue(),
            3 => new Red(),
            4 => new Silver(),
            5 => new White(),
            _ => new Black(),
        };
    }

    private IBodyShape ChooseBodyShape()
    {
        Console.WriteLine(
@"Выберите форму кузова: 
    1 - Купе
    2 - Хэтчбэк
    3 - Лифтбэк
    4 - Седан"
            );

        return GetOption( 1, 4 ) switch
        {
            1 => new Coupe(),
            2 => new Hatchback(),
            3 => new Liftback(),
            4 => new Sedan(),
            _ => new Coupe(),
        };
    }

    private int GetOption( int minValue, int maxValue )
    {
        int choice;
        while ( !int.TryParse( Console.ReadLine(), out choice ) || choice < minValue || choice > maxValue )
        {
            Console.WriteLine( $"Необходимо ввести число от {minValue} до {maxValue}!" );
        }

        return choice;
    }

    private static string GetString()
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
