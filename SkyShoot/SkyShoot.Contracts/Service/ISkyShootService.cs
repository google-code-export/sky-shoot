using System.Drawing;
using System.ServiceModel;
using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Perks;
using SkyShoot.Contracts.Session;

namespace SkyShoot.Contracts.Service
{
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
        GameDescription CreateGame(GameMode mode, int maxPlayers);

        [OperationContract]
        bool JoinGame(GameDescription game);

        // Временное решение. Заменить на vector2 из XNA
        // Вектор нормализован, модуль 1
        [OperationContract(IsOneWay = true)]
        void Move(PointF direction);

        [OperationContract(IsOneWay = true)]
        void Shoot(PointF direction);

        [OperationContract(IsOneWay = true)]
        void TakeBonus(ABonus bonus);

        [OperationContract]
        void TakePerk(Perk perk);

        [OperationContract]
        void LeaveGame();
    }
}
