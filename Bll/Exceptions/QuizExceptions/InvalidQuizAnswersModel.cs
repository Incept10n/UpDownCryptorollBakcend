namespace Bll.Exceptions.QuizExceptions;

public class InvalidQuizAnswersModel : Exception
{
    public InvalidQuizAnswersModel(string message) 
        : base(message) { }
    
    public InvalidQuizAnswersModel(string message, Exception inner)
        : base(message, inner) { }
}