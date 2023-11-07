using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class CommentUpdateEvent : BaseEvent
    {
        public CommentUpdateEvent() : base(nameof(CommentUpdateEvent))
        {
        }
        public Guid CommentId { get; set; }
        public string Comment { get; set; }
        public string Username { get; set; }
        public DateTime EditDate { get; set; }
    }
}