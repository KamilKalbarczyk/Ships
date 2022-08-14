using System;

namespace TEST
{
    class Field
    {
        private FieldState _state;

        public Field()
        {
            _state = FieldState.Empty;
        }

        public bool IsShip => _state == FieldState.Ship;
        public bool IsNotAvailable => _state == FieldState.Ship || _state == FieldState.NotAvailable;


        public void Ship()
        {
            _state = FieldState.Ship;
        }

        public void Shot()
        {
            if(_state == FieldState.Ship)
            {
                _state = FieldState.Sink;
            }
            else
            {
                _state = FieldState.Miss;
            }
        }

        public void NotAvailable()
        {
            _state = FieldState.NotAvailable;
        }

        public void Display()
        {
            if (_state == FieldState.Miss) Console.Write("X");
            else if (_state == FieldState.Sink) Console.Write("!");
            else Console.Write("O");
        }
    }
}