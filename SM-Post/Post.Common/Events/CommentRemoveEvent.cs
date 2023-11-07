using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class CommentRemoveEvent : BaseEvent
    {
        public CommentRemoveEvent() : base(nameof(CommentRemoveEvent))
        {
        }

        public Guid CommentId {get;set;}
    }
}