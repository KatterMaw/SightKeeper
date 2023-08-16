﻿using CommunityToolkit.Diagnostics;
using FastYolo;
using FastYolo.Model;
using SightKeeper.Application.Scoring;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Services.Scoring.Export;
using RectangleF = System.Drawing.RectangleF;

namespace SightKeeper.Services.Scoring;

public sealed class FastYoloDetector : Detector, IDisposable
{
    public FastYoloDetector(DetectorDataSet dataSet, ModelConfig config)
    {
        throw new NotImplementedException();
        _exporter = new TempExporter();
        _itemClasses = dataSet.ItemClasses.ToDictionary(itemClass => itemClass.Name);
        //var configPath = _exporter.Export(config, new DetectorConfigParameters(dataSet));
        var weightsPath = _exporter.Export(dataSet.WeightsLibrary.Weights.Last());
        var namesPath = _exporter.Export(dataSet.ItemClasses);
        //_yoloWrapper = new YoloWrapper(configPath, weightsPath, namesPath);
    }
    
    public IReadOnlyCollection<DetectionItem> Detect(byte[] image) => ToDetectionItems(_yoloWrapper.Detect(image)).ToList();

    public void Dispose()
    {
        _yoloWrapper.Dispose();
        _exporter.Dispose();
    }

    private readonly YoloWrapper _yoloWrapper;
    private readonly Dictionary<string, ItemClass> _itemClasses;
    private readonly TempExporter _exporter;

    private IEnumerable<DetectionItem> ToDetectionItems(IEnumerable<YoloItem> items) => items.Select(ToDetectionItem);

    private DetectionItem ToDetectionItem(YoloItem item)
    {
        Guard.IsNotNull(item.Type);
        return new DetectionItem(
            _itemClasses[item.Type],
            new RectangleF(item.X, item.Y, item.Width, item.Height),
            (float)item.Confidence);
    }
}