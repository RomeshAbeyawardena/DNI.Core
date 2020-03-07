using DNI.Shared.Shared.Attributes;

namespace DNI.Shared.App.Contracts
{
    public interface ICustomerReference
    {
        [Claim("Reference")]
        string CustomerReference { get; set; }
    }
}
