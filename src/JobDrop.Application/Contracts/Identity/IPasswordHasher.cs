namespace JobDrop.Application.Contracts.Identity;

public interface IAppPasswordHasher
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}