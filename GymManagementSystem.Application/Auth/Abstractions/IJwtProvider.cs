using GymManagementSystem.Domain.Entities;

namespace GymManagementSystem.Application.Auth.Abstractions;

public interface IJwtProvider
{
    (string token,int expiresIn) GenerateToken(ApplicationUser user,IEnumerable<string> roles);

    string? ValidateToken(string token);

}
