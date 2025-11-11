using System.Security.Claims;
using Enrollment.Extensions;
using Enrollment.Models.Enums;
using Enrollment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enrollment.Controllers;

[ApiController]
[Route("api/basket")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpPost("put")]
    [Authorize(Roles = UserRole.Student)]
    public async Task<ActionResult> PutBasket([FromQuery] long courseId)
    {
        var studentId = User.GetUserId();

        await _basketService.PutAsync(studentId, courseId);
        return Ok();
    }

    [HttpPost("erase")]
    [Authorize(Roles = UserRole.Student)]
    public async Task<ActionResult> EraseBasket([FromQuery] long basketId)
    {
        var studentId = User.GetUserId();

        await _basketService.EraseAsync(basketId, studentId);
        return Ok();
    }
}