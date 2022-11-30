﻿using FluentAssertions;
using SightKeeper.DAL;
using SightKeeper.DAL.Members.Common;

namespace SightKeeper.Tests.DAL;

[ManageDatabase]
public sealed class GameTests
{
	[Fact]
	public void ShouldAddGame()
	{
		// arrange
		using AppDbContext dbContext = Helper.NewDbContext;
		Game testGame = TestGame;
		
		// act
		dbContext.Add(testGame);
		dbContext.SaveChanges();
		
		// assert
		dbContext.Games.Should().Contain(testGame);
	}

	[Fact]
	public void ShouldDeleteGame()
	{
		// arrange
		using AppDbContext dbContext = Helper.NewDbContext;
		Game testGame = TestGame;
		dbContext.Add(testGame);
		dbContext.SaveChanges();
		dbContext.Games.Should().Contain(testGame);
		
		// act
		dbContext.Remove(testGame);
		dbContext.SaveChanges();
		
		// assert
		dbContext.Games.Should().NotContain(testGame);
	}

	[Fact]
	public void ShouldGetGame()
	{
		// arrange
		using AppDbContext dbContext = Helper.NewDbContext;
		Game testGame = TestGame;
		dbContext.Add(testGame);
		dbContext.SaveChanges();
		dbContext.Games.Should().Contain(testGame);
		
		// act
		Game gameFromDb = dbContext.Games.Single();
		
		// assert
		gameFromDb.Should().Be(testGame);
	}

	private static Game TestGame => new("TestGame", "process.exe");
}