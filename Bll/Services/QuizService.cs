using Bll.Dtos.QuizDtos;
using Bll.Dtos.Tasks;
using Bll.Exceptions.QuizExceptions;
using Dal.DatabaseContext;
using Dal.Enums;

namespace Bll.Services;

public class QuizService(
    ApplicationDbContext dbContext,
    RewardTaskService rewardTaskService)
{
    public QuizAnswersResponseDto CheckQuizAnswers(string username, List<QuizCheckDto> quizAnswers)
    {
        var correctQuizAnswers = dbContext.Quiz.ToList();

        if (quizAnswers.Count != correctQuizAnswers.Count)
            throw new InvalidQuizAnswersModel($"quiz answers count is invalid, " +
                                              $"should be {correctQuizAnswers.Count} " +
                                              $"but found {quizAnswers.Count}");
        
        quizAnswers.Sort((x, y) => x.QuestionId < y.QuestionId ? -1 : 1);
        var quizAnswerResponseDto = new QuizAnswersResponseDto();

        for (var i = 0; i < correctQuizAnswers.Count; i++)
        {
            if (correctQuizAnswers[i].Id != quizAnswers[i].QuestionId)
                throw new InvalidQuizAnswersModel("the quiz check request is invalid");

            if (correctQuizAnswers[i].CorrectAnswer != quizAnswers[i].QuestionAnswer)
            {
                quizAnswerResponseDto.QuizResponses.Add(new QuizResponseDto
                {
                    IsQuestionAnsweredCorrectly = false,
                    QuestionAnswer = quizAnswers[i].QuestionAnswer,
                    QuestionId = quizAnswers[i].QuestionId
                });

                quizAnswerResponseDto.IsQuizCompleted = false;
            }
            else
            {
                quizAnswerResponseDto.QuizResponses.Add(new QuizResponseDto
                {
                    IsQuestionAnsweredCorrectly = true,
                    QuestionAnswer = quizAnswers[i].QuestionAnswer,
                    QuestionId = quizAnswers[i].QuestionId
                });
            }
        }

        if (quizAnswerResponseDto.IsQuizCompleted)
        {
            var task = rewardTaskService.GetAllTasks(username).Tasks.FirstOrDefault(task => task.Id == 6);

            if (task?.Status == RewardTaskStatus.Completed)
            {
                return quizAnswerResponseDto;
            }
            
            rewardTaskService.ChangeTaskStatus(username, new RewardTaskChangeDto
            {
                TaskId = 6,
                ChangedStatus = RewardTaskStatus.Uncollected,
            });
        }
        
        return quizAnswerResponseDto;
    }
    
}