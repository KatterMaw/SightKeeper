﻿using System.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Services.Windows;

public sealed class WindowsGamesService : GamesService
{
    public bool IsGameActive(Game game)
    {
        var process = Process.GetProcessesByName(game.ProcessName)
            .FirstOrDefault(process => process.MainWindowHandle > 0);
        if (process == null)
            return false;
        return User32.GetForegroundWindow() == process.MainWindowHandle;
    }
}