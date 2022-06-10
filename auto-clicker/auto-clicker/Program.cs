using auto_clicker;

string command;
var cancellationTokenSource = new CancellationTokenSource();

do
{
    var autoClickerThread = new Thread(AutoClicker.Worker);
    autoClickerThread.Start(cancellationTokenSource.Token);

    Console.Write("Command: ");
    command = (Console.ReadLine() ?? string.Empty).Trim();

    CommandSwitcher(command);

} while (command != Command.QUIT);

cancellationTokenSource.Cancel();
cancellationTokenSource.Dispose(); // really need?

Environment.Exit(0);

static void CommandSwitcher(string command)
{
    switch (true)
    {
        case true when command.StartsWith(Command.SET):
            AutoClicker.SetCursorPosition(command);
            break;
        default:
            break;
    }
}
































//var autoClickerThread = new Thread((obj) =>
//{
//    CancellationToken cancellationToken = (CancellationToken)obj;

//    while (!cancellationToken.IsCancellationRequested)
//    {
//        Thread.Sleep(new TimeSpan(0, 1, 0));

//        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

//        Console.WriteLine("Clicou");
//    }
//});

//autoClickerThread.Start();

//var a = ThreadPool.QueueUserWorkItem(new WaitCallback(Work), cts.Token);