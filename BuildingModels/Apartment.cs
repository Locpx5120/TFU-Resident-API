﻿using Core.Entity;

namespace BuildingModels;

public partial class Apartment : MasterDataEntityBase
{
    public string? DepartmentType { get; set; }

    public double? Price { get; set; }

    public int? Floor { get; set; }
    public int? RoomNumber { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Living> Livings { get; set; } = new List<Living>();

    public virtual ICollection<OwnerShip> OwnerShips { get; set; } = new List<OwnerShip>();
}
