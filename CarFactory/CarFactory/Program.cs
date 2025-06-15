using CarFactory.Application.CarReader;
using CarFactory.Models.Cars;

internal class Program
{
    private static void Main( string[] args )
    {
        Console.WriteLine( "Добро пожаловать на фабрику машин!" );
        Console.WriteLine( "Давайте соберем вашу первую машину?" );
        ICarReader carReader = new ConsoleCarReader();
        CarFactory.Application.Factories.CarFactory factory = new( carReader );

        bool isCarFactoryRunning = true;
        while ( isCarFactoryRunning )
        {
            ICar car = factory.CreateCar();

            Console.WriteLine( "Ваша машина готова!" );
            Console.WriteLine( car.ToString() );

            Console.WriteLine( "Хотите собрать еще одну машину? Нажмите Enter - чтобы продолжить, любую клавишу - чтобы выйти" );
            ConsoleKeyInfo userConfirmation = Console.ReadKey();
            Console.Clear();
            if ( userConfirmation.Key == ConsoleKey.Enter )
            {
                Console.WriteLine( "Хорошо, давайте соберем еще одну!" );
            }
            else
            {
                Console.WriteLine( "До свидания!" );
                isCarFactoryRunning = false;
            }
        }
    }
}