using Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpDownCryptorollBackend.Filters.FilterAttributes;
using UpDownCryptorollBackend.MapperConfiguration;
using UpDownCryptorollBackend.Models.Quiz;

namespace UpDownCryptorollBackend.Controllers;

[ApiController]
[Route("api/quiz")]
[Authorize]
[UsernameAuthorization]
public class QuizController(
    QuizService quizService,
    CustomMapper customMapper) : ControllerBase
{
    [HttpPost]
    public IActionResult CheckQuiz(string username, [FromBody] List<QuizCheckModel> quizAnswers)
    {
        return Ok(quizService.CheckQuizAnswers(username, customMapper.ToQuizCheckDtos(quizAnswers)));
    }
}