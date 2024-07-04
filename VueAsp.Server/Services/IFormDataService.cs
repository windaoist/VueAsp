using VueAsp.Server.Data;
using VueAsp.Server.Models;
using Microsoft.EntityFrameworkCore;
namespace VueAsp.Server.Services
{
    public interface IFormDataService
    {
        Task<bool> CheckNameExistsAsync(string name);
        Task<FormData> GetFormDataAsync(string Account);
        // 其他需要的数据库操作方法
    }

    public class FormDataService : IFormDataService
    {
        private readonly AppDbContext _context;

        public FormDataService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckNameExistsAsync(string name)
        {
            return await _context.FormData.AnyAsync(f => f.Account == name);
        }

        public async Task<FormData> GetFormDataAsync(string Account)
        {
            return await _context.FormData.FirstOrDefaultAsync(f => f.Account == Account);
        }
        // 其他方法的实现
    }

}
