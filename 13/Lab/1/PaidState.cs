using System;

namespace DesignPatternsLab13.Task1
{
    /// <summary>
    /// Состояние: Paid - бронирование оплачено, номер закреплен за пользователем
    /// </summary>
    public class PaidState : IBookingState
    {
        public void SelectRoom(HotelBookingContext context, string roomNumber)
        {
            Console.WriteLine("❌ Невозможно выбрать номер. Бронирование уже оплачено.");
        }

        public void ConfirmBooking(HotelBookingContext context)
        {
            Console.WriteLine("ℹ️ Бронирование уже подтверждено и оплачено.");
        }

        public void MakePayment(HotelBookingContext context, decimal amount)
        {
            Console.WriteLine("ℹ️ Бронирование уже оплачено.");
        }

        public void CancelBooking(HotelBookingContext context)
        {
            Console.WriteLine("❌ Невозможно отменить бронирование после оплаты.");
            Console.WriteLine("   Свяжитесь с администрацией для возврата средств.");
        }

        public void ChangeRoom(HotelBookingContext context, string newRoomNumber)
        {
            Console.WriteLine("❌ Невозможно изменить номер после оплаты.");
        }

        public string GetStateName()
        {
            return "Оплачено - Бронирование завершено (Paid)";
        }

        public void PrintAvailableActions()
        {
            Console.WriteLine("\n✨ Бронирование успешно завершено!");
            Console.WriteLine($"📋 Номер {0} закреплен за вами.");
            Console.WriteLine("   Для отмены обратитесь к администрации.");
        }
    }
}

