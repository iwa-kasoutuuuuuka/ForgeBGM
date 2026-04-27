using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeBGM
{
    public enum ModelType { Standard, HeartMuLa, ACEStep }

    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string[] Keywords { get; set; } = Array.Empty<string>();
    }

    public class PromptResult
    {
        public string FullPrompt { get; set; } = string.Empty;
        public string Lyrics { get; set; } = string.Empty;
        public string ArtPrompt { get; set; } = string.Empty;
        public int Bpm { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Structure { get; set; } = string.Empty;
        public List<Skill> ActiveSkills { get; set; } = new List<Skill>();
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string UserInput { get; set; } = string.Empty;
        public ModelType TargetModel { get; set; } = ModelType.Standard;
        public override string ToString() => $"[{Timestamp:HH:mm}] {UserInput.Substring(0, Math.Min(20, UserInput.Length))}...";
    }

    public class ForgeEngine
    {
        private static readonly Random Rnd = new Random();
        private static List<Skill>? _currentSkills;
        public static List<Skill> CurrentSkills 
        { 
            get => _currentSkills ?? DefaultSkills;
            set => _currentSkills = value;
        }

        public static readonly List<Skill> DefaultSkills = new List<Skill>
        {
            new Skill { Id = 1, Name = "Cinematic Master", NameEn = "Cinematic Master", Keywords = new[] { "壮大", "オーケストラ", "映画", "epic", "cinematic" } },
            new Skill { Id = 2, Name = "LoFi & Chill Architect", NameEn = "LoFi & Chill Architect", Keywords = new[] { "リラックス", "chill", "lofi" } },
            new Skill { Id = 3, Name = "Cyberpunk Engineer", NameEn = "Cyberpunk Engineer", Keywords = new[] { "サイバー", "未来", "cyberpunk", "synthwave" } },
            new Skill { Id = 4, Name = "Emotional Composer", NameEn = "Emotional Composer", Keywords = new[] { "感情", "切ない", "emotional", "piano" } }
        };

        private static readonly Dictionary<string, string[]> MainInstruments = new Dictionary<string, string[]>
        {
            { "Epic", new[] { "heroic brass", "orchestral strings" } },
            { "Chill", new[] { "jazzy Rhodes", "acoustic guitar" } },
            { "Cyber", new[] { "sawtooth leads", "neon pads" } },
            { "Emotional", new[] { "solo piano", "cello melody" } }
        };

        private static readonly Dictionary<string, string[]> LyricsDatabase = new Dictionary<string, string[]>
        {
            { "Epic", new[] { "Rise above the thunder", "Light will guide our destiny" } },
            { "Chill", new[] { "Raindrops on the window", "Faded coffee and old books" } },
            { "Cyber", new[] { "Digital hearts", "Neon pulse through veins" } },
            { "Emotional", new[] { "Whispers of a dream", "Tears like stars" } }
        };

        public static async Task<PromptResult> GenerateAsync(string text, ModelType model = ModelType.Standard)
        {
            string cleanText = string.IsNullOrWhiteSpace(text) ? "Ambient soundscape" : text.Trim();
            var activeSkills = CurrentSkills.Select(s => new { Skill = s, Score = s.Keywords.Count(kw => cleanText.Contains(kw, StringComparison.OrdinalIgnoreCase)) })
                                           .Where(x => x.Score > 0).OrderByDescending(x => x.Score).Take(3).Select(x => x.Skill).ToList();
            
            return await Task.Run(() =>
            {
                string styleKey = activeSkills.Any(s => s.Id == 1) ? "Epic" : activeSkills.Any(s => s.Id == 3) ? "Cyber" : activeSkills.Any(s => s.Id == 4) ? "Emotional" : "Chill";
                string GetSafe(Dictionary<string, string[]> d, string k) => d.ContainsKey(k) ? d[k][Rnd.Next(d[k].Length)] : d["Chill"][Rnd.Next(d["Chill"].Length)];

                string mainInst = GetSafe(MainInstruments, styleKey);
                string genre = styleKey == "Epic" ? "Cinematic" : styleKey == "Cyber" ? "Synthwave" : "Lo-Fi";
                int bpm = styleKey == "Epic" ? 140 : 80;

                return new PromptResult
                {
                    FullPrompt = $"{genre}, {mainInst}, {cleanText}, {bpm} BPM, high quality",
                    Lyrics = string.Join("\n", Enumerable.Range(0, 2).Select(_ => GetSafe(LyricsDatabase, styleKey))),
                    ArtPrompt = $"{genre} landscape, cinematic lighting",
                    Bpm = bpm, Key = "C", Structure = "Standard", ActiveSkills = activeSkills, UserInput = cleanText, TargetModel = model
                };
            });
        }

        public static string GenerateReport(PromptResult result, bool isEnglish = false)
        {
            return $"[ForgeBGM v3.0 Report]\nModel: {result.TargetModel}\nBPM: {result.Bpm}\nPrompt: {result.FullPrompt}";
        }
    }
}
