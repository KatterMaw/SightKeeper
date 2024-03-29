﻿using System.Collections.Immutable;
using System.Drawing;
using System.Numerics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Serilog;
using SharpHook.Native;
using SightKeeper.Application;
using SightKeeper.Application.Prediction;
using SightKeeper.Commons;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Services.Input;

namespace SightKeeper.Services.Prediction.Handling.MouseMoving;

public sealed class MouseMoverDetectionHandler : DetectionObserver, IDisposable
{
    public IObservable<float?> RequestedProbabilityThreshold { get; }
    
    public MouseMoverDetectionHandler(Profile profile, DetectionMouseMover mouseMover, SharpHookHotKeyManager hotKeyManager, ProfileEditor profileEditor)
    {
        _profile = profile;
        _mouseMover = mouseMover;
        _profileItemClasses = profile.ItemClasses.ToDictionary(
            profileItemClass => profileItemClass.ItemClass,
            profileItemClass => profileItemClass);
        if (profile.ItemClasses.Any(itemClass => itemClass.ActivationCondition != ItemClassActivationCondition.None))
        {
            hotKeyManager.Register(MouseButton.Button1, 
                    () => _isFiring = true,
                    () => _isFiring = false)
                .DisposeWithEx(_constructorDisposables);
        }
        
        RequestedProbabilityThreshold = profileEditor.ProfileEdited
            .Where(editedProfile => editedProfile == profile)
            .Select(editedProfile => (float?)editedProfile.DetectionThreshold);
    }

    public void OnNext(DetectionData data)
    {
        if (_disposed)
            return;
        var suitableItems = WhereSuitable(data.Items).ToImmutableList();
        if (!suitableItems.Any())
            return;
        var mostSuitableItem = suitableItems.MinBy(GetItemOrder);
        MoveTo(data, mostSuitableItem);
    }

    public void OnPaused()
    {
        _mouseMover.OnPaused();
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _constructorDisposables.Dispose();
        _disposed = true;
    }

    private readonly Profile _profile;
    private readonly Dictionary<ItemClass, ProfileItemClass> _profileItemClasses;
    private readonly DetectionMouseMover _mouseMover;
    private readonly CompositeDisposable _constructorDisposables = new();
    private bool _isFiring;
    private bool _disposed;
    
    private IEnumerable<DetectionItem> WhereSuitable(IEnumerable<DetectionItem> items)
    {
        return items
            .Where(item => item.Probability >= _profile.DetectionThreshold && _profileItemClasses.ContainsKey(item.ItemClass))
            .Where(IsFiringModePassing);
    }
    
    private bool IsFiringModePassing(DetectionItem item)
    {
        var profileItemClass = _profileItemClasses[item.ItemClass];
        if (profileItemClass.ActivationCondition == ItemClassActivationCondition.None)
            return true;
        return (profileItemClass.ActivationCondition == ItemClassActivationCondition.IsShooting) == _isFiring;
    }
    
    private float GetItemOrder(DetectionItem item)
    {
        var distance = GetNormalizedDistanceTo(item.Bounding);
        var itemClassIndex = _profileItemClasses[item.ItemClass].Index;
        var order = distance + itemClassIndex;
        Log.Debug("Order of item {Item} is {Order} (Distance: {Distance}, Item class index: {ItemClassIndex})",
            item, order, distance, itemClassIndex);
        return order;
    }
    
    private static float GetNormalizedDistanceTo(RectangleF rectangle) => GetMoveVector(rectangle).Length();

    private void MoveTo(DetectionData data, DetectionItem item)
    {
        var moveVector = GetMoveVector(item.Bounding);
        moveVector = moveVector * _profile.Weights.Library.DataSet.Resolution / 2;
        moveVector *= _profile.MouseSensitivity;
        _mouseMover.Move(new MouseMovingContext(data, item), moveVector);
    }

    private static Vector2 GetMoveVector(RectangleF rectangle)
    {
        var center = new Vector2(0.5f, 0.5f);
        var pos = new Vector2((rectangle.Left + rectangle.Right) / 2, (rectangle.Top + rectangle.Bottom) / 2);
        var difference = pos - center;
        return difference;
    }
}