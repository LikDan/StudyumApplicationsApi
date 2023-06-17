namespace ApplicationsApi.Utils.Converters; 

public class SimpleConverter <TF, TT> : IConverter<TF, TT> {
    private Func<TF, TT> ConvertFunc { get; }

    public SimpleConverter(Func<TF, TT> convertFunc) {
        ConvertFunc = convertFunc;
    }

    public TT Convert(TF from) => ConvertFunc(from);
}