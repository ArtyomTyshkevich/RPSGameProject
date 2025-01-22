using Auth.BLL.Commands;
using Auth.BLL.DTOs.Identity;
using Auth.BLL.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Authenticate] Login attempt started for user: {Username}", request.Email);

            var query = new AuthenticateQuery { AuthRequest = request };
            var response = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("[Authenticate] Login successful for user: {Username}", request.Email);

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Register] Registration attempt started for user: {Username}", request.Email);

            var command = new RegisterCommand { RegisterRequest = request };
            var response = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("[Register] Registration successful for user: {Username}", request.Email);

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel tokenModel, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[RefreshToken] Token refresh attempt started for RefreshToken: {RefreshToken}", tokenModel.RefreshToken);

            var command = new RefreshTokenCommand { tokenModel = tokenModel };
            var newTokens = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("[RefreshToken] Token refresh successful for RefreshToken: {RefreshToken}", tokenModel.RefreshToken);

            return Ok(newTokens);
        }

        [Authorize]
        [HttpPost("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Revoke] Revoke tokens attempt started for user: {Username}", username);

            var command = new RevokeTokenCommand { Username = username };
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("[Revoke] Tokens successfully revoked for user: {Username}", username);

            return Ok();
        }

        [Authorize]
        [HttpPost("revoke-all")]
        public async Task<IActionResult> RevokeAll(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[RevokeAll] Revoke all tokens attempt started.");

            var command = new RevokeAllTokensCommand();
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("[RevokeAll] All tokens successfully revoked.");

            return Ok();
        }
    }
}