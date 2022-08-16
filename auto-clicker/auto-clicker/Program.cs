namespace auto_clicker
{
    internal class Program
    {
        private const int MAX_ARGS = 4;
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
                var input = (Console.ReadLine() ?? string.Empty).Trim();

                CommandSwitcher(input, out quit);
            }
            while (!quit);
        }

        private static void CommandSwitcher(string[] args)
        {
            switch (args.FirstOrDefault())
            {
                case Command.START:
                    AutoClicker.Start();
                    break;
                case Command.FOLLOW:
                    AutoClicker.SwitchIsFollowEnabled();
                    break;
                case Command.POSITION:
                    SetCursorPosition(args);
                    break;
                case Command.INTERVAL:
                    SetInterval(args);
                    break;
                default:
                    Console.WriteLine(Command.INVALID);
                    break;
            }
        }

        private static void CommandSwitcher(string input, out bool quit)
        {
            quit = default;

            var arguments = input
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
                case Command.INTERVAL:
                    SetInterval(arguments);
                    break;
                case Command.RESET:
                    AutoClicker.Reset();
                    break;
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
            var parameters = arguments.TakeToArray(MAX_ARGS, 1);

            if (IsCoordinateValid(parameters, out var point))
            {
                AutoClicker.SetCursorPosition(point);
            }
            else
            {
                Console.WriteLine(Command.INVALID);
            }
        }

        private static bool IsCoordinateValid(string[] parameters, out Point point)
        {
            int x = default, y = default;

            var isSuccess = parameters.Length >= 2 &&
                int.TryParse(parameters[0], out x) &&
                int.TryParse(parameters[1], out y);

            point = new Point(x, y);

            return isSuccess;
        }

        private static void SetInterval(string[] arguments)
        {
            try
            {
                var parameters = arguments.TakeToArray(MAX_ARGS, 1);

                switch (parameters.Length)
                {
                    case 1:
                        AutoClicker.SetInterval(int.Parse(parameters[0]));
                        break;
                    case 2:
                        AutoClicker.SetInterval(int.Parse(parameters[0]), int.Parse(parameters[1]));
                        break;
                    case 3:
                        AutoClicker.SetInterval(int.Parse(parameters[0]), int.Parse(parameters[1]), int.Parse(parameters[2]));
                        break;
                    default:
                        Console.WriteLine(Command.INVALID);
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine(Command.INVALID);
            }
        }
    }
}
