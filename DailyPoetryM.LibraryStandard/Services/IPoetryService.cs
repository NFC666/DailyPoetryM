using System.Linq.Expressions;
using DailyPoetryM.LibraryStandard.Model;

namespace DailyPoetryM.LibraryStandard.Services;

public interface IPoetryService
{
    bool isInitialize { get; }
    Task InitializeAsync();
    Task<Poetry> GetPoetryAsync(int id);
    Task<IEnumerable<Poetry>> GetPoetryListByRulesAsync(
        Expression<Func<Poetry, bool>> where, int skip, int take);

    Task CloseDbAsync();
}