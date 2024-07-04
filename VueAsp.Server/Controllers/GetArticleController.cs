using Microsoft.AspNetCore.Mvc;
using VueAsp.Server.Models;
using VueAsp.Server.Services;
namespace VueAsp.Server.Controllers
{
    [ApiController]
    [Route("api")]
    public class GetArticleController : Controller
    {
        private readonly IGetArticleService _getArticleService;
        public GetArticleController(GetArticleService getArticleService)
        {
            _getArticleService = getArticleService;
        }
        [HttpPost("getArticle")]
        public IActionResult Index()
        {
            var articles = _getArticleService.getData();
            return Ok(new { message = "Successfully read data from database", data = articles });
        
        }
        [HttpGet("article")]
        public IActionResult GetArticle([FromHeader] int id)
        {
            var dat = _getArticleService.GetArticleAsync(id);
            if (dat == null)
            {
                return Ok(new { message = "Article Not Found" });
            }
            return Ok(new { message = "Successfully read data from database", data = dat });
        }
        [HttpPost("write")]
        public IActionResult WriteArticle([FromBody] ArticleData articleData)
        {
            if (articleData == null)
            {
                return BadRequest("Invalid data received");
            }
            _getArticleService.WriteArticleAsync(articleData);
            return Ok(new { message = "Article created successfully", articleId = articleData.Id });
        }
        [HttpPost("change")]
        public IActionResult ChangeArticle([FromBody] ArticleData articleData)
        {
            if (articleData == null)
            {
                return BadRequest("Invalid data received");
            }
            _getArticleService.ChangeArticleAsync(articleData);
            return Ok(new { message = "Article created successfully", articleId = articleData.Id });
        }
        [HttpGet("delete")]
        public IActionResult DeleteArticle([FromHeader] int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid data received");
            }
            _getArticleService.DeleteArticleAsync(id);
            return Ok(new { message = "Article created successfully", articleId = id });
        }

    }
}
