using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace ApplicationsApi.Utils;

public static class Http {
    public static void Send(byte[] bytes) {
        using var httpClient = new HttpClient();
        var form = new MultipartFormDataContent();
        httpClient.DefaultRequestHeaders.Add("Authorization", "token");

        using var stream = new MemoryStream(bytes);
        using var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        form.Add(fileContent, "file", "application.pdf");

        var response = httpClient.PostAsync("http://likdn.co/api/storage", form).Result;
        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
    }
}