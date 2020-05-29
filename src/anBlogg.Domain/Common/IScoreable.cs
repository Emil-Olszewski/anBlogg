using anBlogg.Domain.ValueObjects;

namespace anBlogg.Domain.Common
{
    public interface IScoreable
    {
        Score Score { get; set; }
    }
}