using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Exercises.Dtos;

public record ExerciseDto(int Id, string Name, string Description, string MuscleGroup);


