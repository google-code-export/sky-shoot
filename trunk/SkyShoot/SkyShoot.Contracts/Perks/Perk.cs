using System.Runtime.Serialization;

namespace SkyShoot.Contracts.Perks
{
    [DataContract]
    public enum Perk
    {
        [EnumMember]
        BloodyMess,
        Sharpshooter,
        BetterCriticals,
    }
}