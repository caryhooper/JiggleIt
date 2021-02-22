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
        //Implement Logger class for debug Console.WriteLines
        //Cross-platform?  different shared libraries?   

        //Publish with:
        //dotnet publish --self-contained true -r win10-x64 -p:PublishSingleFile=true

        //User reflection to access Win32 API methods, GetCursorPos & SetCursorPos
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);
        //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getcursorpos
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);
        //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setcursorpos

        private static void Main(string[] args)
        {
            banner(); //Displays banner
            long jiggleMinutes = 0;
            //Parse user args, if any
            if (args.Length == 0)
            {
                jiggleMinutes = 535960; //1yr
                Console.WriteLine($"Jiggling the mouse for a year.  Go grab a coffee, this may take a while...");
            }
            else if (args.Length == 1){
                
                try{
                    //Exception handling for bad user input.
                    jiggleMinutes = Int64.Parse(args[0]);
                }
                catch (FormatException e)
                {
                    Console.WriteLine($"Error: {args[0]} is not a valid number of minutes.");
                }
                    

                if (jiggleMinutes > 1)
                {
                    //Logic for grammar.  There is definintely a better way to do this.
                    Console.WriteLine($"Starting to jiggle the mouse for {jiggleMinutes} minutes.");
                }
                else
                {
                    Console.WriteLine($"Starting to jiggle the mouse for {jiggleMinutes} minute.");
                }
            }
            else
            {
                //More than one argument?  exit
                Console.WriteLine("Usage:   ./JiggleIt.exe minutes_to_jiggle");
                Console.WriteLine("Example: ./JiggleIt.exe 120");
                System.Environment.Exit(1);
            }
            long jiggleSeconds = jiggleMinutes * 60;
            //jump is the distance in px the mouse moves
            int jump = 100;

            //Define a mouse pointer object and assign coordinates
            Point mouse = default;
            mouse = reportCursorPosition(mouse);
            int xval = mouse.X;
            int yval = mouse.Y;

            //Loop for X number of seconds
            for (int i = 2; i < jiggleSeconds; i++)
            {
                SetCursorPos(xval + jump, yval);
                reportCursorPosition(mouse);
                SetCursorPos(xval + jump, yval + jump);
                reportCursorPosition(mouse);
                SetCursorPos(xval, yval + jump);
                reportCursorPosition(mouse);
                SetCursorPos(xval, yval);
                reportCursorPosition(mouse);
                //Here we take a break for 5 seconds every two minutes.  This is to allow users to regain control
                //if they accidentally ran the program and can't hit CTL + C
                if (jiggleSeconds % 60 == 0)
                {
                    Thread.Sleep(5 * 1000);
                    jiggleSeconds += 10;
                } 
                //Console.WriteLine($"Iteration: {i}");
            }
        }

        //Method to retrieve mouse pointer cooridnates and sleep for 1/4 second.
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
