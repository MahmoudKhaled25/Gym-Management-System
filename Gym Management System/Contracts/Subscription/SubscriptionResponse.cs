using Gym_Management_System.Enums;

namespace Gym_Management_System.Contracts.Subscription;

public record SubscriptionResponse(int Id,string UserId,string UserName,string? TrainerId,string? TrainerName,int MembershipPlanId,string MembershipPlanName,DateOnly StartDate,DateOnly EndDate,SubscriptionStatus Status);
