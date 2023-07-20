﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Core.Interfaces;
using Movies.Core.Models.Auth;
using Movies.Core.Models.Factories;

namespace MoviesApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly IMapper _mapper;
		private IResponseFactory _successFactory;
		private IResponseFactory _failureFactory;
		private readonly IResponseFactory _unAuthorizedFactory;


		public AuthController(IAuthService authService, IMapper mapper)
		{
			_authService = authService;
			_mapper = mapper;
			_unAuthorizedFactory = new UnAuthorizedFailureResponseFactory();
		}


		[HttpPost("register")]
		public async Task<IActionResult> RegisterAsync(UserRegisterDto dto)
		{
			var result = await _authService.RegisterAsync(dto);

			if (!result.IsAuthed)
			{

				_failureFactory = new FailureResponseFactory(400, result.Message);
				return BadRequest(_failureFactory.CreateResponse());
			}

			SetRefreshTokenToCookies(result.RefreshToken!, result.RefreshTokenExpiration);
			_successFactory = new SuccessResponseFactory<AuthModel>(200, result);
			return Ok(_successFactory.CreateResponse());
		}


		[HttpPost("login")]
		public async Task<IActionResult> LoginAsync(UserLoginDto dto)
		{
			var result = await _authService.LoginAsync(dto);

			if (!result.IsAuthed)
			{
				_failureFactory = new FailureResponseFactory((int)HttpStatusCode.Unauthorized, result.Message);
				return Unauthorized(_failureFactory.CreateResponse());
			}

			SetRefreshTokenToCookies(result.RefreshToken , result.RefreshTokenExpiration);
			_successFactory = new SuccessResponseFactory<AuthModel>(200, result);
			return Ok(_successFactory.CreateResponse());
		}


		private void SetRefreshTokenToCookies(string refreshToken, DateTime expiresOn)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = expiresOn
			};

			Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
		}
	}
}
