using System.Drawing;
using System.Runtime.InteropServices;

namespace auto_clicker
{
    public static class AutoClicker
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        private const char WHITESPACE = ' ';

        private static int _x = 0;
        private static int _y = 0;

        private static readonly TimeSpan ONE_MINUTE = new(0, 1, 0);

        public static void Worker(object? obj)
        {
            CancellationToken cancellationToken;

            try
            {
                if (obj is not null)
                {
                    cancellationToken = (CancellationToken)obj;
                }
                else
                {
                    throw new ArgumentNullException($"{obj}");
                }
            }
            catch (InvalidCastException)
            {
                throw;
            }
            catch (ArgumentNullException)
            {
                throw;
            }

            for (int i = 1; !cancellationToken.IsCancellationRequested; i++)
            {
                Thread.Sleep(ONE_MINUTE);

                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, _x, _y, 0, 0);

                Console.WriteLine("Click " + i);
            }
        }

        public static void SetCursorPosition(string command)
        {
            var splittedCommand = command
                .Split(WHITESPACE, StringSplitOptions.RemoveEmptyEntries)
                .Take(3)
                .ToList();

            switch (splittedCommand.Count)
            {
                case 2:
                    if (splittedCommand[1] == Command.FOLLOW &&
                        GetCursorPos(out var point))
                    {
                        _x = point.X;
                        _y = point.Y;
                    }
                    break;
                case 3:
                    if (int.TryParse(splittedCommand[1], out var x) &&
                        int.TryParse(splittedCommand[2], out var y))
                    {
                        _x = x;
                        _y = y;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out Point point);

        //[StructLayout(LayoutKind.Sequential)]
        //public struct POINT
        //{
        //    public int X;
        //    public int Y;

        //    public static implicit operator Point(POINT point)
        //    {
        //        return new Point(point.X, point.Y);
        //    }
        //}
    }
}
