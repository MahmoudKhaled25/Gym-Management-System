using GymManagementSystem.Application.Common;
using GymManagementSystem.Application.Consts;
using GymManagementSystem.Application.Exercises.Dtos;
using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;


namespace GymManagementSystem.Application.Exercises.Queries.Get_All_Exercises;

public record GetAllExercisesQuery(RequestFilters Filters): IRequest<Result<PaginatedList<ExerciseDto>>>;