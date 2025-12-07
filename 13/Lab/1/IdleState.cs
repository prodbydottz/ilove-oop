using System;

namespace DesignPatternsLab13.Task1
{
    /// <summary>
    /// Состояние: Idle - система ожидает действия пользователя
    /// </summary>
    public class IdleState : IBookingState
    {
        public void SelectRoom(HotelBookingContext context, string roomNumber)
        {
            Console.WriteLine($"✅ Номер {roomNumber} выбран!");
            context.RoomNumber = roomNumber;
            context.AddToHistory($"Выбран номер: {roomNumber}");
            context.SetState(new RoomSelectedState());
        }

        public void ConfirmBooking(HotelBookingContext context)
        {
            Console.WriteLine("❌ Невозможно подтвердить бронирование. Сначала выберите номер.");
        }

        public void MakePayment(HotelBookingContext context, decimal amount)
        {
            Console.WriteLine("❌ Невозможно произвести оплату. Сначала выберите номер.");
        }

        public void CancelBooking(HotelBookingContext context)
        {
            Console.WriteLine("ℹ️ Нет активного бронирования для отмены.");
        }

        public void ChangeRoom(HotelBookingContext context, string newRoomNumber)
        {
            Console.WriteLine("❌ Невозможно изменить номер. Сначала выберите номер.");
        }

        public string GetStateName()
        {
            return "Ожидание действий (Idle)";
        }

        public void PrintAvailableActions()
        {
            Console.WriteLine("\n✨ Доступные действия:");
            Console.WriteLine("  → Выбрать номер (SelectRoom)");
        }
    }
}

