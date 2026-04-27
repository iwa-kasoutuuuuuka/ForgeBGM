using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForgeBGM
{
    public class SkillManager
    {
        private const string SkillFile = "skills.json";
        public List<Skill> CustomSkills { get; private set; } = new List<Skill>();

        public void LoadSkills()
        {
            try
            {
                if (File.Exists(SkillFile))
                {
                    string json = File.ReadAllText(SkillFile);
                    var loaded = JsonSerializer.Deserialize<List<Skill>>(json);
                    CustomSkills = loaded ?? new List<Skill>(ForgeEngine.DefaultSkills);
                }
                else
                {
                    CustomSkills = new List<Skill>(ForgeEngine.DefaultSkills);
                    SaveSkills(CustomSkills);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Skill Load Error: {ex.Message}");
                CustomSkills = new List<Skill>(ForgeEngine.DefaultSkills);
            }
        }

        public void SaveSkills(List<Skill> skills)
        {
            try
            {
                string json = JsonSerializer.Serialize(skills, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SkillFile, json);
            }
            catch { }
        }
    }

    public class UpdateService
    {
        public static async Task<string?> CheckForUpdateAsync(string currentVersion)
        {
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "ForgeBGM-UpdateChecker");
                // 実際のリリースチェックは将来的な拡張用
                return null; 
            }
            catch { return null; }
        }
    }
}
