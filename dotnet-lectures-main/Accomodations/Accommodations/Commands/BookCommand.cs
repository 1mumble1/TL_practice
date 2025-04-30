using Accommodations.Dto;
using Accommodations.Models;

namespace Accommodations.Commands;

public class BookCommand(IBookingService bookingService, BookingDto bookingDto) : ICommand
{
    private Booking? _executedBookingDto;

    public void Execute()
    {
        Currency currency = bookingDto.Currency switch
        {
            CurrencyDto.Usd => Currency.Usd,
            CurrencyDto.Rub => Currency.Rub,
            CurrencyDto.Cny => Currency.Cny,
            _ => throw new ArgumentOutOfRangeException()
        };
        _executedBookingDto = bookingService.Book(bookingDto.UserId, bookingDto.Category, bookingDto.StartDate,
            bookingDto.EndDate, currency);
        Console.WriteLine($"Booking successful: ID {_executedBookingDto!.Id}");
    }

    public void Undo()
    {
        // добавлена проверка на null, при попытке отменить бронь, которая null, выбрасывается исключение, на верхнем уровне перехватится с помощью try catch
        if (_executedBookingDto is null)
        {
            throw new ArgumentNullException($"Booking is undefined, failed to cancel");
        }
        bookingService.CancelBooking(_executedBookingDto.Id);
        decimal cancellationPenalty = bookingService.CalculateCancellationPenaltyAmount(_executedBookingDto);
        Console.WriteLine(
            $"Booking {_executedBookingDto.Id} was canceled. Cancellation penalty: {cancellationPenalty}");
    }
}
