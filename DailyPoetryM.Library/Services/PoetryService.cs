using System.Linq.Expressions;
using DailyPoetryM.Library.Model;

namespace DailyPoetryM.Library.Services;

public class PoetryService : IPoetryService
{
    private readonly IPreferences _preferences;
    public PoetryService(IPreferences preferences)
    {
        _preferences = preferences;
    }
    public bool isInitialize =>
    _preferences.Get(PoetryConstant.VersionKey, 0) == PoetryConstant.Version;
    
    public Task InitializeAsync()
    {
        Console.WriteLine("Initialize");
        return Task.CompletedTask;
    }

    public Task<Poetry> GetPoetryAsync(int id)
    {
        return Task.FromResult(new Poetry());
    }

    public Task<IEnumerable<Poetry>> GetPoetryListAsync(
        Expression<Func<Poetry, bool>> where, int skip, int take)
    {
        return Task.FromResult(Enumerable.Empty<Poetry>());
    }
}

public static class PoetryConstant
{
    public const int Version = 1;
    public const string VersionKey = nameof(PoetryConstant) + "." + nameof(Version);
}