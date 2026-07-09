using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.ProgressLogs.Dtos;

public record AddProgressLogRequest(
    float Weight,
    string Notes,
    DateOnly LogDate);
