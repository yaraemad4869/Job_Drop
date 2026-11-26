using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.RecruiterReview.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.RecruiterReview.Handlers;

public sealed class InviteToInterviewHandler : IRequestHandler<InviteToInterviewCommand, bool>
{
    private readonly IApplicationDbContext _context;
    public InviteToInterviewHandler(IApplicationDbContext context) => _context = context;

    public async Task<bool> Handle(InviteToInterviewCommand request, CancellationToken ct)
    {
        var app = await _context.Applications.FirstOrDefaultAsync(a => a.Id == request.DropId, ct)
            ?? throw new JobDrop.Domain.Exceptions.NotFoundException("Drop not found");
        app.ScheduleInterview();
        await _context.SaveChangesAsync(ct);
        return true;
    }
}