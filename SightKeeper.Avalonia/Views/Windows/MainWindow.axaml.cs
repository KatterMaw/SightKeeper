using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Windows;

namespace SightKeeper.Avalonia.Views.Windows;

public partial class MainWindow : ReactiveWindow<MainViewModel>
{
	public MainWindow()
	{
		InitializeComponent();
	}
	
	public MainWindow(MainViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
	}
}