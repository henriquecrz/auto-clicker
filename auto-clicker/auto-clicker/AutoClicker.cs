using System.Runtime.InteropServices;

namespace auto_clicker
{
    public static class AutoClicker
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        private static readonly TimeSpan ONE_MINUTE = new(0, 1, 0); // parametrizar isso

        //private static Timer a = new Timer();

        private static CancellationTokenSource _cancellationTokenSource = new();
        private static Task _task = new(Worker, _cancellationTokenSource.Token); // fazer com timer polling
        private static bool _isFollowEnabled = default;
        private static Point _point = default;

        public static void Start()
        {
            if (_task.Status is not TaskStatus.Running)
            {
                _cancellationTokenSource = new();
                _task = new(Worker, _cancellationTokenSource.Token);

                _task.Start();
            }
            else
            {
                Console.WriteLine($"The task is already running.");
            }
        }

        public static void Stop()
        {
            if (_task.Status is TaskStatus.Running)
            {
                _cancellationTokenSource.Cancel();
            }
            else
            {
                Console.WriteLine($"The task is already canceled.");
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

        private static async void Worker()
        {
            for (int i = 1; !_cancellationTokenSource.Token.IsCancellationRequested; i++)
            {
                try
                {
                    await Task.Delay(ONE_MINUTE, _cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    return;
                }

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

                Console.WriteLine($"Click{i} {x}, {y}");
            }
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point point);
    }
}
