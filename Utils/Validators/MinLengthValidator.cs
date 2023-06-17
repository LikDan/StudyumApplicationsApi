using ApplicationsApi.Utils.Validators.Utils;

namespace ApplicationsApi.Utils.Validators;

public class MinLengthValidator : ImplValidator<string> {
    public string ErrorMessage { get; set; } = DefaultErrorMessage;

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public int MinLength { get; set; }
    
    public MinLengthValidator(int minLength) {
        MinLength = minLength;
    }

    public override void MustValidate(string obj) {
        if (obj.Length < MinLength) throw new ValidationException(this, Name, ErrorMessage, ExceptionParams);
    }
    
    public Dictionary<string, object> ExceptionParams => new() { { "minLength", MinLength } };

    public const string Name = "minLength";
    public const string DefaultErrorMessage = "min length is {{minLength}}";
}