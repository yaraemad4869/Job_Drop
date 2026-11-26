using MediatR;
using Microsoft.EntityFrameworkCore;
using JobDrop.Domain.Entities.Observability;
using JobDrop.Application.Contracts.Persistence;

namespace JobDrop.Application.Queries;

public class GetAuditLogsQueryHandler : IRequestHandler<GetAuditLogsQuery, AuditLogResponse>
{
    private readonly IApplicationDbContext _context;

    public GetAuditLogsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AuditLogResponse> Handle(GetAuditLogsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.AuditLogs.AsQueryable();

        if (request.ActorUserId.HasValue)
            query = query.Where(a => a.ActorUserId == request.ActorUserId);

        if (!string.IsNullOrEmpty(request.Action))
            query = query.Where(a => a.Action == request.Action);

        if (!string.IsNullOrEmpty(request.TargetType))
            query = query.Where(a => a.TargetType == request.TargetType);

        var totalItems = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(a => a.CreatedAtUtc)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(a => new AuditLogDto(
                a.Id,
                a.ActorUserId,
                a.Action,
                a.TargetType,
                a.CreatedAtUtc,
                a.IpAddress))
            .ToListAsync(cancellationToken);

        return new AuditLogResponse(
            totalItems,
            request.PageNumber,
            request.PageSize,
            (int)Math.Ceiling(totalItems / (double)request.PageSize),
            items);
    }
}
