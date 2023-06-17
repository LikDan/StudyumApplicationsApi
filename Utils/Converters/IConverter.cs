namespace ApplicationsApi.Utils.Converters; 

public interface IConverter<in TF, out TT> {
    TT Convert(TF from);
}