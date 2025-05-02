using System.Threading.Channels;

internal class Program
{
    private static void Main( string[] args )
    {
        Console.WriteLine( "Добро пожаловать на фабрику машин!" );
        Console.WriteLine( "Давайте соберем вашу первую машину?" );
        CarFactory.Factories.CarFactory factory = new();

        bool isCarFactoryFinished = false;
        while ( !isCarFactoryFinished )
        {
            CarFactory.Cars.ICar car = factory.CreateCar();

            Console.WriteLine( "Ваша машина готова!" );
            Console.WriteLine( car.GetDescription() );

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
                isCarFactoryFinished = true;
            }
        }
    }
}