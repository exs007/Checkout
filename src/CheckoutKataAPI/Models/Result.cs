using System.Collections.Generic;
using System.Linq;

namespace CheckoutKataAPI.Models
{
    /// <summary>
    /// Common repsonse structure
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Result<T>
    {
        private readonly List<MessageInfo> _messages;

        public Result(bool status, T data = default(T))
        {
            _messages = new List<MessageInfo>();
            Data = data;
            Success = status;
        }

        public T Data { get; }

        public bool Success { get; }

        public void AddMessage(string field, string message)
        {
            _messages.Add(new MessageInfo
            {
                Field = field,
                Message = message
            });
        }

        public void AddMessages(IEnumerable<MessageInfo> messages)
        {
            _messages.AddRange(messages);
        }

        public IEnumerable<MessageInfo> Messages => _messages.AsEnumerable();

        public static implicit operator Result<T>(T value)
        {
            return new Result<T>(true, value);
        }
    }
}