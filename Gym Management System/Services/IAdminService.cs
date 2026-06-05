using GymManagementSystem.Contracts.Admin;

namespace GymManagementSystem.Services;

public interface IAdminService
{
    Task<Result<IEnumerable<AdminResponse>>> GetAllAdminsAsync(CancellationToken cancellationToken = default);
    Task<Result> AddAdminAsync(AddAdminRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(string adminId, CancellationToken cancellationToken = default);
}
