namespace auto_clicker
{
    internal class Program
    {
        private const int MAX_ARGS = 3;
        private const char SPACE = ' ';

        static void Main(string[] args)
        {
            if (args.Length != default)
            {
                CommandSwitcher(args);
            }

            bool quit;

            do
            {
                Console.Write(">");
                var command = (Console.ReadLine() ?? string.Empty).Trim();

                CommandSwitcher(command, out quit);
            }
            while (!quit);
        }

        private static void CommandSwitcher(string[] args)
        {
            var arguments = args.TakeToArray(MAX_ARGS);

            switch (arguments.FirstOrDefault())
            {
                case Command.START:
                    AutoClicker.Start();
                    break;
                case Command.FOLLOW:
                    AutoClicker.SwitchIsFollowEnabled();
                    break;
                case Command.POSITION:
                    SetCursorPosition(arguments);
                    break;
                case Command.INTERVAL:
                    AutoClicker.SetInterval();
                    break;
                default:
                    Console.WriteLine(Command.INVALID);
                    break;
            }
        }

        private static void CommandSwitcher(string command, out bool quit)
        {
            quit = default;

            var arguments = command
                .Split(SPACE, StringSplitOptions.RemoveEmptyEntries)
                .TakeToArray(MAX_ARGS);

            switch (arguments.FirstOrDefault())
            {
                case Command.START:
                    AutoClicker.Start();
                    break;
                case Command.STOP:
                    AutoClicker.Stop();
                    break;
                case Command.FOLLOW:
                    AutoClicker.SwitchIsFollowEnabled();
                    break;
                case Command.POSITION:
                    SetCursorPosition(arguments);
                    break;
                //case Command.
                case Command.QUIT:
                    quit = true;
                    break;
                default:
                    Console.WriteLine(Command.INVALID);
                    break;
            }
        }

        private static void SetCursorPosition(string[] arguments)
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

        private static bool IsFollowCommand(string? arg) => arg == Command.FOLLOW;

        private static bool IsCoordinateValid(string[] parameters, out Point point)
        {
            int x = default;
            int y = default;

            var isSuccess = parameters.Length >= 2 &&
                int.TryParse(parameters[0], out x) &&
                int.TryParse(parameters[1], out y);

            point = new Point(x, y);

            return isSuccess;
        }
    }
}
