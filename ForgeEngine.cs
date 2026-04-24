using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeBGM
{
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
        public int Bpm { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Structure { get; set; } = string.Empty;
        public List<Skill> ActiveSkills { get; set; } = new List<Skill>();
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string UserInput { get; set; } = string.Empty;

        public override string ToString() => $"[{Timestamp:HH:mm}] {UserInput.Substring(0, Math.Min(20, UserInput.Length))}...";
    }

    public class ForgeEngine
    {
        private static readonly Random Rnd = new Random();
        
        public static readonly List<Skill> Skills = new List<Skill>
        {
            new Skill { Id = 1, Name = "Cinematic Master", NameEn = "Cinematic Master", Keywords = new[] { "壮大", "オーケストラ", "映画", "ゲーム", "ドラマチック", "epic", "cinematic", "orchestral" } },
            new Skill { Id = 2, Name = "LoFi & Chill Architect", NameEn = "LoFi & Chill Architect", Keywords = new[] { "リラックス", "作業", "chill", "lofi", "カフェ", "癒し", "analog", "warm" } },
            new Skill { Id = 3, Name = "Cyberpunk / Synthwave Engineer", NameEn = "Cyberpunk Engineer", Keywords = new[] { "サイバー", "未来", "シンセ", "電子", "cyberpunk", "synthwave", "neon", "future" } },
            new Skill { Id = 4, Name = "Emotional Composer", NameEn = "Emotional Composer", Keywords = new[] { "感情", "切ない", "悲しい", "希望", "感動", "emotional", "sad", "hopeful", "piano" } },
            new Skill { Id = 5, Name = "Loop Perfectionist", NameEn = "Loop Perfectionist", Keywords = new[] { "ループ", "配信", "繰り返し", "loop", "bgm", "seamless" } },
            new Skill { Id = 6, Name = "High Energy Action Specialist", NameEn = "Action Specialist", Keywords = new[] { "バトル", "戦闘", "激しい", "高速", "action", "battle", "intense", "fast" } },
            new Skill { Id = 7, Name = "Acoustic & Organic Creator", NameEn = "Acoustic Creator", Keywords = new[] { "生楽器", "ギター", "アコースティック", "自然", "acoustic", "guitar", "organic" } },
            new Skill { Id = 8, Name = "Experimental Sound Designer", NameEn = "Sound Designer", Keywords = new[] { "実験的", "独特", "先進的", "experimental", "unique", "texture" } }
        };

        // 楽器データベース
        private static readonly Dictionary<string, string[]> MainInstruments = new Dictionary<string, string[]>
        {
            { "Epic", new[] { "soaring heroic brass", "powerful orchestral strings", "fast staccato violins", "epic pipe organ" } },
            { "Chill", new[] { "warm jazzy Rhodes piano", "mellow acoustic guitar", "soft felt piano arpeggios", "chill electric guitar" } },
            { "Cyber", new[] { "fat sawtooth leads", "aggressive synth arpeggios", "neon futuristic pads", "heavy FM synth bass" } },
            { "Emotional", new[] { "emotional solo piano", "delicate cello melody", "expressive violin solo", "soft harp plucks" } },
            { "Action", new[] { "heavy distorted electric guitar", "fast aggressive synth bass", "staccato brass stabs" } },
            { "Organic", new[] { "warm acoustic guitar", "earthy percussion", "native flute melodies", "kalimba sparkles" } }
        };

        private static readonly Dictionary<string, string[]> SubInstruments = new Dictionary<string, string[]>
        {
            { "Epic", new[] { "thunderous taiko drums", "cinematic percussion layers", "choir textures" } },
            { "Chill", new[] { "soft vinyl crackle", "mellow boom bap drums", "subtle nature sounds", "ambient lo-fi pads" } },
            { "Cyber", new[] { "retro drum machine", "distorted reese bass", "glitchy textures" } },
            { "Emotional", new[] { "ethereal ambient pads", "distant reverb washes", "subtle clockwork ticking" } }
        };

        private const string QualityTags = ", high quality, studio master, crystal clear mix, professional mastering, wide dynamic range, rich spatial reverb, detailed soundstage";

        private static bool FastContains(string source, string value)
        {
            return source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static async Task<List<Skill>> SelectSkillsAsync(string text)
        {
            return await Task.Run(() =>
            {
                return Skills.Select(s => new { Skill = s, Score = s.Keywords.Count(kw => FastContains(text, kw)) })
                             .Where(x => x.Score > 0)
                             .OrderByDescending(x => x.Score)
                             .Take(3)
                             .Select(x => x.Skill)
                             .ToList();
            });
        }

        public static async Task<PromptResult> GenerateAsync(string text)
        {
            var activeSkills = await SelectSkillsAsync(text);
            
            return await Task.Run(() =>
            {
                // 動的楽器選択ロジック
                string styleKey = "Chill"; // デフォルト
                if (activeSkills.Any(s => s.Id == 1 || s.Id == 6)) styleKey = "Epic";
                else if (activeSkills.Any(s => s.Id == 3)) styleKey = "Cyber";
                else if (activeSkills.Any(s => s.Id == 4)) styleKey = "Emotional";
                else if (activeSkills.Any(s => s.Id == 7)) styleKey = "Organic";

                string mainInst = GetRandom(MainInstruments.ContainsKey(styleKey) ? MainInstruments[styleKey] : MainInstruments["Chill"]);
                string subInst = GetRandom(SubInstruments.ContainsKey(styleKey) ? SubInstruments[styleKey] : SubInstruments["Chill"]);
                
                string genre = styleKey == "Epic" ? "Epic cinematic orchestral" : 
                               styleKey == "Cyber" ? "Cyberpunk synthwave" : 
                               styleKey == "Chill" ? "Chill lo-fi hip hop" : "Ambient soundscape";

                string structure = "intro → main theme → build-up → powerful drop → calm outro, seamless loopable";
                string mood = string.IsNullOrWhiteSpace(text) ? "peaceful and atmospheric" : text;
                string key = styleKey == "Epic" || styleKey == "Cyber" ? "D minor" : "C major";
                int bpm = styleKey == "Epic" ? 142 : styleKey == "Cyber" ? 128 : 85;

                string fullPrompt = $"{genre}, {mainInst}, {subInst}, {structure}, {mood}, {key}, {bpm} BPM{QualityTags}";

                return new PromptResult
                {
                    FullPrompt = fullPrompt,
                    Bpm = bpm,
                    Key = key,
                    Structure = structure,
                    ActiveSkills = activeSkills,
                    UserInput = text
                };
            });
        }

        private static string GetRandom(string[] array) => array[Rnd.Next(array.Length)];

        public static string GenerateReport(PromptResult result, bool isEnglish = false)
        {
            string softName = isEnglish ? "ForgeBGM v1.1 (Native Advanced)" : "ForgeBGM v1.1 (ネイティブ高機能版)";
            return $"[ForgeBGM Generation Report]\n" +
                   $"Software: {softName}\n" +
                   $"Date: {result.Timestamp:yyyy-MM-dd HH:mm:ss}\n" +
                   $"Prompt Used: {result.FullPrompt}\n" +
                   $"Selected BPM: {result.Bpm}\n" +
                   $"Key: {result.Key}\n" +
                   $"Structure: {result.Structure}\n" +
                   $"Seed: {Rnd.Next(100000000, 999999999)}\n" +
                   $"Duration: 3:15 (Estimated)";
        }
    }
}
