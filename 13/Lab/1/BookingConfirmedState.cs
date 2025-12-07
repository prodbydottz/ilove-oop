using System;

namespace DesignPatternsLab13.Task1
{
    /// <summary>
    /// Состояние: BookingConfirmed - бронирование подтверждено, но не оплачено
    /// </summary>
    public class BookingConfirmedState : IBookingState
    {
        public void SelectRoom(HotelBookingContext context, string roomNumber)
        {
            Console.WriteLine("❌ Невозможно выбрать другой номер. Бронирование уже подтверждено.");
            Console.WriteLine("   Отмените текущее бронирование, чтобы выбрать другой номер.");
        }

        public void ConfirmBooking(HotelBookingContext context)
        {
            Console.WriteLine("ℹ️ Бронирование уже подтверждено.");
        }

        public void MakePayment(HotelBookingContext context, decimal amount)
        {
            decimal finalAmount = context.CalculateFinalAmount();
            
            if (amount < finalAmount)
            {
                Console.WriteLine($"❌ Недостаточная сумма. Требуется: {finalAmount:C}, получено: {amount:C}");
                return;
            }

            Console.WriteLine($"✅ Оплата успешно проведена: {finalAmount:C}");
            
            if (context.Discount > 0)
            {
                Console.WriteLine($"🎁 Применена скидка: {context.Discount * 100}%");
            }
            
            if (amount > finalAmount)
            {
                Console.WriteLine($"💵 Сдача: {(amount - finalAmount):C}");
            }

            context.AddToHistory($"Оплата выполнена: {finalAmount:C}");
            context.SetState(new PaidState());
        }

        public void CancelBooking(HotelBookingContext context)
        {
            Console.WriteLine($"❌ Бронирование номера {context.RoomNumber} отменено.");
            context.AddToHistory($"Бронирование отменено после подтверждения (до оплаты)");
            context.RoomNumber = null;
            context.TotalAmount = 0;
            context.Discount = 0;
            context.SetState(new BookingCancelledState());
        }

        public void ChangeRoom(HotelBookingContext context, string newRoomNumber)
        {
            Console.WriteLine("❌ Невозможно изменить номер после подтверждения бронирования.");
            Console.WriteLine("   Отмените бронирование и начните заново.");
        }

        public string GetStateName()
        {
            return "Бронирование подтверждено (BookingConfirmed)";
        }

        public void PrintAvailableActions()
        {
            Console.WriteLine("\n✨ Доступные действия:");
            Console.WriteLine("  → Произвести оплату (MakePayment)");
            Console.WriteLine("  → Отменить бронирование (CancelBooking)");
        }
    }
}

