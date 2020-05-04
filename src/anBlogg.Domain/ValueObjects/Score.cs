using anBlogg.Domain.Common;
using System.Collections.Generic;

namespace anBlogg.Domain.ValueObjects
{
    public class Score : ValueObject
    {
        public int Value { get; private set; }

        public void Increase()
        {
            Value++;
        }

        public void Decrease()
        {
            Value--;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
