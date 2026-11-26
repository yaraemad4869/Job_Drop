namespace JobDrop.Application.Services;

/// <summary>
/// BE-04: Provides neutral avatars for blind screening.
/// Never derives avatars from candidate gender.
/// </summary>
public class NeutralAvatarService
{
    private static readonly string[] NeutralAvatars = new[]
    {
        "/assets/avatars/neutral-1.svg",
        "/assets/avatars/neutral-2.svg",
        "/assets/avatars/neutral-3.svg",
        "/assets/avatars/neutral-4.svg",
        "/assets/avatars/neutral-5.svg"
    };

    public string GetNeutralAvatar(Guid candidateId)
    {
        var index = Math.Abs(candidateId.GetHashCode()) % NeutralAvatars.Length;
        return NeutralAvatars[index];
    }
}
