using CTB.Server.Logic;
using CTB.Shared;
using CTB.Shared.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CTB.Server.Data
{
    public class Repository : IRepository
    {
        private readonly ConcurrentDictionary<string, Monkey> _monkeys = new();
        private readonly ConcurrentDictionary<string, Shark> _sharks = new();
        private readonly ConcurrentDictionary<string, Banana> _bananas = new();
        private readonly ConcurrentDictionary<string, string> _mapConnectionID2PlayerID = new();
        private readonly Random _random = new();

        public Monkey GetByConnectionID(string connectionID)
        {
            if (_mapConnectionID2PlayerID.ContainsKey(connectionID))
            {
                var playerID = _mapConnectionID2PlayerID[connectionID];
                return GetByPlayerID(playerID);
            }
            return null;
        }

        public Monkey DeleteByConnectionID(string connectionID)
        {
            if (_mapConnectionID2PlayerID.ContainsKey(connectionID))
            {
                var playerID = _mapConnectionID2PlayerID[connectionID];
                _mapConnectionID2PlayerID.Remove(connectionID, out _);
                return _monkeys[playerID];
            }
            return null;
        }

        public Monkey MapConnectionIDToPlayer(string connectionID, string playerID)
        {
            _mapConnectionID2PlayerID[connectionID] = playerID;
            return GetByConnectionID(connectionID);
        }

        public Monkey GetByPlayerID(string playerID)
        {
            if (_monkeys.ContainsKey(playerID))
            {
                return _monkeys[playerID];
            }

            var name = PlayerNameGenerator.CreateName();
            _monkeys[playerID] = new Monkey()
            {
                ID = playerID,
                Name = name,
                Score = 0,
                UI = _random.Next(1, 7),
                Position = new Position()
                {
                    X = _random.Next(WorldConstants.BorderRadius, WorldConstants.Screen.Width - WorldConstants.BorderRadius),
                    Y = _random.Next(WorldConstants.BorderRadius, WorldConstants.Screen.Height - WorldConstants.BorderRadius)
                }
            };
            return _monkeys[playerID];
        }

        public List<Monkey> GetMonkeys()
        {
            return _monkeys.Values.ToList();
        }

        public void AddShark(Shark shark)
        {
            _sharks[shark.ID] = shark;
        }

        public List<Shark> GetSharks()
        {
            return _sharks.Values.ToList();
        }

        public void DeleteShark(string id)
        {
            _sharks.Remove(id, out _);
        }

        public void AddBanana(Banana banana)
        {
            _bananas[banana.ID] = banana;
        }

        public List<Banana> GetBananas()
        {
            return _bananas.Values.ToList();
        }

        public void DeleteBanana(string id)
        {
            _bananas.Remove(id, out _);
        }
    }
}
