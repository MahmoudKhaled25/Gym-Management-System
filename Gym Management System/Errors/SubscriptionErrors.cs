namespace Gym_Management_System.Errors;

public record SubscriptionErrors
{
    public static readonly Error SubscriptionNotFound =
  new("Subscription.SubscriptionNotFound", "Subscription Not Found", StatusCodes.Status404NotFound);
}
