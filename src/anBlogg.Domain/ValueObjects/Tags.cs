using anBlogg.Domain.Common;
using anBlogg.Domain.Services;
using anBlogg.Domain.Services.Implementations;
using System.Collections.Generic;

namespace anBlogg.Domain.ValueObjects
{
    public class Tags : ValueObject
    {
        public string Raw { get; protected set; }
        private readonly ITagsInString tagsInString = new TagsInString();

        public Tags(string raw)
        {
            Raw = raw;
        }

        public Tags(params string[] tags)
        {
            Raw = tagsInString.CreateFrom(tags);
        }

        public IEnumerable<string> Enumerate()
        {
            return tagsInString.Enumerate(Raw);
        }

        public void Add(params string[] tags)
        {
            Raw = tagsInString.InsertInto(Raw, tags);
        }

        public void Remove(params string[] tags)
        {
            Raw = tagsInString.RemoveFrom(Raw, tags);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            return Enumerate();
        }
    }
}