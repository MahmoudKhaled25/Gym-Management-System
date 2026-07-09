using GymManagementSystem.Domain.Abstractions.Error;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GymManagementSystem.Application.Accounts.Commands.UploadProfileImage;

public record UploadProfileImageCommand(string UserId, IFormFile Image) : IRequest<Result<string>>;
