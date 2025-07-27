using System.Linq.Expressions;
using DailyPoetryM.Library.Model;

namespace DailyPoetryM.Library.Services;

public interface IPoetryService
{
    bool isInitialize { get; }
    Task InitializeAsync();
    Task<Poetry> GetPoetryAsync(int id);
    Task<IEnumerable<Poetry>> GetPoetryListAsync(
        Expression<Func<Poetry, bool>> where, int skip, int take);
}