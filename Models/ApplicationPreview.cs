using ApplicationsApi.Utils.Validators.Utils;

namespace ApplicationsApi.Models; 

public class ApplicationPreview {
    public string Html { get; set; } = "";
    public ValidationError[]? Errors { get; set; } = Array.Empty<ValidationError>();
}

