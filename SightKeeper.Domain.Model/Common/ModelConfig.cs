﻿using ReactiveUI;
using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Common;

public abstract class ModelConfig : ReactiveObject, Entity
{
	public int Id { get; private set; }
	public string Name { get; private set; }
	public string Content { get; private set; }

	public ModelConfig(string name, string content)
	{
		Name = name;
		Content = content;
	}
}