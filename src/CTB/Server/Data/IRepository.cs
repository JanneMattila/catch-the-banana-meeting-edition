using CTB.Shared.Interfaces;
using System.Collections.Generic;

namespace CTB.Server.Data
{
    public interface IRepository
    {
        Monkey GetByPlayerID(string playerID);
        List<Monkey> GetMonkeys();
    }
}