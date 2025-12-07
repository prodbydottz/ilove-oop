using System;
using System.Collections.Generic;

namespace DesignPatternsLab13.Task1
{
    /// <summary>
    /// Контекст бронирования гостиницы
    /// </summary>
    public class HotelBookingContext
    {
        private IBookingState _currentState;
        private string _roomNumber;
        private decimal _totalAmount;
        private decimal _discount;
        private List<string> _bookingHistory;

        public HotelBookingContext()
        {
            _currentState = new IdleState();
            _bookingHistory = new List<string>();
            AddToHistory("Система инициализирована");
        }

        public void SetState(IBookingState state)
        {
            _currentState = state;
            AddToHistory($"Переход в состояние: {state.GetStateName()}");
        }

        public string RoomNumber 
        { 
            get => _roomNumber; 
            set => _roomNumber = value; 
        }

        public decimal TotalAmount 
        { 
            get => _totalAmount; 
            set => _totalAmount = value; 
        }

        public decimal Discount 
        { 
            get => _discount; 
            set => _discount = value; 
        }

        public void SelectRoom(string roomNumber)
        {
            _currentState.SelectRoom(this, roomNumber);
        }

        public void ConfirmBooking()
        {
            _currentState.ConfirmBooking(this);
        }

        public void MakePayment(decimal amount)
        {
            _currentState.MakePayment(this, amount);
        }

        public void CancelBooking()
        {
            _currentState.CancelBooking(this);
        }

        public void ChangeRoom(string newRoomNumber)
        {
            _currentState.ChangeRoom(this, newRoomNumber);
        }

        public void AddToHistory(string action)
        {
            _bookingHistory.Add($"[{DateTime.Now:HH:mm:ss}] {action}");
        }

        public void ShowHistory()
        {
            Console.WriteLine("\n📜 История бронирования:");
            Console.WriteLine("════════════════════════════════════════");
            foreach (var entry in _bookingHistory)
            {
                Console.WriteLine(entry);
            }
            Console.WriteLine("════════════════════════════════════════");
        }

        public void ShowCurrentState()
        {
            Console.WriteLine($"\n📊 Текущее состояние: {_currentState.GetStateName()}");
            if (!string.IsNullOrEmpty(_roomNumber))
            {
                Console.WriteLine($"🏨 Номер: {_roomNumber}");
            }
            if (_totalAmount > 0)
            {
                Console.WriteLine($"💰 Сумма: {_totalAmount:C}");
                if (_discount > 0)
                {
                    decimal finalAmount = _totalAmount * (1 - _discount);
                    Console.WriteLine($"🎁 Скидка: {_discount * 100}%");
                    Console.WriteLine($"💵 К оплате: {finalAmount:C}");
                }
            }
            _currentState.PrintAvailableActions();
        }

        public decimal CalculateFinalAmount()
        {
            return _totalAmount * (1 - _discount);
        }
    }
}

