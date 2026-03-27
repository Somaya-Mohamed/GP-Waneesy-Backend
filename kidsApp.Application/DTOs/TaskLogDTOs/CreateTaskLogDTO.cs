namespace kidsApp.Application.DTOs.TaskLogDTOs
{
    public class CreateTaskLogDTO
    {
        public int ChildId { get; set; }
        public int TaskId { get; set; }
        public bool IsCompleted { get; set; }
    }
}