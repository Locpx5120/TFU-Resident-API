using Core.Entity;

namespace BuildingModels;

public partial class Finance : MasterDataEntityBase
{
    public string FinanceType { get; set; } = null!;

    public double? Maintain { get; set; }

    public double? IncidentalChanges { get; set; }

    public double? ServiceFee { get; set; }

    public string ProviderName { get; set; } = null!;
}
