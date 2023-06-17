using ApplicationsApi.Models;
using ApplicationsApi.Utils.Validators.Utils;

namespace ApplicationsApi.Utils.Validators;

public class InputValidator : ImplValidator<Dictionary<string, object>> {
    public string ErrorMessage { get; set; } = "";

    private readonly InputScheme[] _scheme;

    public InputValidator(InputScheme[] scheme) {
        _scheme = scheme;
    }

    public override void MustValidate(Dictionary<string, object> inputs) {
        AssertRequired(inputs);

        foreach (var (key, value) in inputs) {
            var scheme = _scheme.FirstOrDefault(s => s.Parameter == key);
            if (scheme == null) continue;
            var validator = SchemeToObjectValidator(scheme);
            validator.MustValidate(value);
        }
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public void AssertRequired(Dictionary<string, object> inputs) {
        foreach (var scheme in _scheme) {
            var validator = scheme.Validators.FirstOrDefault(v => v.Name == RequiredValidator.Name);
            if (validator == null) continue;

            var value = inputs.FirstOrDefault(i => i.Key == scheme.Parameter).Value;
            Validators["required"](null, validator.NotValidMessage).MustValidate(value);
        }
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static ObjectValidator SchemeToObjectValidator(InputScheme scheme) {
        var validators = scheme.Validators.Select(v => Validators[v.Name](new[] { v.Value }, v.NotValidMessage));
        return new ObjectValidator(validators);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly Dictionary<string, Func<object[]?, string, IValidator>> Validators = new() {
        {
            RequiredValidator.Name, (_, errMsg) => new RequiredValidator { ErrorMessage = errMsg }
        }, {
            MinLengthValidator.Name, (args, errMsg) => new MinLengthValidator((int)args[0]) { ErrorMessage = errMsg }
        }, {
            MaxLengthValidator.Name, (args, errMsg) => new MaxLengthValidator((int)args[0]) { ErrorMessage = errMsg }
        },
    };
}