namespace AnyCodeHub.Contract.Abstractions.Services;

public interface IUrlHelperService
{
    string GenerateMailConfirmationLink(string Email, string Token, string CallbackUrl);
}
