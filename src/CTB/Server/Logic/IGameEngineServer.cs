using CTB.Shared.Interfaces;

namespace CTB.Server.Logic
{
    public interface IGameEngineServer
    {
        bool Update(double delta);
        Position MoveMonkey(Monkey monkey, Position position);
    }
}