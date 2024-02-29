﻿using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application.Annotating;

public interface StreamDataSetScreenshoter
{
    DataSet? DataSet { get; set; }
    bool IsEnabled { get; set; }
    byte ScreenshotsPerSecond { get; set; }
}