using System;

namespace DesignPatternsLab13.Task1
{
    /// <summary>
    /// Состояние: BookingCancelled - бронирование отменено
    /// </summary>
    public class BookingCancelledState : IBookingState
    {
        public void SelectRoom(HotelBookingContext context, string roomNumber)
        {
            Console.WriteLine("ℹ️ Бронирование было отменено. Начните новое бронирование.");
        }

        public void ConfirmBooking(HotelBookingContext context)
        {
            Console.WriteLine("❌ Невозможно подтвердить отмененное бронирование.");
        }

        public void MakePayment(HotelBookingContext context, decimal amount)
        {
            Console.WriteLine("❌ Невозможно произвести оплату отмененного бронирования.");
        }

        public void CancelBooking(HotelBookingContext context)
        {
            Console.WriteLine("ℹ️ Бронирование уже отменено.");
        }

        public void ChangeRoom(HotelBookingContext context, string newRoomNumber)
        {
            Console.WriteLine("❌ Невозможно изменить номер отмененного бронирования.");
        }

        public string GetStateName()
        {
            return "Бронирование отменено (BookingCancelled)";
        }

        public void PrintAvailableActions()
        {
            Console.WriteLine("\n✨ Бронирование отменено.");
            Console.WriteLine("   Начните новое бронирование, выбрав номер.");
        }
    }
}

