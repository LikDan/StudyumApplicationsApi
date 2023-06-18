using ApplicationsApi.Middlewares;
using ApplicationsApi.Models;
using ApplicationsApi.Repository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ApplicationsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationTemplatesController : ControllerBase {
    [HttpGet]
    public IEnumerable<ApplicationTemplatePreview> Index() {
        var user = this.Auth();
        return TemplateRepository.PreviewList(user.StudyPlaceID);
    }

    [HttpGet("{id}")]
    public ApplicationTemplate? Show(string id) {
        var user = this.Auth();
        return TemplateRepository.FindById(id, user.StudyPlaceID);
    }

    [HttpPost]
    public void Create(ApplicationTemplate application) {
        var user = this.Auth("manageApplicationsTemplates");
        application.StudyPlaceID = user.StudyPlaceID;
        application.Id = ObjectId.GenerateNewId().ToString()!;
        TemplateRepository.Create(application);
    }

    [HttpDelete("{id}")]
    public void Delete(string id) {
        var user = this.Auth("manageApplicationsTemplates");
        TemplateRepository.Delete(id);
    }
}