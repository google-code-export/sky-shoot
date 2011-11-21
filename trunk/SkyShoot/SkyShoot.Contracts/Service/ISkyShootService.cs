using System;

using System.ServiceModel;

using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Perks;
using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Session;

using Microsoft.Xna.Framework;

namespace SkyShoot.Contracts.Service
{
    public delegate void SomebodyMovesHandler(AMob sender, Vector2 direction);
    public delegate void ClientShootsHandler(AMob sender, Vector2 direction);
    public delegate void SomebodyShootsHandler(AMob sender, Weapon.Projectiles.AProjectile[] projectiles);
	public delegate void StartGameHandler(AMob[] mobs);
    public delegate void SomebodyDiesHandler(AMob sender);
	public delegate void SomebodyHitHandler(AMob target, Weapon.Projectiles.AProjectile projectile);
    public delegate void SomebodySpawnsHandler(AMob sender);
	public delegate void NewPlayerConnectedHandler(AMob player);
	public delegate void PlayerLeftHandler (AMob player);

    [ServiceContract(CallbackContract = typeof(ISkyShootCallback))]
    public interface ISkyShootService
    {

        [OperationContract(IsInitiating = true)]
        bool Register(string username, string password);

        [OperationContract(IsInitiating = true)]
        Guid? Login(string username, string password);

        [OperationContract]
        GameDescription[] GetGameList();

        [OperationContract]
        GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet level);

        [OperationContract]
        bool JoinGame(GameDescription game);

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
