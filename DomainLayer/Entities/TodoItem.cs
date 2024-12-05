namespace DomainLayer.Entities
{
    public class TodoItem
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public TodoItem(string title, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.");
            Title = title;
            Description = description;
            IsCompleted = false;
            CreatedDate = DateTime.UtcNow;
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
        }

        public void SetDescription(string? description)
        {
            Description = description;
        }
        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.");

            Title = title;
        }
        public void SetCompleted(bool isCompleted)
        {
            IsCompleted = isCompleted;
        }

    }
}
