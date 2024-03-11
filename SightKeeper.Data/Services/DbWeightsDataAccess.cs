﻿using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Services;

public sealed class DbWeightsDataAccess : WeightsDataAccess
{
    public DbWeightsDataAccess(AppDbContext dbContext)
    {
	    _dbContext = dbContext;
    }
    public override WeightsData LoadWeightsONNXData(Weights weights)
    {
	    throw new NotImplementedException();
    }
    public override WeightsData LoadWeightsPTData(Weights weights)
    {
	    throw new NotImplementedException();
    }

    protected override void SaveWeightsData(Weights weights, WeightsData onnxData, WeightsData ptData)
    {
	    throw new NotImplementedException();
    }
    protected override void RemoveWeightsData(Weights weights)
    {
	    throw new NotImplementedException();
    }

    private readonly AppDbContext _dbContext;
}