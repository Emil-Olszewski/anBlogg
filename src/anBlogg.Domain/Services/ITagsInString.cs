using System.Collections.Generic;

namespace anBlogg.Domain.Services
{
    public interface ITagsInString
    {
        IEnumerable<string> Enumerate(string raw);
        IEnumerable<string> EnumerateWithBrackets(string raw);
        string CreateFrom(params string[] tags);
        string InsertInto(string raw, params string[] tags);
        string RemoveFrom(string raw, params string[] tags);
    }
}
