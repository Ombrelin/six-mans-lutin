using System;
using System.Collections.Generic;

namespace bot
{
    public class ManQueue
    {
        public List<string> Players { get; set; } = new List<string>();
        public ManQueueState State { get; set; } = ManQueueState.Filling;
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Captain { get; set; } = 0;
        public int Random { get; set; } = 0;

        public enum ManQueueState
        {
            Filling,
            Voting,
            Drawn
        }
    }
}