using DNI.Core.Shared.Attributes;

namespace DNI.Core.App.Contracts
{
    public interface ICustomerReference
    {
        [Claim("Reference")]
        string CustomerReference { get; set; }
    }
}
