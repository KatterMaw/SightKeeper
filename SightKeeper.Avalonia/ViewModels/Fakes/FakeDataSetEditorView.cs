﻿using SightKeeper.Application.Model;
using SightKeeper.Data;
using SightKeeper.Data.Services;
using SightKeeper.Services;
using SightKeeper.Services.Games;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeDataSetEditorView : Dialogs.DataSetDataSetEditorView
{
    public FakeDataSetEditorView() : base(new DataSetDataValidator(), new RegisteredGamesService(new DbGamesDataAccess(new AppDbContext()), new ProcessesAvailableGamesProvider(new DbGamesDataAccess(new AppDbContext()))), new DbItemClassDataAccess(new AppDbContext()))
    {
        Name = "Some model";
    }
}