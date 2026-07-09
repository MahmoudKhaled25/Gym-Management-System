using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlans.Dtos;

public record WorkoutPlanDto(int Id, string Name, string Description, string? TrainerName, string MemberName);

public record MyWorkoutPlanDto(
    int Id,
    string Name,
    string Description
);

public record WorkoutPlanGroupedDto(
    string? TrainerName,
    IEnumerable<MyWorkoutPlanDto> Plans
);
