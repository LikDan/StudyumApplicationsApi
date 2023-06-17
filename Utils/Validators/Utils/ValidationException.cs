namespace ApplicationsApi.Utils.Validators.Utils;

public class ValidationException : Exception {
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public IValidator Validator { get; }
    // ReSharper disable once MemberCanBePrivate.Global
    public string ValidatorName { get; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string MessageTemplate { get; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Dictionary<string, object> Values { get; }

    public ValidationException(IValidator validator, string validatorName, string message, Dictionary<string, object>? values = null)
        : base(ParseMessage(message, values)) {
        Validator = validator;
        ValidatorName = validatorName;
        MessageTemplate = message;
        Values = values ?? new Dictionary<string, object>();
    }

    public ValidationError Error() {
        return new ValidationError {
            Message = Message,
            Validator = ValidatorName,
        };
    }

    private static string ParseMessage(string message, Dictionary<string, object>? values = null) {
        if (values == null) return message;
        foreach (var (key, value) in values) {
            message = message.Replace($"{{{key}}}", value.ToString());
        }

        return message;
    }
}