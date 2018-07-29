using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutKataAPI.Models;

namespace CheckoutKataAPI.Exceptions
{
    /// <summary>
    /// Business rules exception
    /// </summary>
    public class AppValidationException : Exception
    {
        public IReadOnlyList<MessageInfo> Messages => _messages;
        private readonly List<MessageInfo> _messages = new List<MessageInfo>();

        public AppValidationException(IEnumerable<MessageInfo> messages) : base("See messages")
        {
            _messages.AddRange(messages);
        }

        public AppValidationException(MessageInfo messages) : base("See messages")
        {
            _messages.Add(messages);
        }

        public AppValidationException(string field, string message) : this(new MessageInfo
        {
            Field = field,
            Message = message,
        })
        {

        }

        public AppValidationException(string message) : this(new MessageInfo 
        {
            Field = string.Empty,
            Message = message,
        })
        {
        }

        public override string ToString() => string.Join(Environment.NewLine, _messages.Select(m => m.ToString()));

    }
}
