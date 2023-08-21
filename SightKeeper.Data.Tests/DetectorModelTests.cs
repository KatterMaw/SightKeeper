﻿using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class DetectorModelTests : DbRelatedTests
{
	[Fact]
	public void ShouldAddDetectorModel()
	{
		// arrange
		using var dbContext = DbContextFactory.CreateDbContext();
		var dataSet = DomainTestsHelper.NewDetectorDataSet;

		// act
		dbContext.Add(dataSet);
		dbContext.SaveChanges();

		// assert
		dbContext.DataSets.Should().Contain(dataSet);
	}

	[Fact]
	public void AddingModelWithAssetShouldAddImageScreenshotAndAsset()
	{
		using var dbContext = DbContextFactory.CreateDbContext();
		var dataSet = DomainTestsHelper.NewDetectorDataSet;
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
		var asset = dataSet.MakeAsset(screenshot);
		var itemClass = dataSet.CreateItemClass("Test item class");
		var item = asset.CreateItem(itemClass, new Bounding());
		
		dbContext.DataSets.Add(dataSet);
		dbContext.SaveChanges();

		dbContext.DataSets.Should().Contain(dataSet);
		dbContext.Set<Screenshot>().Should().Contain(screenshot);
		dbContext.Set<DetectorAsset>().Should().Contain(asset);
		dbContext.Set<DetectorItem>().Should().Contain(item);
		dbContext.Set<ItemClass>().Should().Contain(itemClass);
	}

	[Fact]
	public void ShouldCascadeDelete()
	{
		using var dbContext = DbContextFactory.CreateDbContext();
		DataSet<DetectorAsset> dataSet = new("Test model");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
		var asset = dataSet.MakeAsset(screenshot);
		var itemClass = dataSet.CreateItemClass("Test item class");
		asset.CreateItem(itemClass, new Bounding(0, 0, 1, 1));
		dbContext.DataSets.Add(dataSet);
		dbContext.SaveChanges();
		dbContext.DataSets.Remove(dataSet);
		dbContext.SaveChanges();
		dbContext.DataSets.Should().BeEmpty();
		dbContext.Set<ScreenshotsLibrary>().Should().BeEmpty();
		dbContext.Set<DetectorAsset>().Should().BeEmpty();
		dbContext.Set<Screenshot>().Should().BeEmpty();
		dbContext.Set<ItemClass>().Should().BeEmpty();
		dbContext.Set<DetectorItem>().Should().BeEmpty();
	}

	[Fact]
	public void ScreenshotShouldBeRepresentedInLibraryWhenItIsAsset()
	{
		using (var dbContext = DbContextFactory.CreateDbContext())
		{
			DataSet<DetectorAsset> dataSet = new("Test model");
			var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
			dataSet.MakeAsset(screenshot);
			dbContext.Add(dataSet);
			dbContext.SaveChanges();
		}
		using (var dbContext = DbContextFactory.CreateDbContext())
		{
			var model = dbContext.Set<DataSet<DetectorAsset>>()
				.Include(m => m.Assets)
				.Include(m => m.ScreenshotsLibrary.Screenshots)
				.Single();
			model.Assets.Should().NotBeEmpty();
			model.ScreenshotsLibrary.Screenshots.Should().NotBeEmpty();
		}
	}

	[Fact]
	public void ShouldSetGameToNullWhenDeletingGame()
	{
		var dataSet = DomainTestsHelper.NewDetectorDataSet;
		Game game = new("Test game", "game.exe");
		dataSet.Game = game;
		using var dbContext = DbContextFactory.CreateDbContext();
		dbContext.Add(dataSet);
		dbContext.SaveChanges();
		dbContext.Remove(game);
		dbContext.SaveChanges();
	}
}