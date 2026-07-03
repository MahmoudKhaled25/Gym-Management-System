using GymManagementSystem.Domain.Abstractions.Error;
using Microsoft.AspNetCore.Http;

namespace GymManagementSystem.Domain.Errors;

public record SubscriptionErrors
{
    public static readonly Error SubscriptionNotFound =
  new("Subscription.SubscriptionNotFound", "Subscription Not Found", StatusCodes.Status404NotFound);


    public static readonly Error SubscriptionExists =
   new("Subscription.SubscriptionExists", "Subscription Exists", StatusCodes.Status400BadRequest);

    public static readonly Error PlanHasNoTrainer =
   new("Subscription.PlanHasNoTrainer", "Plan Has No Trainer", StatusCodes.Status400BadRequest);

    public static readonly Error UserAlreadyHasActiveSubscription =
           new("Subscription.UserAlreadyHasActiveSubscription", "User Already Has Active Subscription", StatusCodes.Status400BadRequest);

}
