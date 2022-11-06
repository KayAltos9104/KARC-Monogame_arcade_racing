using System;

namespace KARC.WitchEngine
{
    public class Timer
    {
        public int Time { get; private set; }
        public event EventHandler<EventArgs> TimeIsOver = delegate { };
        public bool IsActive { get; private set; } = false;

        public Timer() 
        {

        }
        public Timer(int initialTime)
        {
            Activate(initialTime);
        }
        public void Activate(int initialTime)
        {
            if (initialTime <= 0)
                throw new ArgumentOutOfRangeException("Time cannot be equal zero or less");
            IsActive = true;
            Time = initialTime;
        }
        public void Update()
        {
            if (IsActive)
            {
                if (Time >= 0)
                    Time--;
                else
                {
                    IsActive = false;
                    Time = 0;
                    TimeIsOver.Invoke(this, EventArgs.Empty);
                }
            }
        }        
    }
}
