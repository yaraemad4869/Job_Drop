using JobDrop.Application.Contracts.Persistence;
using JobDrop.Application.Features.RecruiterReview.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.RecruiterReview.Handlers;

public sealed class ShortlistDropHandler : IRequestHandler<ShortlistDropCommand, bool>
{
    private readonly IApplicationDbContext _context;
    public ShortlistDropHandler(IApplicationDbContext context) => _context = context;

    public async Task<bool> Handle(ShortlistDropCommand request, CancellationToken ct)
    {
        var app = await _context.Applications.FirstOrDefaultAsync(a => a.Id == request.DropId, ct)
            ?? throw new JobDrop.Domain.Exceptions.NotFoundException("Drop not found");
        app.Shortlist();
        await _context.SaveChangesAsync(ct);
        return true;
    }
}