using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using ApplicationSecretKeys;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Users.API.DTOs.Requests;
using Users.API.DTOs.Responses;
using Users.API.Models;

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
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password); 

            if (result.Succeeded) 
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var verificationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(verificationCode))
                    return new RegisterUserResponse {
                        Success = false,
                        Errors = ["Email confirmation code invalid. Try again"]
                    };

                return new RegisterUserResponse 
                {
                    Success = true,
                    EmailConfirmation = new UserEmailConfirmation(userId, user.Email, verificationCode)
                };
            }
                
            var registerResponse = new RegisterUserResponse(result.Succeeded);

            if (!result.Succeeded && result.Errors.Any())
                registerResponse.AddErrors(result.Errors.Select(e => e.Description));

            return registerResponse;
        }

        // public async Task SendNewVerificationCode(string userId) 
        // {
            // var verificationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // return new RegisterUserResponse 
            // {
            //     Success = true,
            //     UserId = userId,
            //     VerificationCode = verificationCode
            // };
        // }

        public async Task<RegisterUserResponse> SendConfirmationEmail(UserEmailConfirmation emailConfirmation, string callbackUrl) 
        {
            var user = await _userManager.FindByIdAsync(emailConfirmation.UserId);

            if (user == null)
                return new RegisterUserResponse { Success = false, Errors = ["User not found"] };

            var emailBody = GenerateBodyConfirmationEmail(callbackUrl);

            await SendEmail(callbackUrl);

            return new RegisterUserResponse {
                Success = true,
                Message = $"Verification code sent to the e-mail: <strong>{emailConfirmation.Email}</strong>. Please confirm before login"
            };
        }

        public async Task<EmailConfirmationResponse> ConfirmUserEmailAsync(string userId, string verificationCode) 
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return new EmailConfirmationResponse { Success = false, Error = "This user does not exist" };

            var result = await _userManager.ConfirmEmailAsync(user, verificationCode);

            var emailConfirmationResponse = new EmailConfirmationResponse(result.Succeeded);

            if (emailConfirmationResponse.Success) 
                await _userManager.SetLockoutEnabledAsync(user, false);
            
            if (result.Errors.Any()) 
                emailConfirmationResponse.AddError(result.Errors.Select(e => e.Description)
                    .FirstOrDefault() ?? "Invalid email confirmation");

            return emailConfirmationResponse;
        }

        public async Task<UserLoginResponse> LogInUserAsync(UserLoginRequest userLogin) 
        {
            var user = await _userManager.FindByEmailAsync(userLogin.Email);

            if (user == null)
                return new UserLoginResponse { Success = false, Error = "This user does not exist" };

            var result = await _signInManager.PasswordSignInAsync(user.UserName, userLogin.Password, true, true);

            var userLoginResponse = new UserLoginResponse(result.Succeeded);

            if (!await _userManager.IsEmailConfirmedAsync(user)) 
            {
                userLoginResponse.AddError("You need to confirm your email first to login");

                return userLoginResponse;
            }

            if (result.Succeeded)
                return await GenerateCredentials(user);

            else if (result.IsLockedOut)
                userLoginResponse.AddError("This account is locked out");
            else if (result.IsNotAllowed)
                userLoginResponse.AddError("This account is not allowed");
            else if (result.RequiresTwoFactor)
                userLoginResponse.AddError("You need to confirm the login in the second factor");
            else
                userLoginResponse.AddError("Username or password are incorrect");

            return userLoginResponse;
        }

        public async Task<UserLoginResponse> RefreshLogInAsync(string userId) 
        {
            var userLoginResponse = new UserLoginResponse(true);

            var user = await _userManager.FindByIdAsync(userId);

            if (await _userManager.IsLockedOutAsync(user))
                userLoginResponse.AddError("This account is locked");
            else if (!await _userManager.IsEmailConfirmedAsync(user))
                userLoginResponse.AddError("This account needs to confirm the e-mail before Login");

            if (userLoginResponse.Success)
                return await GenerateCredentials(user);

            return userLoginResponse;
        }

        private string GenerateBodyConfirmationEmail(string callbackUrl) 
        {
            var emailBody = $"Please confirm your email address <a href=\"#URL#\"> Click here </a> ";

            var body = emailBody.Replace("#URL#", HtmlEncoder.Default.Encode(callbackUrl));

            return body;
        }

        private async Task SendEmail(string emailBody) 
        {

        }

        private async Task<UserLoginResponse> GenerateCredentials(IdentityUser user)  
        {
            var accessTokenClaims = await GetClaimsAndRoles(user);
            var refreshTokenClaims = await GetClaimsAndRoles(user, addUserClaims: false);

            var expirationDateAccessToken = DateTime.UtcNow.AddSeconds(JwtSecretValidationParameters.AccessTokenExpiration);
            var expirationDateRefreshToken = DateTime.UtcNow.AddSeconds(JwtSecretValidationParameters.RefreshTokenExpiration);

            var accessToken = GenerateToken(accessTokenClaims, expirationDateAccessToken);
            var refreshToken = GenerateToken(refreshTokenClaims, expirationDateRefreshToken);

            return new UserLoginResponse 
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpirationDate = expirationDateAccessToken
            };   
        }

        private string GenerateToken(IList<Claim> claims, DateTime expirationDate) 
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecretValidationParameters.SecretKey));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: JwtSecretValidationParameters.Issuer,
                audience: JwtSecretValidationParameters.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: expirationDate,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private async Task<IList<Claim>> GetClaimsAndRoles(IdentityUser user, bool addUserClaims = true) 
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            if (addUserClaims) 
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
                // claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));

                var userClaims = await _userManager.GetClaimsAsync(user);
                var roles = await _userManager.GetRolesAsync(user);

                claims.AddRange(userClaims);

                foreach (var role in roles)
                    claims.Add(new Claim("role", role));
            }

            return claims;
        }
    }
}