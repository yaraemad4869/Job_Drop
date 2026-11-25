namespace JobDrop.Domain.Exceptions;

public sealed class DomainRuleException : DomainException
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public DomainRuleException(string message, string errorCode = "DOMAIN_RULE_VIOLATION")
        : base(message, errorCode)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public DomainRuleException(IDictionary<string, string[]> errors, string errorCode = "DOMAIN_RULE_VIOLATION")
        : base("One or more domain rules have been violated.", errorCode)
    {
        Errors = new Dictionary<string, string[]>(errors);
    }
}