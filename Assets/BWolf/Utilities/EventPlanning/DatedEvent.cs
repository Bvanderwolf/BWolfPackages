using System;

namespace BWolf.Utilities.EventPlanning
{
    public struct DatedEvent
    {
        public CalenderDate Date { get; private set; }

        private Action<DatedEvent> _onEvent;

        public DatedEvent(CalenderDate date, Action<DatedEvent> onEvent)
        {
            Date = date;
            _onEvent = onEvent;
        }

        public void Execute()
        {
            _onEvent(this);
        }
    }
}