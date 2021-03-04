using System;
using System.Timers;


namespace MoveMouse
{
    class Program
    {
        private static Timer aTimer;
        private static Random rando = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("Let's Manipulate Some Input!");

            // Create a timer and set a two second interval.
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 2000;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;

            Console.WriteLine("Press the Enter key to exit the program at any time... ");
            Console.ReadLine();
        }

        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {

            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);

            // Current best method
            // ===================
            LPINPUT mouseCoords1 = InputControl.CreateMouseMoveInput(25, 20);
            LPINPUT mouseCoords2 = InputControl.CreateMouseMoveInput(55, 50);
            LPINPUT WkeyDown = InputControl.CreateKeyboardScancodeInput(0x11, KEYEVENTF.KEYDOWN);
            LPINPUT WkeyUp = InputControl.CreateKeyboardScancodeInput(0x11, KEYEVENTF.KEYUP);

            LPINPUT[] inputs = { mouseCoords1, WkeyDown, mouseCoords2, WkeyUp };
            InputControl.SendInputArray(inputs);

            // Unreliable method
            // =================
            // POINT p = new POINT(rando.Next(501), rando.Next(1001));
            // InputControl.ClientToScreen(InputControl.GetForegroundWindow(), ref p);
            // InputControl.SetCursorPos(p.x, p.y);

            // Old method
            // ==========
            // InputControl.Move(40, 40);
        }
    }
}
