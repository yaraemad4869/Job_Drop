using JobDrop.Application.Contracts.Persistence;
using JobDrop.Domain.Entities.Drops;
using JobDrop.Domain.Entities.Identity;
using JobDrop.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobDrop.Application.Features.Drops.Handlers;

public sealed class TransferSpecialDropCommandHandler
    : IRequestHandler<Commands.TransferSpecialDropCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public TransferSpecialDropCommandHandler(IApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<bool> Handle(
        Commands.TransferSpecialDropCommand request,
        CancellationToken cancellationToken)
    {
        var senderId = Guid.Empty; // TODO: From ICurrentUser

        var recipientUser = await _userManager.FindByNameAsync(request.RecipientHandle)
            ?? throw new InvalidOperationException($"User @{request.RecipientHandle} not found");

        var recipient = await _db.CandidateProfiles
            .FirstOrDefaultAsync(c => c.UserId == recipientUser.Id, cancellationToken)
            ?? throw new InvalidOperationException("Recipient has no candidate profile");

        var senderWallet = await _db.DropWalletBalances
            .FirstOrDefaultAsync(w => w.CandidateProfileId == senderId, cancellationToken)
            ?? throw new InvalidOperationException("Sender wallet not found");

        if (senderWallet.SpecialBalance < request.Amount)
        {
            throw new InvalidOperationException("Insufficient Special balance");
        }

        senderWallet.DebitSpecial(request.Amount);

        var recipientWallet = await _db.DropWalletBalances
            .FirstOrDefaultAsync(w => w.CandidateProfileId == recipient.Id, cancellationToken);

        if (recipientWallet == null)
        {
            // Fixed: Use constructor with required parameters
            recipientWallet = new DropWalletBalance(recipient.Id, dailyLimit: 5);
            _db.DropWalletBalances.Add(recipientWallet);
        }

        recipientWallet.CreditSpecial(request.Amount);

        // Fixed: Use constructor with required parameters for DropLedgerEntry
        var senderLedger = new DropLedgerEntry(
            candidateProfileId: senderId,
            bucket: WalletBucket.Special,
            delta: -request.Amount,
            reasonCode: "TransferOut",
            referenceId: recipient.Id.ToString(),
            idempotencyKey: $"transfer:out:{Guid.NewGuid()}");

        var recipientLedger = new DropLedgerEntry(
            candidateProfileId: recipient.Id,
            bucket: WalletBucket.Special,
            delta: request.Amount,
            reasonCode: "TransferIn",
            referenceId: senderId.ToString(),
            idempotencyKey: $"transfer:in:{Guid.NewGuid()}");

        _db.DropLedgerEntries.AddRange(senderLedger, recipientLedger);
        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }
}