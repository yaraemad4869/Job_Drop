using System;
using System.Collections.Generic;
using MediatR;

namespace JobDrop.Application.Queries;

public record GetAuditLogsQuery(
    Guid? ActorUserId,
    string? Action,
    string? TargetType,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<AuditLogResponse>;

public record AuditLogResponse(
    int TotalItems,
    int PageNumber,
    int PageSize,
    int TotalPages,
    List<AuditLogDto> Data);

public record AuditLogDto(
    Guid Id,
    Guid? ActorUserId,
    string Action,
    string? TargetType,
    DateTimeOffset CreatedAtUtc,
    string? IpAddress);
