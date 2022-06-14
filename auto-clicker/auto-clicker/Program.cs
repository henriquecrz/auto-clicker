namespace auto_clicker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (IsCoordinateValid(args, out var point))
            {
                AutoClicker.SetCursorPosition(point);
            }

            var cancellationTokenSource = new CancellationTokenSource();
            string command;

            do
            {
                new Thread(AutoClicker.Worker).Start(cancellationTokenSource.Token);

                Console.Write("Command: ");
                command = (Console.ReadLine() ?? string.Empty).Trim();

                CommandSwitcher(command);

            } while (command != Command.QUIT);

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        static bool IsCoordinateValid(string[] arguments, out Point point)
        {
            if (arguments.Length >= 2 &&
                int.TryParse(arguments[0], out var x) &&
                int.TryParse(arguments[1], out var y))
            {
                point = new Point(x, y);

                return true;
            }

            point = new Point();

            return false;
        }

        static void CommandSwitcher(string command)
        {
            switch (true)
            {
                case true when command.StartsWith(Command.QUIT):
                    break;
                case true when command.StartsWith(Command.SET):
                    SetCursorPosition(command);
                    break;
                default:
                    Console.WriteLine(Command.INVALID);
                    break;
            }
        }

        static void SetCursorPosition(string command)
        {
            var arguments = command
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Take(1..^0)
                .ToArray();

            var first = arguments.First();

            switch (true)
            {
                case true when IsFollowCommand(first):
                    AutoClicker.SwitchIsFollowEnabled();
                    break;
                case true when IsCoordinateValid(arguments, out var point):
                    AutoClicker.SetCursorPosition(point);
                    break;
                default:
                    Console.WriteLine(Command.INVALID);
                    break;
            }
        }

        static bool IsFollowCommand(string arg) => arg == Command.FOLLOW;
    }
}
