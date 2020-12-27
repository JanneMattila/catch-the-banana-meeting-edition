using CTB.Shared.Interfaces;
using System.Collections.Generic;

namespace CTB.Server.Data
{
    public interface IRepository
    {
        Monkey GetByConnectionID(string playerID);
        Monkey GetByPlayerID(string playerID);
        
        List<Monkey> GetMonkeys();
        List<Shark> GetSharks();

        Monkey DeleteByConnectionID(string connectionID);
        Monkey MapConnectionIDToPlayer(string connectionID, string playerID);
    }
}