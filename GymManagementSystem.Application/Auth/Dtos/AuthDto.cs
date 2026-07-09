using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Dtos;

public record AuthDto
(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string Token,
    int ExpiresIn,
    IEnumerable<string> Roles,
    string RefreshToken,
    DateTime RefreshTokenExpiration
);
