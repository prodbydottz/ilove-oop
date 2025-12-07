namespace DesignPatternsLab13.Task2
{
    /// <summary>
    /// Интерфейс состояния заявки на бронирование билетов (паттерн State)
    /// </summary>
    public interface IBookingRequestState
    {
        void SendToClient(BookingRequestContext context);
        void MakePayment(BookingRequestContext context);
        void ConfirmBooking(BookingRequestContext context);
        void CancelRequest(BookingRequestContext context);
        string GetStateName();
        void PrintAvailableActions();
    }
}

