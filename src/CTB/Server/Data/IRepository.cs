using CTB.Shared.Interfaces;

namespace CTB.Server.Data
{
    public interface IRepository
    {
        Monkey Get(string playerID);
    }
}