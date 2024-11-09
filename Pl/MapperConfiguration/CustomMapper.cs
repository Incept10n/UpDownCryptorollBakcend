using Bll.Dtos.QuizDtos;
using UpDownCryptorollBackend.Models.Quiz;

namespace UpDownCryptorollBackend.MapperConfiguration;

public class CustomMapper
{
    public List<QuizCheckDto> ToQuizCheckDtos(List<QuizCheckModel> quizCheckDtos)
    {
        return quizCheckDtos
            .Select(q => new QuizCheckDto
            {
                QuestionId = q.QuestionId,
                QuestionAnswer = q.QuestionAnswer
            })
            .ToList();
    }
}