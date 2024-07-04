using VueAsp.Server.Data;
using VueAsp.Server.Models;
using Microsoft.EntityFrameworkCore;
namespace VueAsp.Server.Services
{
    public interface IGetArticleService
    {
        List<dynamic> getData();
        Task<dynamic> GetArticleAsync(int id);
        void WriteArticleAsync(ArticleData articleData);
        void ChangeArticleAsync(ArticleData articleData);
        void DeleteArticleAsync(int id);
    }
    public class GetArticleService : IGetArticleService
    {
        private readonly AppDbContext _context;

        public GetArticleService(AppDbContext context)
        {
            _context = context;
        }
        public List<dynamic> getData()
        {
            var result = _context.ArticleData.Select(t => new
            {
                id = t.Id,
                name = t.Name,
                date = t.Date,
                category = t.Category.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                content = t.Content
            }).ToList<dynamic>();
            return result;
        }
        public async Task<dynamic> GetArticleAsync(int id)
        {

            var articleData = await _context.ArticleData.FirstOrDefaultAsync(f => f.Id == id);

            if (articleData == null)
            {
                return null;
            }

            var result = new
            {
                id = articleData.Id,
                name = articleData.Name,
                date = articleData.Date,
                category = articleData.Category?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                content = articleData.Content
            };
            return result;
        }
        public async void WriteArticleAsync(ArticleData articleData)
        {

            _context.ArticleData.Add(articleData);
            await _context.SaveChangesAsync();
        }
        public async void ChangeArticleAsync(ArticleData articleData)
        {
            // 查找要更新的文章
            var existingArticle = await _context.ArticleData.FindAsync(articleData.Id);
            if (existingArticle == null)
            {
                // 处理找不到记录的情况
                throw new KeyNotFoundException($"Article with ID {articleData.Id} not found.");
            }

            // 更新文章内容
            existingArticle.Name = articleData.Name;
            existingArticle.Date = articleData.Date;
            existingArticle.Category = articleData.Category;
            existingArticle.Content = articleData.Content;

            // 保存更改
            await _context.SaveChangesAsync();
        }
        public async void DeleteArticleAsync(int id)
        {
            var article = await _context.ArticleData.FindAsync(id);
            if (article != null)
            {
                _context.ArticleData.Remove(article);
                await _context.SaveChangesAsync();

                var articles = await _context.ArticleData.OrderBy(a => a.Id).ToListAsync();
                foreach (var entity in _context.ChangeTracker.Entries())
                {
                    entity.State = EntityState.Detached;
                }

                _context.Database.ExecuteSqlRaw("DELETE FROM ArticleData");

                for (int i = 0; i < articles.Count; i++)
                {
                    articles[i].Id = i + 1;
                    _context.ArticleData.Add(articles[i]);
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}


