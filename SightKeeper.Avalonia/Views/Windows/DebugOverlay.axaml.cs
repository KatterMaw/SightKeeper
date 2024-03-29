﻿using Avalonia.Controls;
using CommunityToolkit.Diagnostics;
using SightKeeper.Services.Windows;

namespace SightKeeper.Avalonia.Views.Windows;

public partial class DebugOverlay : Window
{
    public DebugOverlay()
    {
        InitializeComponent();
        var platformHandle = TryGetPlatformHandle();
        Guard.IsNotNull(platformHandle);
        User32.MakeWindowClickThrough(platformHandle.Handle);
    }

    public string Text
    {
        get => TextBlock.Text ?? string.Empty;
        set => TextBlock.Text = value;
    }
}