using CTB.Shared.Interfaces;
using System.Threading.Tasks;

namespace CTB.Server.Logic
{
    public interface IGameEngineServer
    {
        Task<bool> UpdateAsync(double delta);
        Position MoveMonkey(Monkey monkey, Position position);
    }
}
