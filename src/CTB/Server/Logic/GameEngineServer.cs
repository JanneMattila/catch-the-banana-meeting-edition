using CTB.Server.Data;
using CTB.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTB.Server.Logic
{
    public class GameEngineServer : IGameEngineServer
    {
        private readonly IRepository _repository;

        public GameEngineServer(IRepository repository)
        {
            _repository = repository;
        }

        public int Update(double delta)
        {
            foreach (var monkey in _repository.GetMonkeys())
            {
                if (monkey.Position.Speed > 0)
                {
                    monkey.Position.X += (int)Math.Round(delta / 10 * Math.Cos(monkey.Position.Rotation));
                    monkey.Position.Y += (int)Math.Round(delta / 10 * Math.Sin(monkey.Position.Rotation));
                }
            }
            return 0;
        }
    }
}
