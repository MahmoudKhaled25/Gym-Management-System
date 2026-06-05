using GymManagementSystem.Contracts.Admin;
using GymManagementSystem.Contracts.Dashboard;

namespace GymManagementSystem.Services;

public interface IAdminService
{
    Task<Result<IEnumerable<AdminResponse>>> GetAllAdminsAsync(CancellationToken cancellationToken = default);
    Task<Result> AddAdminAsync(AddAdminRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(string adminId, CancellationToken cancellationToken = default);

    Task<Result<DashboardResponse>> GetDashboardAsync(CancellationToken cancellationToken = default);
}
