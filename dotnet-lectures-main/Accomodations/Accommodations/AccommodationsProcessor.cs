using Accommodations.Commands;
using Accommodations.Dto;

namespace Accommodations;

public static class AccommodationsProcessor
{
    private static BookingService _bookingService = new();
    private static Dictionary<int, ICommand> _executedCommands = new();
    // исправлено название свойства класса s_commandIndex -> _commandIndex
    private static int _commandIndex = 0;

    public static void Run()
    {
        Console.WriteLine("Booking Command Line Interface");
        Console.WriteLine("Commands:");
        Console.WriteLine("'book <UserId> <Category> <StartDate> <EndDate> <Currency>' - to book a room");
        Console.WriteLine("'cancel <BookingId>' - to cancel a booking");
        Console.WriteLine("'undo' - to undo the last command");
        Console.WriteLine("'find <BookingId>' - to find a booking by ID");
        Console.WriteLine("'search <StartDate> <EndDate> <CategoryName>' - to search bookings");
        Console.WriteLine("'exit' - to exit the application");

        // исправлена строка на nullable
        string? input;
        while ((input = Console.ReadLine()) != "exit")
        {
            try
            {
                ProcessCommand(input);
            }
            // исправлен catch, теперь будут абсолютно все исключения перехватываться
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private static void ProcessCommand(string? input)
    {
        // добавлена проверка на nullable string
        if (string.IsNullOrEmpty(input))
        {
            return;
        }

        string[] parts = input.Split(' ');
        string commandName = parts[0];

        switch (commandName)
        {
            case "book":
                if (parts.Length != 6)
                {
                    // вместо Console.WriteLine добавлен выброс исключения, который на верхнем уровне перехватится с помощью try catch
                    throw new ArgumentException("Invalid number of arguments for booking.\n" +
                        "Expected format: book <UserId> <Category> <StartDate> <EndDate> <Currency>.");
                }

                // проверка на валидный userId
                if (!int.TryParse(parts[1], out int userId))
                {
                    throw new ArgumentException("Invalid user id for booking. It must be a number.");
                }

                // проверка на валидную дату начала брони
                if (!DateTime.TryParse(parts[3], out DateTime startDate))
                {
                    throw new ArgumentException("Invalid start date for booking. Expected format: dd/mm/yyyy or dd.mm.yyyy.");
                }

                // проверка на валидную дату конца брони
                if (!DateTime.TryParse(parts[4], out DateTime endDate))
                {
                    throw new ArgumentException("Invalid end date for booking. Expected format: dd/mm/yyyy or dd.mm.yyyy.");
                }

                // проверка на валидную валюту
                if (!Enum.TryParse(parts[5], true, out CurrencyDto currency))
                {
                    throw new ArgumentException($"Invalid currency for booking. Expected format: {string.Join(", ", Enum.GetNames(typeof(CurrencyDto)))}.");
                }

                BookingDto bookingDto = new()
                {
                    UserId = userId,
                    Category = parts[2],
                    StartDate = startDate,
                    EndDate = endDate,
                    Currency = currency,
                };

                BookCommand bookCommand = new(_bookingService, bookingDto);
                bookCommand.Execute();
                _executedCommands.Add(++_commandIndex, bookCommand);
                Console.WriteLine("Booking command run is successful.");
                break;

            case "cancel":
                if (parts.Length != 2)
                {
                    // вместо Console.WriteLine добавлен выброс исключения, который на верхнем уровне перехватится с помощью try catch
                    throw new ArgumentException("Invalid number of arguments for canceling.\n" +
                        "Expected format: cancel <bookingId>");
                }

                // проверка guid на валидность
                if (!Guid.TryParse(parts[1], out Guid bookingId))
                {
                    throw new ArgumentException("Invalid booking id for cancelling.");
                }

                CancelBookingCommand cancelCommand = new(_bookingService, bookingId);
                cancelCommand.Execute();
                _executedCommands.Add(++_commandIndex, cancelCommand);
                Console.WriteLine("Cancellation command run is successful.");
                break;

            case "undo":
                // проверка на пустую историю команд
                if (_executedCommands.Count == 0)
                {
                    throw new IndexOutOfRangeException("Command History is empty, but tried to undo");
                }

                _executedCommands[_commandIndex].Undo();
                _executedCommands.Remove(_commandIndex);
                _commandIndex--;
                Console.WriteLine("Last command undone.");
                break;

            case "find":
                if (parts.Length != 2)
                {
                    // вместо Console.WriteLine добавлен выброс исключения, который на верхнем уровне перехватится с помощью try catch
                    throw new ArgumentException("Invalid arguments for 'find'. Expected format: 'find <BookingId>'");
                }

                // проверка guid на валидность
                if (!Guid.TryParse(parts[1], out Guid id))
                {
                    throw new ArgumentException("Invalid id for finding.");
                }

                FindBookingByIdCommand findCommand = new(_bookingService, id);
                findCommand.Execute();
                break;

            case "search":
                if (parts.Length != 4)
                {
                    // вместо Console.WriteLine добавлен выброс исключения, который на верхнем уровне перехватится с помощью try catch
                    throw new ArgumentException("Invalid arguments for 'search'. Expected format: 'search <StartDate> <EndDate> <CategoryName>'");
                }

                // проверка на валидную дату начала брони
                if (!DateTime.TryParse(parts[1], out startDate))
                {
                    throw new ArgumentException("Invalid start date of booking. Expected format: dd/mm/yyyy or dd.mm.yyyy.");
                }

                // проверка на валидную дату конца брони
                if (!DateTime.TryParse(parts[2], out endDate))
                {
                    throw new ArgumentException("Invalid end date of booking. Expected format: dd/mm/yyyy or dd.mm.yyyy.");
                }

                string categoryName = parts[3];
                SearchBookingsCommand searchCommand = new(_bookingService, startDate, endDate, categoryName);
                searchCommand.Execute();
                break;

            default:
                // вместо Console.WriteLine добавлен выброс исключения, который на верхнем уровне перехватится с помощью try catch
                throw new ArgumentException("Unknown command.");
        }
    }
}
