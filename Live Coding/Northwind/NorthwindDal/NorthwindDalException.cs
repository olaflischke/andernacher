using System.Runtime.Serialization;

namespace NorthwindDal
{
    [Serializable]
    public class NorthwindDalException : Exception
    {
        public NorthwindDalException()
        {
        }

        public NorthwindDalException(string? message) : base(message)
        {
        }

        public NorthwindDalException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NorthwindDalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}