using CommunityToolkit.Mvvm.ComponentModel;
using DailyPoetryM.LibraryStandard.Model;
using DailyPoetryM.LibraryStandard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TheSalLab.MauiInfiniteScrolling;

namespace DailyPoetryM.LibraryStandard.ViewModels;

public class ResultPageViewModel : ObservableObject
{
    private IPoetryService _poetryService;
    public Expression<Func<Poetry, bool>> _where;
    public Expression<Func<Poetry, bool>> Where
    {
        get => _where;
        set => SetProperty(ref _where, value);
    }

    public string _stauts;
    public string Status
    {
        get => _stauts;
        set => SetProperty(ref _stauts, value);
    }

    public MauiInfiniteScrollCollection<Poetry> Poetries { get;  }

    public ResultPageViewModel(IPoetryService poetryService)
    { 
        _poetryService = poetryService;
        Poetries = new MauiInfiniteScrollCollection<Poetry>()
        {
            OnCanLoadMore = () => true,  
            OnLoadMore = async () =>
            {
                return await poetryService.GetPoetryListByRulesAsync(
                    Where, Poetries.Count, PageSize);
                
            }
        };
    }
    public const int PageSize = 10;
    public const string Loading = "正在加载";
    public const string NoMore = "没有更多了";
    public const string NoResult = "没有满足条件的结果";
}