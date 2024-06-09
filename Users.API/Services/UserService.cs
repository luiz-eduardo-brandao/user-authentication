using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApplicationSecretKeys;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Users.API.DTOs.Requests;
using Users.API.DTOs.Responses;

namespace Users.API.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest registerUser) 
        {
            var user = new IdentityUser
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password); 

            if (result.Succeeded)
                await _userManager.SetLockoutEnabledAsync(user, false);

            var registerResponse = new RegisterUserResponse(result.Succeeded);

            if (!result.Succeeded && result.Errors.Any())
                registerResponse.AddErrors(result.Errors.Select(e => e.Description));

            return registerResponse;
        }

        public async Task<UserLoginResponse> LogInUserAsync(UserLoginRequest userLogin) 
        {
            var user = await _userManager.FindByEmailAsync(userLogin.Email);

            if (user == null)
                return new UserLoginResponse { Success = false, Error = "This user does not exist" };

            var result = await _signInManager.PasswordSignInAsync(user.UserName, userLogin.Password, false, true);

            var userLoginResponse = new UserLoginResponse(result.Succeeded);

            if (result.Succeeded)
                return await GenerateToken(user);

            if (result.IsLockedOut)
                userLoginResponse.AddError("This account is locked out");
            else if (result.IsNotAllowed)
                userLoginResponse.AddError("This account is not allowed");
            else if (result.RequiresTwoFactor)
                userLoginResponse.AddError("You need to confirm the login in the second factor");
            else
                userLoginResponse.AddError("Username or password are incorrect");

            return userLoginResponse;
        }

        private async Task<UserLoginResponse> GenerateToken(IdentityUser user) 
        {
            var tokenClaims = await GetClaimsAndRoles(user);

            var expirationDate = DateTime.UtcNow.AddHours(JwtSecretValidationParameters.HoursToExpiry);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecretValidationParameters.SecretKey));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: JwtSecretValidationParameters.Issuer,
                audience: JwtSecretValidationParameters.Audience,
                claims: tokenClaims.ToArray(),
                notBefore: DateTime.Now,
                expires: expirationDate,
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new UserLoginResponse 
            {
                Success = true,
                Token = token,
                ExpirationDate = expirationDate
            };   
        }

        private async Task<IList<Claim>> GetClaimsAndRoles(IdentityUser user) 
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
            // claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));
            
            foreach (var role in roles)
                claims.Add(new Claim("role", role));

            return claims;
        }
    }
}