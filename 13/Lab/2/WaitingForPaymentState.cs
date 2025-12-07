using System;

namespace DesignPatternsLab13.Task2
{
    public class WaitingForPaymentState : IBookingRequestState
    {
        public void SendToClient(BookingRequestContext context)
        {
            Console.WriteLine("Заявка отправлена клиенту.");
        }

        public void MakePayment(BookingRequestContext context)
        {
            // Проверка срока оплаты
            if (context.IsPaymentExpired())
            {
                Console.WriteLine("Срок оплаты истек! Заявка автоматически отменена.");
                context.AddToHistory("Срок оплаты истек. Автоматическая отмена");
                context.SetState(new CancelledState());
                return;
            }

            Console.WriteLine($"Оплата получена: {context.TicketPrice:C}");
            Console.WriteLine($"Билет оплачен клиентом {context.ClientName}");
            context.AddToHistory($"Оплата получена: {context.TicketPrice:C}");
            context.SetState(new PaidState());
        }

        public void ConfirmBooking(BookingRequestContext context)
        {
            Console.WriteLine("Невозможно подтвердить. Сначала необходимо получить оплату.");
        }

        public void CancelRequest(BookingRequestContext context)
        {
            Console.WriteLine($"Заявка {context.RequestId} отменена.");
            Console.WriteLine("Клиент не успел оплатить в установленный срок.");
            context.AddToHistory("Заявка отменена - клиент не оплатил");
            context.SetState(new CancelledState());
        }

        public string GetStateName()
        {
            return "Ожидает оплаты (WaitingForPayment)";
        }

        public void PrintAvailableActions()
        {
            Console.WriteLine("\n✨ Доступные действия:");
            Console.WriteLine("  → Произвести оплату (MakePayment)");
            Console.WriteLine("  → Отменить заявку (CancelRequest)");
        }
    }
}