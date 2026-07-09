using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlanExercises.Commands.Add_WorkoutPlanExercise;

public record AddWorkoutPlanExerciseCommand(
    int WorkoutPlanId,
    int ExerciseId,
    int Sets,
    int Reps,
    float Weight,
    float RestTime) : IRequest<Result>;
