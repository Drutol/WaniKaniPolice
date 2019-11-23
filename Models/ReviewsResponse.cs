using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WaniKaniDiscordProgressBot.Models
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    class ReviewsResponse
    {
        public string Url { get; set; }
        public ReviewPages Pages { get; set; }
        public int TotalCount { get; set; }
        public DateTime? DataUpdatedAt { get; set; }
        public List<Review> Data { get; set; }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class ReviewPages
        {
            public int PerPage { get; set; }
            public object NextUrl { get; set; }
            public object PreviousUrl { get; set; }
        }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class ReviewData
        {
            public DateTime? CreatedAt { get; set; }
            public int AssignmentId { get; set; }
            public int SubjectId { get; set; }
            public int StartingSrsStage { get; set; }
            public string StartingSrsStageName { get; set; }
            public int EndingSrsStage { get; set; }
            public string EndingSrsStageName { get; set; }
            public int IncorrectMeaningAnswers { get; set; }
            public int IncorrectReadingAnswers { get; set; }
        }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class Review
        {
            public int Id { get; set; }
            public string Url { get; set; }
            public DateTime? DataUpdatedAt { get; set; }
            public ReviewData Data { get; set; }
        }
    }
}
