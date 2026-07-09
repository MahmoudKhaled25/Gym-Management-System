using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlanExercises.Dtos;

public record WorkoutPlanExerciseDto(
    int Id,
    int Sets,
    int Reps,
    float Weight,
    float RestTime,
    string? ExerciseName
);

public record WorkoutPlanExercisesGroupedDto(
    string WorkoutPlanName,
    IEnumerable<WorkoutPlanExerciseDto> Exercises
);

