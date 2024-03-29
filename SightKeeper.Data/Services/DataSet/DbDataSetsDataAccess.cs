﻿using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services.DataSet;

public sealed class DbDataSetsDataAccess : DataSetsDataAccess
{
    public IObservable<Domain.Model.DataSet> DataSetRemoved => _dataSetRemoved.AsObservable();
    
    public DbDataSetsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IReadOnlyCollection<Domain.Model.DataSet>> GetDataSets(CancellationToken cancellationToken) =>
        Task.Run(GetDataSets, cancellationToken);

    private IReadOnlyCollection<Domain.Model.DataSet> GetDataSets()
    {
        lock (_dbContext)
            return _dbContext.DataSets
                .Include(dataSet => dataSet.ItemClasses.OrderBy(itemClass => EF.Property<int>(itemClass, "Id")))
                .ToList();
    }

    public async Task RemoveDataSet(Domain.Model.DataSet dataSet, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await Task.Run(() =>
        {
            lock (_dbContext)
            {
                _dbContext.DataSets.Remove(dataSet);
                _dbContext.SaveChanges();
            }
        }, cancellationToken);
        _dataSetRemoved.OnNext(dataSet);
    }

    private readonly AppDbContext _dbContext;
    private readonly Subject<Domain.Model.DataSet> _dataSetRemoved = new();
}