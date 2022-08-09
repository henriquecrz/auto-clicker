using System.Runtime.InteropServices;
using Timer = System.Timers.Timer;

namespace auto_clicker
{
    public static class AutoClicker
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        // fazer macro programável

        private static bool _isFollowEnabled = default;
        private static Point _point = default;
        private static TimeSpan _delay = new(default, 0, 5);
        private static int _count;
        private static readonly Timer _timer = CreateTimer();

        public static void Start()
        {
            if (!_timer.Enabled)
            {
                _timer.Start();
            }
            else
            {
                Console.WriteLine($"The timer is already running.");
            }
        }

        public static void Stop()
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
            }
            else
            {
                Console.WriteLine($"The timer is already stopped.");
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

        public static void SetDelay(int hours, int minutes, int seconds)
        {
            _delay = new(hours, minutes, seconds);
        }

        private static Timer CreateTimer()
        {
            var timer = new Timer()
            {
                AutoReset = true,
                Interval = _delay.TotalMilliseconds
            };

            timer.Elapsed += (sender, e) => Worker();

            return timer;
        }

        private static void Worker()
        {
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

            Console.WriteLine($"Click{_count} {x}, {y}");

            _count++;
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point point);
    }
}
