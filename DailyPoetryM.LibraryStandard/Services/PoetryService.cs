using System.Linq.Expressions;
using DailyPoetryM.LibraryStandard.Model;
using SQLite;

namespace DailyPoetryM.LibraryStandard.Services;


public class PoetryService : IPoetryService , IDisposable
{
    private const string _dbName = "poetrydb.sqlite3";
    public static readonly string DbPath = Path.Combine(
        Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData), _dbName);
    
    private SQLiteAsyncConnection _db; 
    private SQLiteAsyncConnection Db => _db ??= new SQLiteAsyncConnection((DbPath));
    
    private readonly IPreferenceStorage _preferences;
    public PoetryService(IPreferenceStorage preferences)
    {
        _preferences = preferences;
    }
    public bool isInitialize =>
        _preferences.Get(PoetryConstant.VersionKey, 0) == PoetryConstant.Version;
    
    public async Task InitializeAsync()
    {
        await using var dbFileStream = new FileStream(DbPath, FileMode.OpenOrCreate);
        await using var dbAssetStream = typeof(PoetryService).Assembly
            .GetManifestResourceStream("poetrydb.sqlite3") ?? throw new Exception(_dbName+"数据库文件不存在");
        await dbAssetStream.CopyToAsync(dbFileStream);
        _preferences.Set(PoetryConstant.VersionKey, PoetryConstant.Version);
    }

    public async Task<Poetry> GetPoetryAsync(int id)
    {
        
        var res = await Db.Table<Poetry>().FirstOrDefaultAsync(poetry => poetry.Id == id);
        return res;
    }

    public async Task<IEnumerable<Poetry>> GetPoetryListByRulesAsync(
        Expression<Func<Poetry, bool>> where, int skip, int take)
    {
        var res = await Db.Table<Poetry>().Where(where).Skip(skip).Take(take).ToListAsync();
        return res;
    }

    public async Task CloseDbAsync()
    {
        await Db?.CloseAsync();
    }

    public void Dispose()
    {
        Db?.CloseAsync().Wait();
    }
}

public static class PoetryConstant
{
    public const int Version = 1;
    public const string VersionKey = nameof(PoetryConstant) + "." + nameof(Version);
}