using System.Runtime.InteropServices;
using Timer = System.Timers.Timer;

namespace auto_clicker;

public static partial class AutoClicker
{
    private const int MOUSEEVENTF_LEFTDOWN = 0x02;
    private const int MOUSEEVENTF_LEFTUP = 0x04;

    private static bool _isFollowEnabled = true;
    private static Point _point;
    private static int _count;
    private static Point _lastCursorPosition;

    private static readonly TimeSpan ONE_MINUTE = TimeSpan.FromMinutes(1);
    private static readonly Timer _clicker = CreateTimer(ONE_MINUTE.TotalMilliseconds, Click);
    private static readonly Timer _mouseMoveChecker = CreateTimer(500, CheckMouseMove);

    public static void Start()
    {
        if (!_clicker.Enabled)
        {
            _clicker.Start();
            _mouseMoveChecker.Start();

            Console.WriteLine("The timer started.");
        }
        else
        {
            Console.WriteLine("The timer is already running.");
        }
    }

    public static void Stop()
    {
        if (_clicker.Enabled)
        {
            _clicker.Stop();
            _mouseMoveChecker.Stop();

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
        _isFollowEnabled = false;
        _point = point;

        Console.WriteLine($"Position set to {point}.");
    }

    public static void SetInterval(int seconds, int minutes = default, int hours = default)
    {
        _clicker.Interval = new TimeSpan(hours, minutes, seconds).TotalMilliseconds;

        Console.WriteLine($"Interval set to {_clicker.Interval}.");
    }

    public static void Reset()
    {
        _isFollowEnabled = true;
        _point = default;
        _count = default;
        _clicker.Interval = ONE_MINUTE.TotalMilliseconds;

        Console.WriteLine("Configurations set to default.");
    }

    private static Timer CreateTimer(double interval, Action worker)
    {
        var timer = new Timer() { Interval = interval };

        timer.Elapsed += (sender, e) => worker();

        return timer;
    }

    private static void Click()
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

        Console.WriteLine($"Click{_count} | {DateTime.Now} | x:{x}, y:{y}");

        _count++;
    }

    private static void CheckMouseMove()
    {
        if (GetCursorPos(out Point cursorPosition) && (cursorPosition.X != _lastCursorPosition.X || cursorPosition.Y != _lastCursorPosition.Y))
        {
            _clicker.Stop();
            _clicker.Start();

            _lastCursorPosition = cursorPosition;
        }
    }

    [LibraryImport("user32.dll")]
    private static partial void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetCursorPos(out Point point);
}
