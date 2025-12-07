using System;

namespace DesignPatternsLab13.Task1
{
    /// <summary>
    /// Состояние: RoomSelected - номер выбран, но не подтвержден
    /// </summary>
    public class RoomSelectedState : IBookingState
    {
        public void SelectRoom(HotelBookingContext context, string roomNumber)
        {
            Console.WriteLine($"ℹ️ Номер уже выбран. Используйте ChangeRoom для изменения.");
        }

        public void ConfirmBooking(HotelBookingContext context)
        {
            Console.WriteLine($"✅ Бронирование номера {context.RoomNumber} подтверждено!");
            
            // Генерация стоимости на основе номера
            Random rnd = new Random();
            context.TotalAmount = rnd.Next(3000, 10000);
            
            context.AddToHistory($"Бронирование подтверждено. Сумма: {context.TotalAmount:C}");
            context.SetState(new BookingConfirmedState());
        }

        public void MakePayment(HotelBookingContext context, decimal amount)
        {
            Console.WriteLine("❌ Невозможно произвести оплату. Сначала подтвердите бронирование.");
        }

        public void CancelBooking(HotelBookingContext context)
        {
            Console.WriteLine($"❌ Бронирование номера {context.RoomNumber} отменено.");
            context.AddToHistory($"Бронирование отменено на этапе выбора номера");
            context.RoomNumber = null;
            context.SetState(new BookingCancelledState());
        }

        public void ChangeRoom(HotelBookingContext context, string newRoomNumber)
        {
            Console.WriteLine($"🔄 Номер изменен с {context.RoomNumber} на {newRoomNumber}");
            context.AddToHistory($"Номер изменен: {context.RoomNumber} → {newRoomNumber}");
            context.RoomNumber = newRoomNumber;
        }

        public string GetStateName()
        {
            return "Номер выбран (RoomSelected)";
        }

        public void PrintAvailableActions()
        {
            Console.WriteLine("\n✨ Доступные действия:");
            Console.WriteLine("  → Подтвердить бронирование (ConfirmBooking)");
            Console.WriteLine("  → Изменить номер (ChangeRoom)");
            Console.WriteLine("  → Отменить бронирование (CancelBooking)");
        }
    }
}

