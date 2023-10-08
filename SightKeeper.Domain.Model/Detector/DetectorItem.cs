﻿using FlakeId;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public sealed class DetectorItem
{
	public Id Id { get; private set; }
	public Asset Asset { get; private set; }
	public ItemClass ItemClass { get; set; }
	public Bounding Bounding { get; private set; }
	
	internal DetectorItem(Asset asset, ItemClass itemClass, Bounding bounding)
	{
		Asset = asset;
		ItemClass = itemClass;
		Bounding = bounding;
	}
	
	private DetectorItem()
	{
		Asset = null!;
		ItemClass = null!;
		Bounding = null!;
	}
}