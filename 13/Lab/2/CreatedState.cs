using System;

namespace DesignPatternsLab13.Task2
{
    /// <summary>
    /// Состояние: Created - Заявка только что создана
    /// </summary>
    public class CreatedState : IBookingRequestState
    {
        public void SendToClient(BookingRequestContext context)
        {
            Console.WriteLine($"✅ Заявка {context.RequestId} отправлена клиенту {context.ClientName}");
            
            // Устанавливаем срок оплаты (например, 5 минут)
            context.PaymentDeadline = DateTime.Now.AddMinutes(5);
            
            Console.WriteLine($"⏰ Срок оплаты установлен до: {context.PaymentDeadline:HH:mm:ss}");
            context.AddToHistory($"Заявка отправлена клиенту. Срок оплаты: 5 минут");
            context.SetState(new WaitingForPaymentState());
        }

        public void MakePayment(BookingRequestContext context)
        {
            Console.WriteLine("❌ Невозможно оплатить. Сначала отправьте заявку клиенту.");
        }

        public void ConfirmBooking(BookingRequestContext context)
        {
            Console.WriteLine("❌ Невозможно подтвердить. Заявка должна быть оплачена.");
        }

        public void CancelRequest(BookingRequestContext context)
        {
            Console.WriteLine($"❌ Заявка {context.RequestId} отменена на этапе создания.");
            context.AddToHistory("Заявка отменена после создания");
            context.SetState(new CancelledState());
        }

        public string GetStateName()
        {
            return "Создана (Created)";
        }

        public void PrintAvailableActions()
        {
            Console.WriteLine("\n✨ Доступные действия:");
            Console.WriteLine("  → Отправить клиенту (SendToClient)");
            Console.WriteLine("  → Отменить заявку (CancelRequest)");
        }
    }
}

