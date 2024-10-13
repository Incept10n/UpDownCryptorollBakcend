namespace Bll.Exceptions;

public class TaskOutOfRangeException : Exception
{
    public TaskOutOfRangeException(string message)
        : base(message) { }
    
    public TaskOutOfRangeException(string message, Exception exception)
        : base(message, exception) { }
} 