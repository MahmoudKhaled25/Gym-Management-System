using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Accounts.Dtos;

public record ChangeUserPasswordDto(
    string OldPassword,
    string NewPassword
);
