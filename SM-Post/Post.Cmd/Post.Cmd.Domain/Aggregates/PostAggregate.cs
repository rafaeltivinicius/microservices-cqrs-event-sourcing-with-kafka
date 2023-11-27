using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _author;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public PostAggregate()
        {


        }

        public PostAggregate(Guid id, string author, string message)
        {

            RaiseEvent(new PostCreatedEvent
            {
                Id = id,
                Author = author,
                Message = message,
                DatePosted = DateTime.UtcNow
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _author = @event.Author;
            _active = true;
        }

        public void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("Only active posts can be edited");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException($"Post message cannot be empty - {nameof(message)}");
            }

            RaiseEvent(new MessageUpdatedEvent
            {
                Id = _id,
                Message = message
            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {

            _id = @event.Id;
        }

        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("Only active posts can be liked");
            }

            RaiseEvent(new PostLikedEvent
            {
                Id = _id
            });
        }

        public void Apply(PostLikedEvent @event)
        {
            _id = @event.Id;
        }

        public void AddComment(string comment, string username)
        {
            if (!_active)
                throw new InvalidOperationException("Only active posts can have add comments");

            if (string.IsNullOrWhiteSpace(comment))
                throw new InvalidOperationException($"Post comment cannot be empty - {nameof(comment)}");

            RaiseEvent(new CommentAddedEvent
            {
                Id = _id,
                Comment = comment,
                Username = username,
                CommentId = Guid.NewGuid(),
                CommentDate = DateTime.UtcNow

            });
        }

        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
        }

        public void EditComment(Guid commentId, string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("Only active posts can have comments added");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException("Only the comment author can edit the comment");
            }

            RaiseEvent(new CommentUpdateEvent
            {
                Id = _id,
                CommentId = commentId,
                Comment = comment,
                Username = username,
                EditDate = DateTime.UtcNow
            });
        }

        public void Apply(CommentUpdateEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }

        public void RemoveComment(Guid commentId, string username)
        {

            if (!_active)
            {
                throw new InvalidOperationException("Only active posts can have comments removed");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException("Only the comment author can removed the comment");
            }

            RaiseEvent(new CommentRemoveEvent
            {
                Id = _id,
                CommentId = commentId
            });
        }

        public void apply(CommentRemoveEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }

        public void DeletePosted(String username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("The post has already been deleted");
            }

            if (!_author.Equals(username, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException("Only the post author can delete the post");
            }

            RaiseEvent(new PostRemovedEvent
            {

                Id = _id
            });
        }

        public void Apply(PostRemovedEvent @event)
        {
            _id = @event.Id;
            _active = false;
        }
    }
}