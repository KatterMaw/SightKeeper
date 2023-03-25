﻿using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface GamesRegistrator
{
	IReadOnlyCollection<Game> AvailableGames { get; }
	IReadOnlyCollection<Game> RegisteredGames { get; }

	bool IsGameRegistered(Game game);
	void RegisterGame(Game game);
	void UnregisterGame(Game game);
	void RefreshAvailableGames();
}