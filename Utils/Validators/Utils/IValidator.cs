namespace ApplicationsApi.Utils.Validators.Utils;

public interface IValidator<in T> : IValidator where T : class {
    void MustValidate(T obj);
    ValidationError? Validate(T obj);
 
    void IValidator.MustValidate(object obj) {
        MustValidate((obj as T)!);
    }
 
    ValidationError? IValidator.Validate(object obj) {
        return Validate((obj as T)!);
    }
}

public interface IValidator {
    void MustValidate(object obj);
    ValidationError? Validate(object obj);
}