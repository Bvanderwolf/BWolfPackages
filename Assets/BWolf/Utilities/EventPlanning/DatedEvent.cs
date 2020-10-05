using System;

namespace BWolf.Utilities.EventPlanning
{
    public struct DatedEvent
    {
        public EventDate Date { get; private set; }

        private Action<DatedEvent> _onEvent;

        public DatedEvent(EventDate date, Action<DatedEvent> onEvent)
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