using Core.Entity;

namespace BuildingModels;

public class Resident : MasterDataEntityBase
{
    public DateTime? RegistratorDate { get; set; }

    public string Name { get; set; }
    public string? Email { get; set; }
    public DateTime? Birthday { get; set; }
    public string? Phone { get; set; }
    public string? Password { get; set; }
    public bool IsOwner { get; set; }

    public bool? IsChangePassword { get; set; }

    public virtual ICollection<Living> Livings { get; set; } = new List<Living>();
    public virtual ICollection<RequestComplain> RequestComplains { get; set; } = new List<RequestComplain>();
}
