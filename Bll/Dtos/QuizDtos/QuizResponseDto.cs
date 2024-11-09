namespace Bll.Dtos.QuizDtos;

public class QuizResponseDto
{
    public int QuestionId { get; set; }
    public int QuestionAnswer { get; set; }
    public bool IsQuestionAnsweredCorrectly { get; set; }
}