using Microsoft.AspNetCore.Mvc;

namespace CSharpAspNet.Server.Controllers;

[Route("api/[controller]")]
public abstract class Controller : ControllerBase
{
    protected readonly ILogger<Controller> Logger;
    
    protected Controller(ILogger<Controller> logger)
    {
        Logger = logger;
    }
}
