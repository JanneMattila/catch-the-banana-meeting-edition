﻿using CTB.Server.Logic;
using CTB.Shared.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CTB.Server.Data
{
    public class Repository : IRepository
    {
        private ConcurrentDictionary<string, Monkey> _monkeys = new();
        private ConcurrentDictionary<string, Shark> _sharks = new();
        private ConcurrentDictionary<string, Banana> _bananas = new();
        private ConcurrentDictionary<string, string> _mapConnectionID2PlayerID = new();
        private Random _random = new Random();

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
                UI = _random.Next(1, 3),
                Position = new Position()
                {
                    X = _random.Next(10, 150),
                    Y = _random.Next(10, 150)
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
