using System.Text;

namespace auto_clicker;

public static class Message
{
    public static readonly string HELP = Help();

    public static readonly string INVALID = "Invalid command.";

    private static string Help()
    {
        var sb = new StringBuilder();
        sb.AppendLine("start    - Starts auto clicker worker");
        sb.AppendLine("stop     - Stops auto clicker worker");
        sb.AppendLine("position - Sets cursor position");
        sb.AppendLine("         - e.g. position 50 50");
        sb.AppendLine("follow   - Sets position to current cursor");
        sb.AppendLine("interval - Sets auto clicker worker interval");
        sb.AppendLine("         - e.g. interval 5 5(opt) 5(opt)");
        sb.Append("reset    - Sets configurations to default");

        return sb.ToString();
    }
}
