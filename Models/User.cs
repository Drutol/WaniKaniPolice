using System;
using System.Collections.Generic;
using System.Text;

namespace WaniKaniDiscordProgressBot.Models
{
    public class User
    {
        public string WkToken { get; set; }
        public ulong UserId { get; set; }
        public string CheckOverride { get; set; }
        public string CrossOverride { get; set; }
    }
}
