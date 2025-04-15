const int DeliveryTimeInDays = 3;

static int GetInteger()
{
    int number;
    while ( !int.TryParse( Console.ReadLine(), out number ) )
    {
        Console.WriteLine( "Число не распознано. Введите пожалуйста число, например: 12" );
    }
    return number;
}

static string GetString()
{
    string? str = Console.ReadLine();
    while ( string.IsNullOrWhiteSpace( str ) )
    {
        Console.WriteLine( "Данное поле не может быть пустым!" );
        Console.Write( "Введите пожалуйста еще раз: " );
        str = Console.ReadLine();
    }
    return str;
}

static bool ConfirmOrderStatus( string product, int count, string name, string address )
{
    Console.WriteLine( $"Здравствуйте, {name}, вы заказали {count} {product} на адрес {address}, все верно?" );
    Console.WriteLine( "Нажмите Enter - если все правильно, любую другую клавишу - если есть ошибки в заказе и нужно его оформить заново" );

    ConsoleKeyInfo confirmationInput = Console.ReadKey();
    if ( confirmationInput.Key == ConsoleKey.Enter )
    {
        DateOnly deliveryDate = DateOnly.FromDateTime( DateTime.UtcNow ).AddDays( DeliveryTimeInDays );
        Console.WriteLine( $"{name}! Ваш заказ '{product}' в количестве {count} оформлен! Ожидайте доставку по адресу {address} к {deliveryDate}" );
        return true;
    }
    else
    {
        Console.Clear();
        Console.WriteLine( "Хорошо, давайте попробуем еще раз оформить заказ)" );
        return false;
    }
}

Console.WriteLine( "Здравствуйте! Данное приложение создано для оформления заказов." );
bool isOrderConfirmed = false;

while ( !isOrderConfirmed )
{
    Console.Write( "Введите пожалуйста название товара: " );
    string product = GetString();

    Console.Write( "Отлично! Теперь введите количество товара: " );
    int count = GetInteger();

    Console.Write( "Великолепно! Введите ваше имя для оформления заказа: " );
    string name = GetString();

    Console.Write( "Супер! Осталось только ввести адрес доставки вашего товара: " );
    string address = GetString();

    isOrderConfirmed = ConfirmOrderStatus( product, count, name, address );
}