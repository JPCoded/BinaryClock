namespace BinaryClock
{
    internal class BinaryCircles
    {
        public BinaryCircle[] Circles { get; set; }
        private int CurrentPosition { get; set; }
        public bool AutoReset { private get; set; }

        public BinaryCircle Next()
        {
            if (CurrentPosition >= Circles.Length && AutoReset)
            {
                CurrentPosition = 0;
            }
            var current = Circles[CurrentPosition];
            CurrentPosition++;
            return current;
        }

        public void Reset()
        {
            CurrentPosition = 0;
        }
    }
}