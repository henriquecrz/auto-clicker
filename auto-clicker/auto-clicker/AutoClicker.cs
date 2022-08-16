using System.Runtime.InteropServices;
using Timer = System.Timers.Timer;

namespace auto_clicker
{
    public static class AutoClicker
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        private static bool _isFollowEnabled = true;
        private static Point _point;
        private static int _count;

        private static readonly TimeSpan ONE_MINUTE = TimeSpan.FromMinutes(1);
        private static readonly Timer _timer = CreateTimer();

        public static void Start()
        {
            if (!_timer.Enabled)
            {
                _timer.Start();

                Console.WriteLine("The timer started.");
            }
            else
            {
                Console.WriteLine("The timer is already running.");
            }
        }

        public static void Stop()
        {
            if (_timer.Enabled)
            {
                _timer.Stop();

                Console.WriteLine("The timer stopped.");
            }
            else
            {
                Console.WriteLine("The timer is already stopped.");
            }
        }

        public static void SwitchIsFollowEnabled()
        {
            _isFollowEnabled = !_isFollowEnabled;

            Console.WriteLine($"Follow set to {_isFollowEnabled}.");
        }

        public static void SetCursorPosition(Point point)
        {
            _point = point;

            Console.WriteLine($"Follow set to {_isFollowEnabled}");
        }

        public static void SetInterval(int seconds, int minutes = default, int hours = default)
        {
            _timer.Interval = new TimeSpan(hours, minutes, seconds).TotalMilliseconds;

            Console.WriteLine($"Interval set to {_timer.Interval}.");
        }

        public static void Reset()
        {
            _isFollowEnabled = true;
            _point = default;
            _count = default;
            _timer.Interval = ONE_MINUTE.TotalMilliseconds;

            Console.WriteLine("Configurations set to default.");
        }

        private static Timer CreateTimer()
        {
            var timer = new Timer()
            {
                AutoReset = true,
                Interval = ONE_MINUTE.TotalMilliseconds
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
