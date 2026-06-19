using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.Application.ProgressLogs.Dtos;

public record AllProgressLogsDto(int Id, string MemberFullName, float Weight, string Notes, DateOnly LogDate);


public record MyProgressLogsDto(float Weight, string Notes, DateOnly LogDate);


