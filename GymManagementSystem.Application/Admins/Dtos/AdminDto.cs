using GymManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Admins.Dtos;

public record AdminDto(string Id, string Name, string Email, string? PhoneNumber, Gender Gender, bool IsActive);

