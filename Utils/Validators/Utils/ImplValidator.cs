namespace ApplicationsApi.Utils.Validators.Utils;

public abstract class ImplValidator<T> : IValidator<T> where T : class {
    public abstract void MustValidate(T obj);

    public ValidationError? Validate(T obj) {
        try {
            MustValidate(obj);
            return null;
        }
        catch (ValidationException e) {
            return e.Error();
        }
    }
}