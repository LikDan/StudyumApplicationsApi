namespace ApplicationsApi.Utils.Converters.Serializers; 

public interface ISerializer<TF, TT> {
    TT Serialize(TF from);
    TF Deserialize(TT from);
}