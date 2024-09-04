using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace Data.Entities;

public class ApplicationUser : IdentityUser
{

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
    public string? ProfileImage { get; set; } = "avatar.jpg";
    public string? PhoneNumber { get; set; } = null!;
    public string? Bio { get; set; }
    public AddressEntity? Address { get; set; }
}

public class AddressEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [ProtectedPersonalData]
    [StringLength(100)]
    public string PrimaryAddress { get; set; } = null!;

    [ProtectedPersonalData]
    [StringLength(100)]
    public string? SecondaryAddress { get; set; } = null!;

    [ProtectedPersonalData]
    [StringLength(10)]
    public string PostalCode { get; set; } = null!;

    [ProtectedPersonalData]
    [StringLength(100)]
    public string City { get; set; } = null!;
}
