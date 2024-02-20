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
        sb.AppendLine("         - e.g. position  x   y");
        sb.AppendLine("         - e.g. position 150 200");
        sb.AppendLine("follow   - Sets position to current cursor");
        sb.AppendLine("interval - Sets auto clicker worker interval");
        sb.AppendLine("         - e.g. interval ss mm(opt) hh(opt)");
        sb.AppendLine("         - e.g. interval 5     8       1");
        sb.Append("reset    - Sets configurations to default");

        return sb.ToString();
    }
}
