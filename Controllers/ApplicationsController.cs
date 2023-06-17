using ApplicationsApi.Middlewares;
using ApplicationsApi.Models;
using ApplicationsApi.Proto;
using ApplicationsApi.Repository;
using ApplicationsApi.Utils.Converters.Serializers;
using ApplicationsApi.Utils.Parser;
using ApplicationsApi.Utils.Validators;
using ApplicationsApi.Utils.Validators.Utils;
using ApplicationsApi.Utils.Websockets;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TimeoutException = ApplicationsApi.Utils.Parser.Utils.TimeoutException;

namespace ApplicationsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase {
    [HttpGet]
    public IEnumerable<ApplicationTemplatePreview> Index() {
        var user = this.Auth();
        return Database.TemplatesCollection
            .Find(t => t.StudyPlaceID == user.StudyPlaceID)
            .ToEnumerable()
            .Select(v => v.Preview());
    }

    [HttpGet("{id}")]
    public ApplicationTemplate Show(string id) {
        return Database.TemplatesCollection.Find(t => t.Id == id).First();
    }

    [HttpGet("{id}/[action]")]
    public void Preview(string id) {
        var template = TemplateRepository.FindById(id);

        IWebsocket<object, object> simpleTextWebSocket =
            new HttpWebSocket<object>(HttpContext, new JsonByteSerializer<object>());

        simpleTextWebSocket.OnReceive = msg => {
            var validator = new InputValidator(template.Scheme);
            var error = validator.Validate((Dictionary<string, object>)msg);

            ApplicationPreview preview;

            try {
                var task = Parser.InstantParse(template.Template, msg, template.Timeout);
                preview = new ApplicationPreview {
                    Html = task,
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
}