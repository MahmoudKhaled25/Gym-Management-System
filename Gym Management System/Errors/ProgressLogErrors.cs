namespace GymManagementSystem.Errors;

public class ProgressLogErrors
{
    public static readonly Error ProgressLogNotFound =
new("ProgressLog.ProgressLogNotFound", "Progress Log Not Found", StatusCodes.Status404NotFound);


    public static readonly Error ProgressLogExists =
   new("ProgressLog.ProgressLogExists", "Progress Log Exists", StatusCodes.Status400BadRequest);
}
