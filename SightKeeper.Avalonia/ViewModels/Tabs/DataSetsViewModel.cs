﻿using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.DataSet.Creating;
using SightKeeper.Application.DataSet.Editing;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels.Dialogs;
using SightKeeper.Avalonia.ViewModels.Dialogs.DataSet;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed partial class DataSetsViewModel : ViewModel
{
	public ReadOnlyObservableCollection<DataSetViewModel> DataSetsViewModels { get; }

	public DataSetsViewModel(ILifetimeScope scope, DataSetsListViewModel dataSetsListViewModel, DataSetsDataAccess dataSetsDataSetsDataSetsDataAccess, DataSetCreator dataSetCreator, DataSetEditor dataSetEditor)
	{
		DataSetsViewModels = dataSetsListViewModel.DataSets;
		_scope = scope;
		_dataSetsDataAccess = dataSetsDataSetsDataSetsDataAccess;
		_dataSetCreator = dataSetCreator;
		_dataSetEditor = dataSetEditor;
	}

	private readonly ILifetimeScope _scope;
	private readonly DataSetsDataAccess _dataSetsDataAccess;
	private readonly DataSetCreator _dataSetCreator;
	private readonly DataSetEditor _dataSetEditor;

	[RelayCommand]
	private async Task CreateNewDataSet(CancellationToken cancellationToken)
	{
		await using var scope = _scope.BeginLifetimeScope(this);
		var viewModel = scope.Resolve<DataSetCreatingViewModel>();
		var applied = await viewModel.ShowDialog(this);
		if (applied)
			await _dataSetCreator.CreateDataSet(new NewDataSetInfoDTO(viewModel), cancellationToken);
	}

	[RelayCommand(CanExecute = nameof(CanEditDataSet))]
	private async Task EditDataSet(CancellationToken cancellationToken)
	{
		Guard.IsNotNull(SelectedDataSetViewModel);
		var dataSetToEdit = SelectedDataSetViewModel.DataSet;
		await using var scope = _scope.BeginLifetimeScope(this);
		var viewModel = scope.Resolve<DataSetEditingViewModel>(new PositionalParameter(0, dataSetToEdit));
		var applied = await viewModel.ShowDialog(this);
		if (applied)
			await _dataSetEditor.ApplyChanges(new DataSetChangesDTO(dataSetToEdit, viewModel), cancellationToken);
	}

	private bool CanEditDataSet() => SelectedDataSetViewModel != null;

	[RelayCommand(CanExecute = nameof(CanDeleteDataSet))]
	private async Task DeleteDataSet(CancellationToken cancellationToken)
	{
		Guard.IsNotNull(SelectedDataSetViewModel);
		Guard.IsNotNull(_dataSetsDataAccess);
		await _dataSetsDataAccess.RemoveDataSet(SelectedDataSetViewModel.DataSet, cancellationToken);
		OnPropertyChanged(nameof(DataSetsViewModels));
	}

	private bool CanDeleteDataSet() => SelectedDataSetViewModel != null;

	[RelayCommand(CanExecute = nameof(CanEditWeights))]
	private async Task EditWeights()
	{
		Guard.IsNotNull(SelectedDataSetViewModel);
		var dialogViewModel = _scope.Resolve<WeightsEditorViewModel>();
		await dialogViewModel.SetLibrary(SelectedDataSetViewModel.DataSet.WeightsLibrary);
		await dialogViewModel.ShowDialog(this);
	}

	private bool CanEditWeights() => SelectedDataSetViewModel != null;

	[ObservableProperty] private DataSetViewModel? _selectedDataSetViewModel;
}