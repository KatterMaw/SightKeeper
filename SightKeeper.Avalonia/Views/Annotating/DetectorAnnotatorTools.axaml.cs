﻿using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Annotating;

namespace SightKeeper.Avalonia.Views.Annotating;

public sealed partial class DetectorAnnotatorTools : ReactiveUserControl<DetectorAnnotatorToolsViewModel>
{
    public DetectorAnnotatorTools()
    {
        InitializeComponent();
    }
}