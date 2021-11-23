using System.Runtime.Serialization;

namespace EventSourcingDrinkAlcoholFun.Core.EventSourcing
{
    [Serializable]
    internal class MissingParameterLessConstructorException : Exception
    {
        private Type type;

        public MissingParameterLessConstructorException()
        {
        }

        public MissingParameterLessConstructorException(Type type)
        {
            this.type = type;
        }

        public MissingParameterLessConstructorException(string? message) : base(message)
        {
        }

        public MissingParameterLessConstructorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected MissingParameterLessConstructorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}