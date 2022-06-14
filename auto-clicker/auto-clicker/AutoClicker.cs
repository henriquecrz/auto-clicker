using System.Runtime.InteropServices;

namespace auto_clicker
{
    public static class AutoClicker
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        private static readonly TimeSpan ONE_MINUTE = new(0, 1, 0);

        private static bool _isFollowEnabled = false;

        private static Point _point = new();

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

                int x, y;

                if (_isFollowEnabled && GetCursorPos(out var point))
                {
                    x = point.X;
                    y = point.Y;
                }
                else
                {
                    x = _point.X;
                    y = _point.Y;
                }

                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);

                Console.WriteLine("Click " + i);
            }
        }

        public static void SwitchIsFollowEnabled()
        {
            _isFollowEnabled = !_isFollowEnabled;
        }

        public static void SetCursorPosition(Point point)
        {
            _point = point;
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point point);
    }
}
