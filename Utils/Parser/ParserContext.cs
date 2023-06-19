using System.Collections;
using ApplicationsApi.Controllers;
using ApplicationsApi.Proto;

namespace ApplicationsApi.Utils.Parser;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable NotAccessedField.Global
public class ParserContext {
    public TimeOnly Time => new();
    public DateOnly Date => new();
    public DateTime Datetime => new();

    public StudyPlace StudyPlace => _studyPlace ?? LoadStudyPlace();
    public User User { get; set; }

    public object Input;

    public ParserContext(string inputJSON, User user) : this(Json.Deserialize(inputJSON), user) { }

    public ParserContext(object input, User user) {
        Input = input;
        User = user;
    }

    public object? GetByName(string name) {
        object? obj = null;
        foreach (var s in name.Split(".")) {
            if (s == "ctx") {
                obj = this;
                continue;
            }

            if (obj == null) continue;

            var indexFragments = s.Split("[");
            var objName = indexFragments[0];
            obj = GetNestedObject(obj, objName);

            for (var i = 1; i < indexFragments.Length; i++) {
                var index = indexFragments[i][..(indexFragments[i].Length - 1)];
                obj = GetByIndex(obj, Convert.ToInt32(index));
            }
        }

        return obj;
    }

    public string GetStringByName(string name) {
        var obj = GetByName(name);
        return obj switch {
            null => "null",
            _ => Json.Serialize(obj)
        };
    }

    private StudyPlace LoadStudyPlace() {
        _studyPlace = StudyPlacesController.StudyPlace(User.StudyPlaceID);
        return _studyPlace;
    }
    
    private StudyPlace? _studyPlace;
    
    public static int GetArrayLength(object? array) {
        return ((IEnumerable<object>)array!).Count();
    }

    private static object GetByIndex(object? array, int index) {
        return ((IEnumerable<object>)array!).ToList()[index];
    }

    private static object? GetNestedObject(object? obj, string name) {
        if (obj is IDictionary dictionary) {
            return dictionary[name];
        }

        name = string.Concat(name[0].ToString().ToUpper(), name[1..]);

        var propValue = obj?.GetType().GetProperty(name)?.GetValue(obj);
        if (propValue != null) return propValue;

        var fieldValue = obj?.GetType().GetField(name)?.GetValue(obj);
        return fieldValue;
    }
}