﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class AnnotatorScreenshotsViewModel : ViewModel
{
    public IReadOnlyCollection<Screenshot> Screenshots { get; }

    public IEnumerable<SortingRule<Screenshot>> SortingRules { get; } = new[]
    {
        new SortingRule<Screenshot>("Old first", SortDirection.Ascending, screenshot => screenshot.CreationDate),
        new SortingRule<Screenshot>("New first", SortDirection.Descending, screenshot => screenshot.CreationDate)
    };

    [ObservableProperty] private SortingRule<Screenshot> _sortingRule;

    public AnnotatorScreenshotsViewModel(ScreenshotsDataAccess screenshotsDataAccess)
    {
        _sortingRule = SortingRules.First();
        var sortingRule = this.WhenAnyValue(viewModel => viewModel.SortingRule)
            .Select(rule => rule.Comparer);
        _screenshotsDataAccess = screenshotsDataAccess;
        _screenshots.Connect()
            .Sort(sortingRule)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out var screenshots)
            .Subscribe();
        Screenshots = screenshots;
    }

    private readonly SourceList<Screenshot> _screenshots = new();
    private readonly ScreenshotsDataAccess _screenshotsDataAccess;

    [ObservableProperty] private Model? _model;
    private CompositeDisposable? _modelDisposable;

    partial void OnModelChanged(Model? value)
    {
        _modelDisposable?.Dispose();
        _screenshots.Clear();
        if (value == null)
            return;
        _screenshotsDataAccess.Load(value.ScreenshotsLibrary);
        _screenshots.AddRange(value.ScreenshotsLibrary.Screenshots);
        _modelDisposable = new CompositeDisposable();
        value.ScreenshotsLibrary.ScreenshotAdded
            .Subscribe(newScreenshot => _screenshots.Add(newScreenshot))
            .DisposeWith(_modelDisposable);
        value.ScreenshotsLibrary.ScreenshotRemoved
            .Subscribe(removedScreenshot => _screenshots.Remove(removedScreenshot))
            .DisposeWith(_modelDisposable);
    }
}