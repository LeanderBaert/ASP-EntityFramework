using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using models.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using webapi.Entities;
using webapi.Helpers;

namespace webapi.Repositories
{
    public class PersoonRepositorie : IPersoonRepositorie
    {
        private readonly SignInManager<Persoon> _signInManager;
        private readonly UserManager<Persoon> _userManager;
        private readonly AppSettings _appSettings;

        public PersoonRepositorie(SignInManager<Persoon> signInManager, UserManager<Persoon> userManager, IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        public async Task<PostAuthenticeerResponseModel> Authenticate(PostAuthenticateRequestModel postAuthenticateRequestModel, string ipAddress)
        {
            Persoon? persoon = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == postAuthenticateRequestModel.UserName);

            if (persoon == null)
            {
                throw new Exception("Ongeldige gebruikersnaam");
            }

            PasswordVerificationResult signInResult = _userManager.PasswordHasher.VerifyHashedPassword(persoon, persoon.PasswordHash, postAuthenticateRequestModel.Password);
            //Verify password of found user
            //SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(member, postAuthenticateRequestModel.Password, lockoutOnFailure: false);

            if (signInResult.ToString() != "Success")
            {
                throw new Exception("Ongeldig wachtwoord");
            }

            //Authentication was succesful so generate JW and refresh tokens
            string jwtToken = await GenerateJwtToken(persoon);
            RefreshToken refreshToken = GenerateRefreshToken(ipAddress);

            //save refresh token
            persoon.RefreshTokens.Add(refreshToken);

            try
            {
                await _userManager.UpdateAsync(persoon);
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                throw new Exception(ex.Message.ToString());
            }


            return new PostAuthenticeerResponseModel
            {
                Id = persoon.Id,
                Voornaam = persoon.VoorName,
                Familienaam = persoon.FamilieNaam,
                Gebruikersnaam = persoon.UserName,
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token,
                Roles = await _userManager.GetRolesAsync(persoon)
            };
        }

        public async Task<PostAuthenticeerResponseModel> RenewToken(string token, string ipAddress)
        {
            Persoon persoon = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshTokens.Any(t => t.Token == token));

            if (persoon == null)
            {
                throw new Exception("Geen gebruiker gevonden met dit token");
            }

            RefreshToken refreshToken = persoon.RefreshTokens.Single(x => x.Token == token);

            //Refresh token is no longer active
            if (!refreshToken.IsActive)
            {
                throw new Exception("Refresh token is vervallen");
            };

            //Replace old refresh token with new one
            RefreshToken newRefreshToken = GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            // generate new jwt
            string jwtToken = await GenerateJwtToken(persoon);

            persoon.RefreshTokens.Add(newRefreshToken);

            await _userManager.UpdateAsync(persoon);

            return new PostAuthenticeerResponseModel
            {
                Id = persoon.Id,
                Voornaam = persoon.VoorName,
                Familienaam = persoon.FamilieNaam,
                Gebruikersnaam = persoon.UserName,
                JwtToken = jwtToken,
                RefreshToken = newRefreshToken.Token,
                Roles = await _userManager.GetRolesAsync(persoon)
            };
        }

        public async Task DeactivateToken(string token, string ipAddress)
        {
            Persoon? persoon = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshTokens.Any(x => x.Token == token));

            if (persoon == null)
            {
                throw new Exception("Geen gebruiker gevonden met dit token");
            }

            RefreshToken refreshToken = persoon.RefreshTokens.Single(x => x.Token == token);

            //Refresh token is no longer active
            if (!refreshToken.IsActive)
            {
                throw new Exception("Refresh token is vervallen");
            };

            //Revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            await _userManager.UpdateAsync(persoon);
        }



        private async Task<string> GenerateJwtToken(Persoon persoon)
        {
            var roleNames = await _userManager.GetRolesAsync(persoon).ConfigureAwait(false);

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, persoon.Id.ToString()),
                new Claim("VoorName", persoon.VoorName),
                new Claim("FamilieNaam", persoon.FamilieNaam),
                new Claim("UserName", persoon.UserName)
            };

            foreach (string roleName in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            var test = _appSettings.Secret;
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Issuer = "Opleding web api",
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddSeconds(40), //token jwt
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            byte[] randomBytes = RandomNumberGenerator.GetBytes(64);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddMinutes(2), // time must be the same as SetTokenCookies method in UserController
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress

            };
        }
    }
}
