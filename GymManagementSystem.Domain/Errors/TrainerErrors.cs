using GymManagementSystem.Domain.Abstractions.Error;
using Microsoft.AspNetCore.Http;

namespace GymManagementSystem.Domain.Errors;

public record TrainerErrors
{
    public static readonly Error TrainerNotFound =
new("Trainer.TrainerNotFound", "Trainer Not Found", StatusCodes.Status404NotFound);

    public static readonly Error TrainerExists =
   new("Trainer.TrainerExists", "Trainer Exists", StatusCodes.Status400BadRequest);
}
