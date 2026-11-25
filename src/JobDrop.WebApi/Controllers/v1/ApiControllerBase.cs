using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobDrop.WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}