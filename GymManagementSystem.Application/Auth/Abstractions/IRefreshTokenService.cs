using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Auth.Abstractions;

public interface IRefreshTokenService
{
    string Generate();
}
