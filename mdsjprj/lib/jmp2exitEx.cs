using System.Runtime.Serialization;

namespace mdsj.lib
{
    [Serializable]
    internal class jmp2exitEx : Exception
    {
        public jmp2exitEx()
        {
         //   runtimeexc
        }

        public jmp2exitEx(string? message) : base(message)
        {
        }

        public jmp2exitEx(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected jmp2exitEx(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}