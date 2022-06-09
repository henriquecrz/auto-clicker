using System.Runtime.InteropServices;

const int MOUSEEVENTF_LEFTDOWN = 0x02;
const int MOUSEEVENTF_LEFTUP = 0x04;

string? command = string.Empty;
var cts = new CancellationTokenSource();

do
{
    var a = ThreadPool.QueueUserWorkItem(new WaitCallback(Work), cts.Token);

    Console.WriteLine("Command: ");
    command = Console.ReadLine();

} while (command != "q");

cts.Cancel();

static void Work(object obj)
{
    CancellationToken cancellationToken = (CancellationToken)obj;

    while (!cancellationToken.IsCancellationRequested)
    {
        Thread.Sleep(new TimeSpan(0, 1, 0));

        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

        Console.WriteLine("Clicou");
    }
}

//var errorCode = SetCursorPos(0, 0);

//[DllImport("user32.dll")]
//static extern int SetCursorPos(int x, int y);

[DllImport("user32.dll")]
static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
