using CTB.Shared.Interfaces;
using System.Collections.Generic;

namespace CTB.Server.Data
{
    public interface IRepository
    {
        Monkey GetByConnectionID(string playerID);
        Monkey GetByPlayerID(string playerID);
        
        List<Monkey> GetMonkeys();

        Monkey DeleteByConnectionID(string connectionID);
        Monkey MapConnectionIDToPlayer(string connectionID, string playerID);

        void AddShark(Shark shark);
        List<Shark> GetSharks();
        void DeleteShark(string id);

        void AddBanana(Banana banana);
        List<Banana> GetBananas();
        void DeleteBanana(string id);
    }
}