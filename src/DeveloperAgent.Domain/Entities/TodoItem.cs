namespace DeveloperAgent.Domain.Entities
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsComplete { get; set; }

        public TodoItem() {}
        public TodoItem(string title) => Title = title;
    }
}