using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.MembershipPlans.Dtos;

public record MembershipPlanDto(int Id, string Name, string Description, decimal Price, int DurationInDays, bool IsActive);
