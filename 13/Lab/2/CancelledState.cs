using System;

namespace DesignPatternsLab13.Task2
{
    /// <summary>
    /// Состояние: Cancelled - Отменена (финальное состояние)
    /// </summary>
    public class CancelledState : IBookingRequestState
    {
        public void SendToClient(BookingRequestContext context)
        {
            Console.WriteLine("❌ Невозможно отправить отмененную заявку.");
        }

        public void MakePayment(BookingRequestContext context)
        {
            Console.WriteLine("❌ Невозможно оплатить отмененную заявку.");
        }

        public void ConfirmBooking(BookingRequestContext context)
        {
            Console.WriteLine("❌ Невозможно подтвердить отмененную заявку.");
        }

        public void CancelRequest(BookingRequestContext context)
        {
            Console.WriteLine("ℹ️ Заявка уже отменена.");
        }

        public string GetStateName()
        {
            return "Отменена (Cancelled) - Финальное состояние";
        }

        public void PrintAvailableActions()
        {
            Console.WriteLine("\n✨ Заявка отменена.");
            Console.WriteLine("   Создайте новую заявку для бронирования.");
        }
    }
}

