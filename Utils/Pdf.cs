using iText.Html2pdf;

namespace ApplicationsApi.Utils;

public static class Pdf {
    public static byte[] FromHtml(string html) {
        using var memoryStream = new MemoryStream();
        HtmlConverter.ConvertToPdf(html, memoryStream);
        return memoryStream.ToArray();
    }
}