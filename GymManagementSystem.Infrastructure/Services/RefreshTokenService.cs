using GymManagementSystem.Application.Auth.Abstractions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace GymManagementSystem.Infrastructure.Services;

public class RefreshTokenService : IRefreshTokenService
{
    public string Generate()
    {
       return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
