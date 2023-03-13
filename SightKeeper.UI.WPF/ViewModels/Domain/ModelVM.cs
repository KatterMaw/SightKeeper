﻿using System;
using System.Collections.Generic;
using ReactiveUI;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using Image = System.Drawing.Image;

namespace SightKeeper.UI.WPF.ViewModels.Domain;

public abstract class ModelVM : ReactiveObject, IModelVM
{
	protected ModelVM(Model model)
	{
		Model = model;
	}
	
	public string Name
	{
		get => Model.Name;
		set
		{
			Model.Name = value;
			this.RaisePropertyChanged();
		}
	}

	public string Description
	{
		get => Model.Description;
		set
		{
			Model.Description = value;
			this.RaisePropertyChanged();
		}
	}

	public Image? Image => Model.Image == null ? null : throw new NotImplementedException();

	public Resolution Resolution { get; private set; }
	public IEnumerable<ItemClass> Classes { get; private set; }
	public Game? Game { get; set; }
	
	
	public Model Model { get; }
}