﻿using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model;

public class ScreenshotsLibrary
{
    public IObservable<Screenshot> ScreenshotAdded => _screenshotAdded.AsObservable();
    public IObservable<Screenshot> ScreenshotRemoved => _screenshotRemoved.AsObservable();
    public ushort? MaxQuantity { get; set; }
    public IReadOnlyCollection<Screenshot> Screenshots => _screenshots;
    public bool HasAnyScreenshots { get; private set; }

    public ScreenshotsLibrary()
    {
        _screenshots = new List<Screenshot>();
    }

    public Screenshot CreateScreenshot(byte[] content)
    {
        Screenshot screenshot = new(this, content);
        _screenshots.Add(screenshot);
        ClearExceed();
        HasAnyScreenshots = Screenshots.Any();
        _screenshotAdded.OnNext(screenshot);
        return screenshot;
    }
	
    public void DeleteScreenshot(Screenshot screenshot)
    {
        if (!_screenshots.Remove(screenshot))
            ThrowHelper.ThrowInvalidOperationException("Screenshot not found");
        HasAnyScreenshots = Screenshots.Any();
        _screenshotRemoved.OnNext(screenshot);
    }
	
    private readonly List<Screenshot> _screenshots;
    private readonly Subject<Screenshot> _screenshotAdded = new();
    private readonly Subject<Screenshot> _screenshotRemoved = new();

    private void ClearExceed()
    {
        if (MaxQuantity == null)
            return;
        var screenshotsToDelete = Screenshots
            .Where(screenshot => screenshot.Asset == null)
            .OrderByDescending(screenshot => screenshot.CreationDate)
            .Skip(MaxQuantity.Value)
            .ToList();
        foreach (var screenshot in screenshotsToDelete)
            DeleteScreenshot(screenshot);
    }
}