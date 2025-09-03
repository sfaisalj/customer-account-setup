using IdentityService.Core.Models;
using IdentityService.Core.Ports.Incoming;
using IdentityService.Core.Ports.Outgoing;

namespace IdentityService.Core.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthenticationService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthenticationResult> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return new AuthenticationResult
            {
                Success = false,
                Error = "Invalid email or password"
            };
        }

        var token = _tokenService.GenerateToken(user);
        return new AuthenticationResult
        {
            Success = true,
            Token = token,
            User = user
        };
    }

    public async Task<AuthenticationResult> RegisterAsync(string firstName, string lastName, string email, string password, UserRole role = UserRole.Customer)
    {
        try
        {
            if (await _userRepository.EmailExistsAsync(email))
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Error = "User with this email already exists"
                };
            }

            var user = new CustomerAccount
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role
            };

            var createdUser = await _userRepository.CreateAsync(user);
            var token = _tokenService.GenerateToken(createdUser);

            return new AuthenticationResult
            {
                Success = true,
                Token = token,
                User = createdUser
            };
        }
        catch (Exception ex)
        {
            return new AuthenticationResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    public async Task<CustomerAccount?> GetUserByIdAsync(string userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }
}