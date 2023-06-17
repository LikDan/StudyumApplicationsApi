namespace ApplicationsApi.Utils.Converters.Serializers;

public abstract class ImplSerializer<TF, TT> : ISerializer<TF, TT> {
    public TT Serialize(TF from) => SerializeConverter().Convert(from);
    public TF Deserialize(TT from) => DeserializeConverter().Convert(from);

    public abstract IConverter<TF, TT> SerializeConverter();
    public abstract IConverter<TT, TF> DeserializeConverter();
}