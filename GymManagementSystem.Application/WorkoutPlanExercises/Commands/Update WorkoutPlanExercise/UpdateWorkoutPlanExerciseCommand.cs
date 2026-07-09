using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.WorkoutPlanExercises.Commands.Update_WorkoutPlanExercise;

public record UpdateWorkoutPlanExerciseCommand(
    int workoutPlanExerciseId,
    int Sets,
    int Reps,
    float Weight,
    float RestTime) : IRequest<Result>;
