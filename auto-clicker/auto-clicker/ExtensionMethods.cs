namespace auto_clicker
{
    public static class ExtensionMethods
    {
        public static string[] TakeToArray(this IEnumerable<string> collection, int max) =>
            collection
                .Take(max)
                .ToArray();
    }
}
