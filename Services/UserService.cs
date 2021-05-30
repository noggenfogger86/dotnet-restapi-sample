using ApiSkeleton.Common;
using ApiSkeleton.Context;
using ApiSkeleton.Entities;
using ApiSkeleton.Protocols;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ApiSkeleton.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(long id);
    }

    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly JwtToken _jwtTokenConfig;

        private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

        public UserService(IOptions<JwtToken> jwtTokenConfig,
            DataContext context)
        {
            _context = context;
            _jwtTokenConfig = jwtTokenConfig.Value;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user =
                await _context.UserDatas.Include(x => x.Roles).SingleOrDefaultAsync(x => x.Email == model.Email) ??
                throw new LogicException(ResultCode.NotFoundEmail);

            var result = _hasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (!result.Equals(PasswordVerificationResult.Success))
                throw new LogicException(ResultCode.NotMatchedPassword, result.GetDescription());

            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.UserDatas.ToListAsync();
        }

        public async Task<User> GetById(long id)
        {
            return await _context.UserDatas.FirstOrDefaultAsync(x => x.Id == id);
        }

        // helper methods

        private string GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtTokenConfig.Secret));

            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("displayname", user.NickName)
            };

            // Add roles as multiple claims
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtTokenConfig.Issuer,
                Audience = _jwtTokenConfig.Issuer
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}