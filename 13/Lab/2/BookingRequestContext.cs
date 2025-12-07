using System;
using System.Collections.Generic;

namespace DesignPatternsLab13.Task2
{
    /// <summary>
    /// Контекст заявки на бронирование билетов
    /// </summary>
    public class BookingRequestContext
    {
        private IBookingRequestState _currentState;
        private string _requestId;
        private string _clientName;
        private decimal _ticketPrice;
        private List<string> _stateHistory;
        private DateTime _createdAt;
        private DateTime? _paymentDeadline;

        public BookingRequestContext(string clientName, decimal ticketPrice)
        {
            _currentState = new CreatedState();
            _requestId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            _clientName = clientName;
            _ticketPrice = ticketPrice;
            _stateHistory = new List<string>();
            _createdAt = DateTime.Now;
            AddToHistory($"Заявка создана для клиента: {clientName}");
        }

        public void SetState(IBookingRequestState state)
        {
            _currentState = state;
            AddToHistory($"Переход в состояние: {state.GetStateName()}");
        }

        public string RequestId => _requestId;
        public string ClientName => _clientName;
        public decimal TicketPrice => _ticketPrice;
        public DateTime CreatedAt => _createdAt;
        public DateTime? PaymentDeadline 
        { 
            get => _paymentDeadline; 
            set => _paymentDeadline = value; 
        }

        public void SendToClient()
        {
            _currentState.SendToClient(this);
        }

        public void MakePayment()
        {
            _currentState.MakePayment(this);
        }

        public void ConfirmBooking()
        {
            _currentState.ConfirmBooking(this);
        }

        public void CancelRequest()
        {
            _currentState.CancelRequest(this);
        }

        public void AddToHistory(string action)
        {
            _stateHistory.Add($"[{DateTime.Now:HH:mm:ss}] {action}");
        }

        public void ShowHistory()
        {
            Console.WriteLine("\n📜 История заявки:");
            Console.WriteLine("════════════════════════════════════════");
            foreach (var entry in _stateHistory)
            {
                Console.WriteLine(entry);
            }
            Console.WriteLine("════════════════════════════════════════");
        }

        public void ShowCurrentState()
        {
            Console.WriteLine($"\n📊 Текущее состояние заявки: {_currentState.GetStateName()}");
            Console.WriteLine($"🆔 ID заявки: {_requestId}");
            Console.WriteLine($"👤 Клиент: {_clientName}");
            Console.WriteLine($"💰 Стоимость билета: {_ticketPrice:C}");
            Console.WriteLine($"🕒 Создана: {_createdAt:dd.MM.yyyy HH:mm:ss}");
            
            if (_paymentDeadline.HasValue)
            {
                var timeLeft = _paymentDeadline.Value - DateTime.Now;
                if (timeLeft.TotalSeconds > 0)
                {
                    Console.WriteLine($"⏰ До оплаты осталось: {timeLeft.Minutes} мин {timeLeft.Seconds} сек");
                }
                else
                {
                    Console.WriteLine($"⏰ Срок оплаты истек!");
                }
            }
            
            _currentState.PrintAvailableActions();
        }

        public bool IsPaymentExpired()
        {
            return _paymentDeadline.HasValue && DateTime.Now > _paymentDeadline.Value;
        }
    }
}

