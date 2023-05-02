using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using WaniKaniDiscordProgressBot.Models;

namespace WaniKaniDiscordProgressBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();

            await scheduler.Start();

            var job = JobBuilder
                .CreateForAsync<WanikaniStatsJob>()
                .Build();

            var trigger = TriggerBuilder
                .Create()
                //.StartNow()
                .StartAt(DateTime.UtcNow.Date.AddDays(1))
                .WithSimpleSchedule(builder =>
                    builder
                        .WithIntervalInHours(24)
                        .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);

            await Task.Delay(-1);
        }
    }

    public class WanikaniStatsJob : IJob
    {
        private DiscordSocketClient _discordClient;

        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Delay(TimeSpan.FromMinutes(10));
            Console.WriteLine($"{DateTime.UtcNow} Executing...");
            try
            {
                _discordClient = new DiscordSocketClient();
                var secrets = JsonConvert.DeserializeObject<Secrets>(File.ReadAllText("secrets.json"));

                await _discordClient.LoginAsync(
                    TokenType.Bot,
                    token: secrets.BotToken);

                _discordClient.Log += async message =>
                {
                    Console.WriteLine(message);
                };

                _discordClient.Ready += DiscordClientOnReady;
                await _discordClient.StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{DateTime.UtcNow} Failed. {e}");
            }
        }

        private async Task DiscordClientOnReady()
        {
            try
            {
                var server = _discordClient.GetGuild(343060137164144642);
                var channel = (IMessageChannel)server.GetChannel(505414979458170903);

                await server.DownloadUsersAsync();

                foreach (var user in JsonConvert.DeserializeObject<List<User>>(File.ReadAllText("users.json")))
                {
                    try
                    {
                        using var generator = new WaniKaniStatsGenerator(user, server);
                        await channel.SendMessageAsync(null, false, await generator.GenerateEmbed());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"{DateTime.UtcNow} Failed for {user.UserId}. {e}");
                    }
                }

                _discordClient.Dispose();
                Console.WriteLine($"{DateTime.UtcNow} Finished.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{DateTime.UtcNow} Failed. {e}");
            }
        }
    }

    public class WaniKaniStatsGenerator : IDisposable
    {
        private readonly User _user;
        private readonly SocketGuild _socketGuild;
        private readonly HttpClient _httpClient;

        public WaniKaniStatsGenerator(User user, SocketGuild socketGuild)
        {
            _user = user;
            _socketGuild = socketGuild;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization 
                = new AuthenticationHeaderValue("Bearer", user.WkToken);
            _httpClient.DefaultRequestHeaders.Add("Wanikani-Revision", "20170710");
        }

        public async Task<Embed> GenerateEmbed()
        {
            var builder = new EmbedBuilder();
            var user = await Get<UserResponse>("/user");
            var reviews = await Get<ReviewsStatisticsResponse>("/review_statistics");
            var assignments = await Get<AssignmentsResponse>("/assignments");
            var summary = await Get<SummaryResponse>("/summary");
            var progressions = await Get<LevelProgressionsResponse>("/level_progressions", false);

            var discordUser = _socketGuild.GetUser(_user.UserId);

            builder.WithAuthor(authorBuilder => authorBuilder
                .WithName(discordUser.Nickname ?? discordUser.Username)
                .WithIconUrl(discordUser.GetAvatarUrl()));

            var reviewsCount = summary.Data.Reviews
                .Where(review => review.AvailableAt != null && DateTime.UtcNow > review.AvailableAt)
                .SelectMany(review => review.SubjectIds)
                .Count();

            var daysOnLevel = (DateTime.UtcNow.Date - progressions.Data.Last().Data.UnlockedAt)?.Days ?? 0;

            builder.WithDescription($"Level: **{user.Data.Level}**\n" +
                                    $"Available Lessons: **{summary.Data.Lessons[0].SubjectIds.Count}**\n" +
                                    $"Available Reviews: **{reviewsCount}**\n" +
                                    $"Serving Crabigator for: **{(DateTime.UtcNow.Date - user.Data.StartedAt).Days} days**");

            var yesterday = DateTime.UtcNow.Date.AddDays(-1);

            if (reviews.Data.Count(s => s.DataUpdatedAt.HasValue && s.DataUpdatedAt.Value.Date == yesterday) > 0)
            {
                builder.WithThumbnailUrl(_user.CheckOverride ?? "https://mylovelyvps.xyz/wkpolice/tick.png");
                builder.WithColor(Color.Green);
            }
            else
            {
                builder.WithThumbnailUrl(_user.CrossOverride ?? "https://mylovelyvps.xyz/wkpolice/cross.png");
                builder.WithColor(Color.DarkRed);
            }

            builder.WithFooter(footerBuilder =>
                footerBuilder.WithText($"{DateTime.UtcNow.Date:M} - {DateTime.UtcNow.Date.AddDays(1):M}"));

            builder.AddField(fieldBuilder => fieldBuilder
                .WithName("Completed Reviews")
                .WithValue(reviews.Data.Count(review => review.DataUpdatedAt.HasValue && 
                                                        review.DataUpdatedAt.Value.Date == yesterday)));

            builder.AddField(fieldBuilder => fieldBuilder
                .WithName("Completed Lessons")
                .WithValue(assignments.Data.Count(assignment => assignment.Data.StartedAt.HasValue && 
                                                                assignment.Data.StartedAt.Value.Date == yesterday)));

            //builder.AddField(fieldBuilder => fieldBuilder
            //    .WithName("New Lessons")
            //    .WithValue(assignments.Data.Count(assignment => assignment.Data.SrsStage == 0)));

            builder.AddField(fieldBuilder => fieldBuilder
                .WithName("Days on current level")
                .WithValue(daysOnLevel.ToString()));

            return builder.Build();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        private async Task<T> Get<T>(string address, bool appendTime = true)
        {
            var endpoint = $"https://api.wanikani.com/v2/{address}";
            if (appendTime)
                endpoint += $"?updated_after={DateTime.UtcNow.Date.Subtract(TimeSpan.FromDays(1)):O}";
            var json = await _httpClient.GetStringAsync(endpoint);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
