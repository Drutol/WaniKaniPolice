using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaniKaniDiscordProgressBot.Models;

[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class ReviewsStatisticsResponse
{
    public string Object { get; set; }
    public string Url { get; set; }
    public Page Pages { get; set; }
    public int TotalCount { get; set; }
    public DateTime? DataUpdatedAt { get; set; }
    public List<Statistic> Data { get; set; }

    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Statistic
    {
        public StatisticData Data { get; set; }
        public DateTime? DataUpdatedAt { get; set; }
    }

    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class StatisticData
    {
        public DateTime? CreatedAt { get; set; }
        public int SubjectId { get; set; }
        public string SubjectType { get; set; }
        public int MeaningCorrect { get; set; }
        public int MeaningIncorrect { get; set; }
        public int MeaningMaxStreak { get; set; }
        public int MeaningCurrentStreak { get; set; }
        public int ReadingCorrect { get; set; }
        public int ReadingIncorrect { get; set; }
        public int ReadingMaxStreak { get; set; }
        public int ReadingCurrentStreak { get; set; }
        public int PercentageCorrect { get; set; }
        public bool Hidden { get; set; }
        public int Id { get; set; }
        public string Object { get; set; }
        public string Url { get; set; }
    }

    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Page
    {
        public int PerPage { get; set; }
        public string NextUrl { get; set; }
        public object PreviousUrl { get; set; }
    }
}
