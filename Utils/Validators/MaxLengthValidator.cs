using ApplicationsApi.Utils.Validators.Utils;

namespace ApplicationsApi.Utils.Validators;

public class MaxLengthValidator : ImplValidator<string> {
    public string ErrorMessage { get; set; } = DefaultErrorMessage;

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public int MaxLength { get; set; }


    public MaxLengthValidator(int maxLength) {
        MaxLength = maxLength;
    }

    public override void MustValidate(string obj) {
        if (obj.Length > MaxLength) throw new ValidationException(this, Name, ErrorMessage, ExceptionParams);
    }

    public Dictionary<string, object> ExceptionParams => new() { { "maxLength", MaxLength } };

    public const string Name = "maxLength";
    public const string DefaultErrorMessage = "max length is {{maxLength}}";
}