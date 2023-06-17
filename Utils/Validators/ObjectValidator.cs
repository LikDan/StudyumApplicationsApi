using ApplicationsApi.Utils.Validators.Utils;

namespace ApplicationsApi.Utils.Validators;

public class ObjectValidator : ImplValidator<object> {
    public string ErrorMessage { get; set; } = "";

    private readonly IEnumerable<IValidator> _validators;

    // ReSharper disable once UnusedMember.Global
    public ObjectValidator(params IValidator[] validators) {
        _validators = validators;
    }

    public ObjectValidator(IEnumerable<IValidator> validators) {
        _validators = validators;
    }

    public override void MustValidate(object obj) {
        foreach (var validator in _validators) {
            validator.MustValidate(obj);
        }
    }
}