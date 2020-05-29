using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace anBlogg.Domain.Services.Implementations
{
    public class TagsInString : ITagsInString
    {
        public IEnumerable<string> Enumerate(string raw)
        {
            foreach (var tag in EnumerateWithBrackets(raw))
                yield return RemoveBracketsFrom(tag);
        }

        public IEnumerable<string> EnumerateWithBrackets(string raw)
        {
            raw = DeleteWhitespaces(raw);
            var separatedTags = Regex.Split(raw, @"(?=<)");
            foreach (var tag in separatedTags)
                if (!string.IsNullOrEmpty(tag))
                    yield return tag;
        }

        private string RemoveBracketsFrom(string tag)
        {
            var rawTag = tag
                .Substring(0, tag.Length - 1)
                .Substring(1, tag.Length - 2);
            return rawTag;
        }

        public string CreateFrom(params string[] tags)
        {
            return InsertInto("", tags);
        }

        public string InsertInto(string raw, params string[] tags)
        {
            raw = DeleteWhitespaces(raw);
            foreach (var tag in tags)
            {
                var tagWithBrackets = AddBracketsTo(DeleteWhitespaces(tag));
                raw += tagWithBrackets;
            }

            return raw;
        }

        public string RemoveFrom(string raw, params string[] tags)
        {
            raw = DeleteWhitespaces(raw);
            foreach (var tag in tags)
            {
                var tagWithBrackets = AddBracketsTo(DeleteWhitespaces(tag));
                var startIndex = raw.IndexOf(tagWithBrackets);
                raw = startIndex < 0 ? raw : raw.Remove(startIndex, tagWithBrackets.Length);
            }

            return raw;
        }

        private string AddBracketsTo(string tag)
        {
            return $"<{tag}>";
        }

        private string DeleteWhitespaces(string text)
        {
            return Regex.Replace(text, @"\s+", "");
        }
    }
}