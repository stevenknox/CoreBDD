namespace CoreBDD.SpecGeneration
{
    public static class Extensions
    {
        public static bool IsGherkin(this string input)
        {
            return input.StartsWith("Given") || input.StartsWith("When") || input.StartsWith("Then") || input.StartsWith("And") || input.StartsWith("AndAlso");
        }
        public static string TrimExpression(this string input)
        {
            return input.TrimStart("this.".ToCharArray());
        }
        public static string TrimArgument(this string input)
        {
            return input.Trim('$').Trim('"');
        }
    }
}