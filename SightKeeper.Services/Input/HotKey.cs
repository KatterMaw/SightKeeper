﻿using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SightKeeper.Services.Input;

public sealed class HotKey
{
    public Gesture Gesture { get; }
    public bool IsPressed { get; internal set; }

    public HotKey(Gesture gesture)
    {
        Gesture = gesture;
    }
    
    internal IObservable<HotKey> Pressed => _pressed.AsObservable();
    internal IObservable<Unit> Released => _released.AsObservable();

    internal void InvokeActions() => Task.Run(() => _pressed.OnNext(this));

    private readonly Subject<HotKey> _pressed = new();
    private readonly Subject<Unit> _released = new();
}