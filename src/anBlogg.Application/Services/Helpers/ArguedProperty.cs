namespace anBlogg.Application.Services.Helpers
{
    public class ArguedProperty
    {
        public string PropertyName { get; private set; }
        public string Argument { get; private set; }

        public ArguedProperty(string propertyName, string argument = "")
        {
            PropertyName = propertyName;
            Argument = argument;
        }
    }
}