﻿using System.Reactive;
using System.Reactive.Linq;

namespace SightKeeper.Application.Extensions;

public static class ReactiveExtensions
{
    public static IObservable<T> WhereNotNull<T>(this IObservable<T?> observable) =>
        observable.Where(item => item != null).Select(item => (T)item!);

    public static IObservable<T> WhereNotNull<T>(this IObservable<T?> observable) where T : struct =>
        observable.Where(item => item != null).Select(item => item!.Value);

    public static IObservable<T> IgnoreCompletion<T>(this IObservable<T> observable) => observable
        .Materialize()
        .Where(notification => notification.Kind != NotificationKind.OnCompleted)
        .Dematerialize();
}