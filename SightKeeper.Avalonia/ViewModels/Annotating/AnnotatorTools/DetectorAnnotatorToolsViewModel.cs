﻿using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class DetectorAnnotatorToolsViewModel : ViewModel, AnnotatorTools<DetectorModel>, IDisposable
{
    public IObservable<Unit> UnMarkSelectedScreenshotAsAssetExecuted =>
        _unMarkSelectedScreenshotAsAssetExecuted.AsObservable();
    public IReadOnlyCollection<ItemClass> ItemClasses => _annotatorViewModel.SelectedModel?.ItemClasses ?? Array.Empty<ItemClass>();

    public DetectorAnnotatorToolsViewModel(AnnotatorViewModel annotatorViewModel, AnnotatorScreenshotsViewModel screenshotsViewModel, DetectorAnnotator annotator)
    {
        _annotatorViewModel = annotatorViewModel;
        _screenshotsViewModel = screenshotsViewModel;
        _annotator = annotator;
        var compositeDisposable = new CompositeDisposable();
        _disposable = compositeDisposable;
        _screenshotsViewModel.SelectedScreenshotChanged.Subscribe(OnScreenshotSelected).DisposeWith(compositeDisposable);
        annotatorViewModel.SelectedModelChanged
            .Subscribe(_ => OnPropertyChanged(nameof(ItemClasses))).DisposeWith(compositeDisposable);
    }
    
    [RelayCommand(CanExecute = nameof(CanMarkSelectedScreenshotAsAsset))]
    private void MarkSelectedScreenshotAsAsset()
    {
        var screenshot = _screenshotsViewModel.SelectedScreenshot;
        Guard.IsNotNull(screenshot);
        _annotator.MarkAsset(screenshot.Item);
        screenshot.NotifyIsAssetChanged();
    }

    private bool CanMarkSelectedScreenshotAsAsset() =>
        _screenshotsViewModel.SelectedScreenshot?.IsAsset == false;

    [RelayCommand(CanExecute = nameof(CanUnMarkSelectedScreenshotAsAsset))]
    private void UnMarkSelectedScreenshotAsAsset()
    {
        var screenshot = _screenshotsViewModel.SelectedScreenshot;
        Guard.IsNotNull(screenshot);
        _annotator.UnMarkAsset(screenshot.Item);
        _unMarkSelectedScreenshotAsAssetExecuted.OnNext(Unit.Default);
        screenshot.NotifyIsAssetChanged();
    }

    private bool CanUnMarkSelectedScreenshotAsAsset() =>
        _screenshotsViewModel.SelectedScreenshot?.IsAsset == true;

    private readonly AnnotatorViewModel _annotatorViewModel;
    private readonly AnnotatorScreenshotsViewModel _screenshotsViewModel;
    private readonly DetectorAnnotator _annotator;
    private readonly IDisposable _disposable;
    private readonly Subject<Unit> _unMarkSelectedScreenshotAsAssetExecuted = new();

    private IDisposable? _selectedScreenshotDisposable;
    [ObservableProperty] private ItemClass? _selectedItemClass;

    private void OnScreenshotSelected(ScreenshotViewModel? screenshot)
    {
        _selectedScreenshotDisposable?.Dispose();
        MarkSelectedScreenshotAsAssetCommand.NotifyCanExecuteChanged();
        UnMarkSelectedScreenshotAsAssetCommand.NotifyCanExecuteChanged();
        if (screenshot == null)
            return;
        _selectedScreenshotDisposable = screenshot.WhenAnyValue(x => x.IsAsset).Subscribe(_ =>
        {
            MarkSelectedScreenshotAsAssetCommand.NotifyCanExecuteChanged();
            UnMarkSelectedScreenshotAsAssetCommand.NotifyCanExecuteChanged();
        });
    }

    public void Dispose()
    {
        _disposable.Dispose();
        _selectedScreenshotDisposable?.Dispose();
    }
}