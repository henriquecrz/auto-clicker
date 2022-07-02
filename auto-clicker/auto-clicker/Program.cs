namespace auto_clicker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SetCursorPosition(args);
            AutoClicker.Start();

            bool quit;

            do
            {
                Console.Write("Command: ");
                var command = (Console.ReadLine() ?? string.Empty).Trim();

                CommandSwitcher(command, out quit);
            }
            while (!quit);
        }

        static void SetCursorPosition(string[] arguments)
        {
            var parameters = arguments
                .Take(1..^0)
                .ToArray();

            switch (true)
            {
                case true when IsFollowCommand(parameters.FirstOrDefault()):
                    AutoClicker.SwitchIsFollowEnabled();
                    break;
                case true when IsCoordinateValid(parameters, out var point):
                    AutoClicker.SetCursorPosition(point);
                    break;
                default:
                    Console.WriteLine(Command.INVALID);
                    break;
            }
        }

        static bool IsFollowCommand(string? arg) => arg == Command.FOLLOW;

        static bool IsCoordinateValid(string[] parameters, out Point point)
        {
            int x = default;
            int y = default;

            var isSuccess = parameters.Length >= 2 &&
                int.TryParse(parameters[0], out x) &&
                int.TryParse(parameters[1], out y);

            point = new Point(x, y);

            return isSuccess;
        }

        static void CommandSwitcher(string command, out bool quit)
        {
            quit = default;

            var arguments = command
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Take(3)
                .ToArray();

            switch (arguments.FirstOrDefault())
            {
                case Command.QUIT:
                    quit = true;
                    break;
                case Command.START:
                    AutoClicker.Start();
                    break;
                case Command.STOP:
                    AutoClicker.Stop();
                    break;
                case Command.SET:
                    SetCursorPosition(arguments);
                    break;
                default:
                    Console.WriteLine(Command.INVALID);
                    break;
            }
        }
    }
}
