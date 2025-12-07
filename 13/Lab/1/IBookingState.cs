namespace DesignPatternsLab13.Task1
{
    /// <summary>
    /// Интерфейс состояния бронирования (паттерн State)
    /// </summary>
    public interface IBookingState
    {
        void SelectRoom(HotelBookingContext context, string roomNumber);
        void ConfirmBooking(HotelBookingContext context);
        void MakePayment(HotelBookingContext context, decimal amount);
        void CancelBooking(HotelBookingContext context);
        void ChangeRoom(HotelBookingContext context, string newRoomNumber);
        string GetStateName();
        void PrintAvailableActions();
    }
}

