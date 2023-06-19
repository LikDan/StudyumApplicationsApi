using System.Text.Json;
using ApplicationsApi.Middlewares;
using ApplicationsApi.Models;
using ApplicationsApi.Repository;
using ApplicationsApi.Utils;
using ApplicationsApi.Utils.Converters.Serializers;
using ApplicationsApi.Utils.Parser;
using ApplicationsApi.Utils.Validators;
using ApplicationsApi.Utils.Validators.Utils;
using ApplicationsApi.Utils.Websockets;
using Microsoft.AspNetCore.Mvc;
using TimeoutException = ApplicationsApi.Utils.Parser.Utils.TimeoutException;

namespace ApplicationsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase {
    [HttpGet("{id}/[action]")]
    public void Preview(string id) {
        var user = this.Auth("createApplications");
        var template = TemplateRepository.FindById(id, user.StudyPlaceID)!;

        IWebsocket<object, object> simpleTextWebSocket =
            new HttpWebSocket<object>(HttpContext, new JsonByteSerializer<object>());

        simpleTextWebSocket.OnReceive = msg => {
            var validator = new InputValidator(template.Scheme);
            var error = validator.Validate((Dictionary<string, object>)msg);

            ApplicationPreview preview;

            try {
                var html = Parser.InstantParse(template.Template, msg, user, template.Timeout);
                preview = new ApplicationPreview {
                    Html = html,
                    Errors = error == null ? null : new[] { error }
                };
            }
            catch (TimeoutException e) {
                preview = new ApplicationPreview {
                    Errors = new[] { new ValidationError { Validator = "timeout", Message = e.Message } }
                };
            }

            simpleTextWebSocket.Send(preview);
        };

        simpleTextWebSocket.StartReceiving();
    }

    [HttpPost("{id}")]
    public Application Create(string id, JsonElement json) {
        var dataObject = Json.Deserialize(json.ToString());
        var data = (Dictionary<string, object>)dataObject;
        var user = this.Auth("createApplications");
        var template = TemplateRepository.FindById(id, user.StudyPlaceID)!;

        var validator = new InputValidator(template.Scheme);
        validator.MustValidate(data);

        var html = Parser.InstantParse(template.Template, data, user, template.Timeout);
        var cdnEntry = Http.Store(Pdf.FromHtml(html));
        var application = new Application {
            UserID = user.Id,
            TemplateID = template.Id,
            StudyPlaceID = user.StudyPlaceID,
            Html = html,
            Data = data,
            CdnEntry = cdnEntry,
        };
        ApplicationsRepository.Create(application);
        return application;
    }

    [HttpGet]
    public IEnumerable<Application> GetUsersApplications() {
        var user = this.Auth("createApplications");
        return ApplicationsRepository.List(user.StudyPlaceID, user.Id);
    }

    [HttpGet("all")]
    public IEnumerable<Application> GetAllApplications() {
        var user = this.Auth("manageApplicationsTemplates");
        return ApplicationsRepository.List(user.StudyPlaceID);
    }
}