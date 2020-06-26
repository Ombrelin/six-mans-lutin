using System;
using System.Collections.Generic;
using System.Linq;

namespace bot
{
    public class QueueEngine
    {
        private Stack<ManQueue> queues = new Stack<ManQueue>();

        public QueueEngine()
        {
            this.queues.Push(new ManQueue());
        }

        public string EnqueuePlayer(string player)
        {
            string response;
            if (this.queues.Peek().Players.Count < 6)
            {
                this.queues.Peek().Players.Add(player);
                response = $"{player} joined the Queue {this.queues.Peek().Id}\n";
                if (this.queues.Peek().Players.Count == 6)
                {
                    response +=
                        $"Vote for the game mode of queue {this.queues.Peek().Id} : CAPTAIN (!c) or RANDOM (!r)";
                }

                return response;
            }
            else
            {
                this.queues.Push(new ManQueue());
                this.queues.Peek().Players.Add(player);
                return $"{player} joined the Queue {this.queues.Peek().Id}";
            }
        }

        public string DequeuePLayer(string player)
        {
            try
            {
                var queue = this.queues.First(q => q.Players.Contains(player));
                queue.Players.Remove(player);
                return $"Player {player} has been removed from queue {queue.Id}";
            }
            catch (InvalidOperationException e)
            {
                return $"{player} not enqueued";
            }
        }

        public Dictionary<string, string> VoteCaptain(Guid queueId)
        {
            try
            {
                var queue = this.queues.First(q => q.Id == queueId);
                if (queue.State == ManQueue.ManQueueState.Voting)
                {
                    queue.Captain++;
                    var responses = new Dictionary<string, string>();
                    if (queue.Captain == 4)
                    {
                        queue.State = ManQueue.ManQueueState.Drawn;
                        var rand = new Random();
                        var captain1 = queue.Players[rand.Next(0, queue.Players.Count - 1)];
                        queue.Players.Remove(captain1);
                        var captain2 = queue.Players[rand.Next(0, queue.Players.Count - 1)];

                        responses.Add("Game name", queue.Id.ToString());
                        responses.Add("Game password", Guid.NewGuid().ToString());
                        responses.Add("Captain 1", captain1);
                        responses.Add("Captain 2", captain2);
                        return responses;
                    }
                    else
                    {
                        responses.Add("Captain votes", queue.Captain.ToString());
                        responses.Add("Random votes", queue.Random.ToString());
                        return responses;
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Queue {queue} is not being voted");
                }
            }
            catch (InvalidOperationException e)
            {
                throw new ArgumentException($"Queue {queueId} does not exist");
            }
        }

        public Dictionary<string, string> VoteRandom(Guid queueId)
        {
            try
            {
                var queue = this.queues.First(q => q.Id == queueId);

                if (queue.State == ManQueue.ManQueueState.Voting)
                {
                    queue.Random++;
                    var responses = new Dictionary<string, string>();
                    if (queue.Captain == 4)
                    {
                        queue.State = ManQueue.ManQueueState.Drawn;
                        var rand = new Random();
                        queue.Players = queue.Players.OrderBy(x => (rand.Next())).ToList();
                        var team1 = new List<string> {queue.Players[0], queue.Players[1], queue.Players[2]};
                        var team2 = new List<string> {queue.Players[3], queue.Players[4], queue.Players[5]};


                        responses.Add("Team 1", team1.Aggregate((v, acc) => acc + " " + v));
                        responses.Add("Team 2", team2.Aggregate((v, acc) => acc + " " + v));
                        responses.Add("Game name", queue.Id.ToString());
                        responses.Add("Game password", Guid.NewGuid().ToString());

                        return responses;
                    }
                    else
                    {
                        responses.Add("Captain votes", queue.Captain.ToString());
                        responses.Add("Random votes", queue.Random.ToString());

                        return responses;
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Queue {queue} is not being voted");
                }
            }
            catch (InvalidOperationException e)
            {
                throw new ArgumentException($"Queue {queueId} does not exist");
            }
        }
    }
}