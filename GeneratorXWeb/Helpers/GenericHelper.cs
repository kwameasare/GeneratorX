namespace GeneratorXWeb.Helpers
{
    public class GenericHelper
    {

        public string ToCamel(string text)
        {
            var camel = Char.ToLowerInvariant(text[0]) + text.Substring(1);
            return camel;
        }

    }
}
