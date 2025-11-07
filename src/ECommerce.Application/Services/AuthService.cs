using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Auth;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


    

    namespace ECommerce.Application.Services;

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepo, IPasswordHasher<User> passwordHasher, IConfiguration config)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
            _config = config;
        }

        public async Task<BaseResponse<string>> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
        {
            var existing = await _userRepo.GetByEmailAsync(request.Email, ct);
            if (existing != null)
                return BaseResponse<string>.Fail("EmailAlreadyExists", "Email is already registered.");

            var dummyUser = new User(request.FirstName, request.LastName, request.Email, string.Empty, request.Phone ?? string.Empty);
            var hash = _passwordHasher.HashPassword(dummyUser, request.Password);
            dummyUser.SetPasswordHash(hash);

            await _userRepo.AddAsync(dummyUser, ct);
            await _userRepo.SaveChangesAsync(ct);

            var token = GenerateJwtToken(dummyUser);
            return BaseResponse<string>.Ok(token, "Registration successful.");
        }

        public async Task<BaseResponse<string>> LoginAsync(LoginRequest request, CancellationToken ct = default)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email, ct);
            if (user == null)
                return BaseResponse<string>.Fail("InvalidCredentials", "Invalid email or password.");

            var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (verify == PasswordVerificationResult.Failed)
                return BaseResponse<string>.Fail("InvalidCredentials", "Invalid email or password.");

            var token = GenerateJwtToken(user);
            return BaseResponse<string>.Ok(token, "Login successful.");
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSection = _config.GetSection("JwtSettings");
            var key = jwtSection.GetValue<string>("Key") ?? throw new Exception("JWT Key is missing");
            var issuer = jwtSection.GetValue<string>("Issuer");
            var audience = jwtSection.GetValue<string>("Audience");
            var expiresMinutes = jwtSection.GetValue<int>("ExpiresMinutes");

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("firstName", user.FirstName),
            new Claim("lastName", user.LastName)
        };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


