
using System.Linq.Expressions;
using DailyPoetryM.LibraryStandard;
using DailyPoetryM.LibraryStandard.Model;
using DailyPoetryM.LibraryStandard.Services;
using Moq;

namespace DailyPoetryM.Test.Services;

public class PoetryServiceTest : IDisposable
{

    public PoetryServiceTest()
    {
        File.Delete(PoetryService.DbPath);
    }
    //isInitialized的测试
    [Fact] 
    public void TestIsInitialized_Default()
    {
        
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        preferenceStorageMock
            .Setup(p => p.Get(PoetryConstant.VersionKey, 0))
            .Returns(PoetryConstant.Version);
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryService = new PoetryService(mockPreferenceStorage);
        Assert.True(poetryService.isInitialize);
    }

    
    [Fact]
    public async Task TestInitializedAsync_Default()
    {
        Assert.False(File.Exists(PoetryService.DbPath));
        var poetryService = await GetInitializedPoetryService();
        Assert.True(File.Exists(PoetryService.DbPath));
    }

    [Fact]
    public async Task TestGetPoetryAsync_Default()
    {
        var poetryService = await GetInitializedPoetryService();
        var poetry1 = await poetryService.GetPoetryAsync(10001);
        await poetryService.CloseDbAsync();
        
        var poetryService2 = await GetInitializedPoetryService();
        await poetryService2.InitializeAsync();
        var poetry2 = await poetryService2.GetPoetryAsync(10001);
        await poetryService2.CloseDbAsync();
        Assert.NotNull(poetry1);
        Assert.NotNull(poetry2);

    }

    [Fact]
    public async Task TestDb_Default()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;
        using var poetryService = new PoetryService(mockPreferenceStorage);
        await poetryService.CloseDbAsync();
        return;
    }
    
    
    [Fact]
    public async Task TestGetPoetryListByRulesAsync_Default()
    {
        var poetryService = await GetInitializedPoetryService();

        var res = await poetryService.GetPoetryListByRulesAsync(
            Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
                Expression.Parameter(typeof(Poetry), "p")), 0, int.MaxValue);

        await poetryService.CloseDbAsync();
        Assert.Equal(30, res.Count());
        
    }

    public static async Task<PoetryService> GetInitializedPoetryService()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;
        using var poetryService = new PoetryService(mockPreferenceStorage);
        await poetryService.InitializeAsync();
        return poetryService;
    }
    
    public void Dispose()
    {
        File.Delete(PoetryService.DbPath);
    }
}