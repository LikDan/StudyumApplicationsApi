using System.Collections;
using ApplicationsApi.Utils.Validators.Utils;

namespace ApplicationsApi.Utils.Validators;

public class RequiredValidator : ImplValidator<object> {
    public string ErrorMessage { get; set; } = DefaultErrorMessage;

    public override void MustValidate(object obj) {
        switch (obj) {
            case string s when s.Trim() == "":
            case null or 0 or 0D or 0F or 0L or (short)0 or decimal.MaxValue or decimal.Zero:
            case IEnumerable source when !source.Cast<object>().Any():
                throw new ValidationException(this, Name, ErrorMessage);
        }
    }

    public const string Name = "required";
    public const string DefaultErrorMessage = "field is required";
}