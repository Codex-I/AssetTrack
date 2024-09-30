using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AssetTrackApi.Contracts
{
    internal class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(int code, ModelStateDictionary modelState)
            : base(new ValidationResultModel(code, modelState))
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    internal class ValidationError(int code, string field, string message)
    {
        public int Code { get; } = code;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Field { get; } = field != string.Empty ? field : null;

        public string Message { get; } = message;
    }

    internal class ValidationResultModel
    {
        public int Code { get; }

        public string Message { get; }

        public List<ValidationError> Errors { get; }

        public ValidationResultModel(int code, ModelStateDictionary modelState)
        {
            Code = code;
            Message = "Validation Failed";
            Errors = modelState.Keys
                               .SelectMany(key => modelState[key].Errors
                               .Select(x => new ValidationError(code * 10, key, x.ErrorMessage)))
                               .ToList();
        }
    }
}
