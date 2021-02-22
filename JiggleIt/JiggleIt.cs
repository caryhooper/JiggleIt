using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace JiggleIt
{
    class JiggleIt
    {
        //TODO
        //Windows Form to enable/disable
        //Move in a circle instead of a square.
        //Rewrite in JavaScript + host on web page
        //Implement Logger class for debug Console.WriteLines
        //
        
        
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);
        
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);
        private static void Main(string[] args)
        {
            banner();
            long jiggleMinutes = 0;
            if (args.Length == 0)
            {
                jiggleMinutes = 535960; //1yr
                Console.WriteLine($"Jiggling the mouse for a year.  Go grab a coffee, this may take a while...");
            }
            else if (args.Length == 1){
                jiggleMinutes = Int64.Parse(args[0]);
                if (jiggleMinutes > 1)
                {
                    Console.WriteLine($"Starting to jiggle the mouse for {jiggleMinutes} minutes.");
                }
                else
                {
                    Console.WriteLine($"Starting to jiggle the mouse for {jiggleMinutes} minute.");
                }
            }
            else
            {
                Console.WriteLine("Usage:   ./JiggleIt.exe minutes_to_jiggle");
                Console.WriteLine("Example: ./JiggleIt.exe 120");
                System.Environment.Exit(1);
            } 
            long jiggleSeconds = jiggleMinutes * 60;
            int jump = 100;

            Point mouse = default;
            mouse = reportCursorPosition(mouse);
            int xval = mouse.X;
            int yval = mouse.Y;

            for (int i = 1; i < jiggleSeconds; i++)
            {
                SetCursorPos(xval + jump, yval);
                reportCursorPosition(mouse);
                SetCursorPos(xval + jump, yval + jump);
                reportCursorPosition(mouse);
                SetCursorPos(xval, yval + jump);
                reportCursorPosition(mouse);
                SetCursorPos(xval, yval);
                reportCursorPosition(mouse);
                //Here we take a break for 10 seconds every two minutes.  This is to allow users to regain control
                //if they accidentally ran the program and can't hit CTL + C
                if (jiggleSeconds % 60 == 0)
                {
                    Thread.Sleep(10 * 1000);
                    jiggleSeconds += 10;
                } 
                //Console.WriteLine($"Iteration: {i}");
            }
        }

        private static Point reportCursorPosition(Point mouse)
        {
            GetCursorPos(out mouse);
            //Console.WriteLine($"Mouse X:{mouse.X} Y:{mouse.Y}");
            Thread.Sleep(250);
            return mouse;
        }
        private static void banner()
        {
            Console.WriteLine("JiggleIt\tv0.1");
            Console.WriteLine("Cary Hooper, 2021, @nopantrootdance\n");
            Console.WriteLine("Move your mouse in circles for any amount of time (default: one year).");
        }
    }
}
