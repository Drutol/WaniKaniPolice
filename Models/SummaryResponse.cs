using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WaniKaniDiscordProgressBot.Models
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SummaryResponse
    {
        public string Url { get; set; }
        public DateTime DataUpdatedAt { get; set; }
        public SummaryData Data { get; set; }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class Lesson
        {
            public DateTime AvailableAt { get; set; }
            public List<int> SubjectIds { get; set; }
        }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class Review
        {
            public DateTime AvailableAt { get; set; }
            public List<object> SubjectIds { get; set; }
        }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class SummaryData
        {
            public List<Lesson> Lessons { get; set; }
            public DateTime NextReviewsAt { get; set; }
            public List<Review> Reviews { get; set; }
        }
    }
}
