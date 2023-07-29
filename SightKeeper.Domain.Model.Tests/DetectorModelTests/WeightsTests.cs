﻿using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests.DetectorModelTests;

public sealed class WeightsTests
{
    [Fact]
    public void ShouldAddWeights()
    {
        DetectorModel model = new("Model");
        var weights = model.WeightsLibrary.CreateWeights(0, Array.Empty<byte>(), Enumerable.Empty<Asset>());
        model.WeightsLibrary.Weights.Should().Contain(weights);
    }

    [Fact]
    public void ShouldNotCreateWithAssetFromDifferentModel()
    {
        DetectorModel model1 = new("Model 1");
        DetectorModel model2 = new("Model 2");
        var screenshot1 = model1.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        var screenshot2 = model2.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        var asset1 = model1.MakeAsset(screenshot1);
        var asset2 = model2.MakeAsset(screenshot2);
        Assert.Throws<ArgumentException>(() => model1.WeightsLibrary.CreateWeights(0, Array.Empty<byte>(), new List<Asset> { asset1, asset2 }));
    }
}