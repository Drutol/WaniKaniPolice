using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WaniKaniDiscordProgressBot.Models
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class AssignmentsResponse
    {
        public string Url { get; set; }
        public AssignmentPages Pages { get; set; }
        public int TotalCount { get; set; }
        public DateTime? DataUpdatedAt { get; set; }
        public List<Assignment> Data { get; set; }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class AssignmentPages
        {
            public int PerPage { get; set; }
            public object NextUrl { get; set; }
            public object PreviousUrl { get; set; }
        }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class AssignmentData
        {
            public DateTime? CreatedAt { get; set; }
            public int SubjectId { get; set; }
            public string SubjectType { get; set; }
            public int SrsStage { get; set; }
            public string SrsStageName { get; set; }
            public DateTime? UnlockedAt { get; set; }
            public DateTime? StartedAt { get; set; }
            public object PassedAt { get; set; }
            public object BurnedAt { get; set; }
            public DateTime? AvailableAt { get; set; }
            public object ResurrectedAt { get; set; }
            public bool Passed { get; set; }
            public bool Hidden { get; set; }
        }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class Assignment
        {
            public long Id { get; set; }
            public string Object { get; set; }
            public string Url { get; set; }
            public DateTime? DataUpdatedAt { get; set; }
            public AssignmentData Data { get; set; }
        }
    }
}
