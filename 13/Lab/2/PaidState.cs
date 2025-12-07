using System;

namespace DesignPatternsLab13.Task2
{
    /// <summary>
    /// Состояние: Paid - Оплачена
    /// </summary>
    public class PaidState : IBookingRequestState
    {
        public void SendToClient(BookingRequestContext context)
        {
            Console.WriteLine("ℹ️ Заявка уже оплачена.");
        }

        public void MakePayment(BookingRequestContext context)
        {
            Console.WriteLine("ℹ️ Заявка уже оплачена.");
        }

        public void ConfirmBooking(BookingRequestContext context)
        {
            Console.WriteLine("✅ Бронирование подтверждено!");
            Console.WriteLine($"🎫 Билет для {context.ClientName} успешно забронирован");
            Console.WriteLine($"🆔 Номер бронирования: {context.RequestId}");
            context.AddToHistory("Бронирование подтверждено системой");
            context.SetState(new ConfirmedState());
        }

        public void CancelRequest(BookingRequestContext context)
        {
            Console.WriteLine("⚠️ Заявка уже оплачена.");
            Console.WriteLine("   Для отмены необходимо обратиться к администратору для возврата средств.");
        }

        public string GetStateName()
        {
            return "Оплачена (Paid)";
        }

        public void PrintAvailableActions()
        {
            Console.WriteLine("\n✨ Доступные действия:");
            Console.WriteLine("  → Подтвердить бронирование (ConfirmBooking)");
        }
    }
}

