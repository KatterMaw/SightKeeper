﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using FluentValidation.Results;

namespace SightKeeper.Avalonia.ViewModels;

public abstract class ValidatableViewModel<TValidatable> : ViewModel, INotifyDataErrorInfo where TValidatable : class
{
    public IValidator<TValidatable> Validator { get; }
    
    public ValidatableViewModel(IValidator<TValidatable> validator)
    {
        Validator = validator;
        PropertyChanged += OnPropertyChanged;
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            return ValidationResult.Errors.Select(error => error.ErrorMessage);
        return ValidationResult.Errors.Where(error => error.PropertyName == propertyName)
            .Select(error => error.ErrorMessage);
    }

    public void SetValidationResult(ValidationResult validationResult)
    {
        var propertiesToValidate = ValidationResult.Errors.Select(error => error.PropertyName)
            .Union(validationResult.Errors.Select(error => error.PropertyName)).ToList();
        ValidationResult = validationResult;
        foreach (var property in propertiesToValidate)
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(property));
    }

    public bool HasErrors => !ValidationResult.IsValid;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    
    protected ValidationResult ValidationResult = new();

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var validatable = this as TValidatable;
        Guard.IsNotNull(validatable);
        var validationResult = Validator.ValidateAsync(validatable).GetAwaiter().GetResult();
        SetValidationResult(validationResult);
    }
}