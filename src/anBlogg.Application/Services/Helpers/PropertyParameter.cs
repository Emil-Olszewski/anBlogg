namespace anBlogg.Application.Services.Helpers
{
    public class PropertyParameter
    {
        public string PropertyName { get; private set; }
        public string Argument { get; private set; }

        public PropertyParameter(string propertyName, string argument = "")
        {
            PropertyName = propertyName;
            Argument = argument;
        }
    }
}