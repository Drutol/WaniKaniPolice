using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WaniKaniDiscordProgressBot.Models
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class UserResponse
    {
        public string Url { get; set; }
        public DateTime? DataUpdatedAt { get; set; }
        public UserData Data { get; set; }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class Subscription
        {
            public bool Active { get; set; }
            public string Type { get; set; }
            public int MaxLevelGranted { get; set; }
            public object PeriodEndsAt { get; set; }
        }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class Preferences
        {
            public int LessonsBatchSize { get; set; }
            public bool LessonsAutoplayAudio { get; set; }
            public bool ReviewsAutoplayAudio { get; set; }
            public string LessonsPresentationOrder { get; set; }
            public bool ReviewsDisplaySrsIndicator { get; set; }
        }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class UserData
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public int Level { get; set; }
            public string ProfileUrl { get; set; }
            public DateTime StartedAt { get; set; }
            public Subscription Subscription { get; set; }
            public bool Subscribed { get; set; }
            public int MaxLevelGrantedBySubscription { get; set; }
            public object CurrentVacationStartedAt { get; set; }
            public Preferences Preferences { get; set; }
        }
    }
}
