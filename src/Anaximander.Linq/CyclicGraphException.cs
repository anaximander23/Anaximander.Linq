using System;
using System.Runtime.Serialization;

namespace Anaximander.Linq.TreeTraversal
{
    [Serializable]
    public class CyclicGraphException : Exception
    {
        public CyclicGraphException()
            : base("The provided data structure contains cyclic references, which are not allowed.")
        {
        }

        public CyclicGraphException(string message) : base(message)
        {
        }

        public CyclicGraphException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CyclicGraphException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}