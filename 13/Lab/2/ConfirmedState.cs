using System;

namespace DesignPatternsLab13.Task2
{
    /// <summary>
    /// Состояние: Confirmed - Подтверждена
    /// </summary>
    public class ConfirmedState : IBookingRequestState
    {
        public void SendToClient(BookingRequestContext context)
        {
            Console.WriteLine("ℹ️ Бронирование уже подтверждено.");
        }

        public void MakePayment(BookingRequestContext context)
        {
            Console.WriteLine("ℹ️ Оплата уже получена.");
        }

        public void ConfirmBooking(BookingRequestContext context)
        {
            Console.WriteLine("ℹ️ Бронирование уже подтверждено.");
        }

        public void CancelRequest(BookingRequestContext context)
        {
            Console.WriteLine("❌ Невозможно отменить подтвержденное бронирование.");
            Console.WriteLine("   Обратитесь к администратору.");
        }

        public string GetStateName()
        {
            return "Подтверждена (Confirmed)";
        }

        public void PrintAvailableActions()
        {
            Console.WriteLine("\n✨ Бронирование успешно завершено!");
            Console.WriteLine($"   Билет подтвержден и готов к использованию.");
        }
    }
}

