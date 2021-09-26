using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WaniKaniDiscordProgressBot.Models
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class LevelProgressionsResponse
    {
        public string Object { get; set; }
        public string Url { get; set; }
        public int TotalCount { get; set; }
        public DateTime? DataUpdatedAt { get; set; }
        public List<LevelUpData> Data { get; set; }


        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class LevelUp
        {
            public DateTime? CreatedAt { get; set; }
            public int Level { get; set; }
            public DateTime? UnlockedAt { get; set; }
            public DateTime? StartedAt { get; set; }
            public DateTime? PassedAt { get; set; }
            public object CompletedAt { get; set; }
            public object AbandonedAt { get; set; }
        }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class LevelUpData
        {
            public long Id { get; set; }
            public string Url { get; set; }
            public DateTime? DataUpdatedAt { get; set; }
            public LevelUp Data { get; set; }
        }
    }
}
