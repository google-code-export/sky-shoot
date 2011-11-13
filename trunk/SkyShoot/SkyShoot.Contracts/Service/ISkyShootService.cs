using System.ServiceModel;
using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Perks;
using SkyShoot.Contracts.Session;
using Microsoft.Xna.Framework;
using SkyShoot.Contracts.Mobs;

namespace SkyShoot.Contracts.Service
{
    public delegate void SomebodyMovesHadler(AMob sender, Vector2 direction);
    public delegate void ClientShootsHandler(AMob sender, Vector2 direction);
    public delegate void SomebodyShootsHandler(AMob sender, Weapon.Projectiles.AProjectile[] projectiles);

    [ServiceContract(CallbackContract = typeof(ISkyShootCallback))]
    public interface ISkyShootService
    {

        [OperationContract(IsInitiating = true)]
        bool Register(string username, string password);

        [OperationContract(IsInitiating = true)]
        bool Login(string username, string password);

        [OperationContract]
        GameDescription[] GetGameList();

        [OperationContract]
        bool CreateGame(GameMode mode, int maxPlayers);

        [OperationContract]
        bool JoinGame(GameDescription game);

        // Временное решение. Заменить на vector2 из XNA
        // Вектор нормализован, модуль 1
        [OperationContract(IsOneWay = true)]
        void Move(Vector2 direction);

        [OperationContract(IsOneWay = true)]
        void Shoot(Vector2 direction);

        [OperationContract(IsOneWay = true)]
        void TakeBonus(AObtainableDamageModifier bonus);

        [OperationContract]
        void TakePerk(Perk perk);

        [OperationContract]
        void LeaveGame();
    }
}
