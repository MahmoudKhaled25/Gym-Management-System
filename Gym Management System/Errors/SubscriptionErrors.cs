namespace Gym_Management_System.Errors;

public record SubscriptionErrors
{
    public static readonly Error SubscriptionNotFound =
  new("Subscription.SubscriptionNotFound", "Subscription Not Found", StatusCodes.Status404NotFound);


    public static readonly Error SubscriptionExists =
   new("Subscription.SubscriptionExists", "Subscription Exists", StatusCodes.Status400BadRequest);
}
