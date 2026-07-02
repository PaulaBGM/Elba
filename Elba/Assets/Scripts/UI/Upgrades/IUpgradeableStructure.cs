public interface IUpgradeableStructure
{
    StructureUpgradeSO NextUpgrade { get; }

    bool CanUpgrade();

    bool Upgrade();
}