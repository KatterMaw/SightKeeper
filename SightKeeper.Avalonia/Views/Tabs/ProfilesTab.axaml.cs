﻿using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Tabs;

namespace SightKeeper.Avalonia.Views.Tabs;

public partial class ProfilesTab : ReactiveUserControl<ProfilesTabVM>
{
	public ProfilesTab()
	{
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}