using System.Runtime.Serialization;

namespace mdsj.lib
{
    [Serializable]
    internal class retFunStpNxtEx : Exception
    {
        public retFunStpNxtEx()
        {
        }

        public retFunStpNxtEx(string? message) : base(message)
        {
        }

        public retFunStpNxtEx(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected retFunStpNxtEx(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}