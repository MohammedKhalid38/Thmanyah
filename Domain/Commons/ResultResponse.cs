using Domain.IdentityModels;
using Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections;

namespace Domain.Commons;

public class ResultResponse
{
    public bool Success { get; set; }
    public string Status { get; set; } = null!;
    public string Message { get; set; } = null!;
    public Hashtable? Errors { get; set; }
    public BaseEntity? Model { get; set; }
    public object? Object { get; set; }
    public Role? Role { get; set; }
    public User? User { get; set; }
    public static ResultResponse Set(bool success, string message) => new ResultResponse { Success = success, Status = success ? Resources.Success : Resources.Error, Message = message };
    public static ResultResponse Set(bool success, string message, BaseEntity? model) => new ResultResponse { Success = success, Status = success ? Resources.Success : Resources.Error, Message = message, Model = model };
    public static ResultResponse Set(bool success, string message, ModelStateDictionary modelState)
    {
        var errors = new Hashtable();
        foreach (var pair in modelState)
            if (pair.Value.Errors.Count > 0)
                errors[pair.Key] = pair.Value.Errors.Select(error => error.ErrorMessage).ToList();

        return new ResultResponse { Success = success, Status = success ? Resources.Success : Resources.Error, Message = message, Errors = errors };
    }
}
