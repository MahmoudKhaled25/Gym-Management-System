using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.Accounts.Dtos;

public record UploadProfileImageRequest(IFormFile Image);
