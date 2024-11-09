namespace Bll.Dtos.QuizDtos;

public class QuizAnswersResponseDto
{
    public bool IsQuizCompleted { get; set; } = true;
    public List<QuizResponseDto> QuizResponses { get; set; } = new();
}