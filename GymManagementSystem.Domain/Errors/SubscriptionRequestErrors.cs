using GymManagementSystem.Domain.Abstractions.Error;
using Microsoft.AspNetCore.Http;

namespace GymManagementSystem.Domain.Errors;

public static class SubscriptionRequestErrors
{
    public static readonly Error UserHasActiveSubscription =
        new("SubscriptionRequest.UserHasActiveSubscription",
            "User already has an active subscription",
            StatusCodes.Status400BadRequest);

    public static readonly Error PendingRequestExists =
        new("SubscriptionRequest.PendingRequestExists",
            "User already has a pending subscription request",
            StatusCodes.Status400BadRequest);

    public static readonly Error RequestNotFound =
        new("SubscriptionRequest.NotFound",
            "Subscription request not found",
            StatusCodes.Status404NotFound);

    public static readonly Error AlreadyProcessed =
        new("SubscriptionRequest.AlreadyProcessed",
            "Subscription request has already been processed",
            StatusCodes.Status400BadRequest);

    public static readonly Error NoRequestsFound = 
        new("SubscriptionRequest.NoRequestsFound",
            "No subscription requests found for the user",
            StatusCodes.Status404NotFound);
}
