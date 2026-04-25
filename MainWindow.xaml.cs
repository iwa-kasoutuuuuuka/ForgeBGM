using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ForgeBGM
{
    public partial class MainWindow : Window
    {
        private bool _isEnglish = false;
        private ObservableCollection<PromptResult> _history = new ObservableCollection<PromptResult>();

        public MainWindow()
        {
            InitializeComponent();
            HistoryList.ItemsSource = _history;
            this.KeyDown += MainWindow_KeyDown;
            UpdateLanguageUI();
        }

        private async void GenerateBtn_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteGenerationAsync();
        }

        private async Task ExecuteGenerationAsync()
        {
            string text = UserInput.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            GenerateBtn.IsEnabled = false;
            GenerateBtn.Content = _isEnglish ? "Processing..." : "プロデュース中...";

            try
            {
                var model = (ModelType)ModelSelector.SelectedIndex;
                var result = await ForgeEngine.GenerateAsync(text, model);
                _history.Insert(0, result);
                UpdateUI(result);
            }
            finally
            {
                GenerateBtn.IsEnabled = true;
                GenerateBtn.Content = _isEnglish ? "Generate Total Produce" : "トータルプロデュース生成";
            }
        }

        private async void LocalGenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PromptOutput.Text))
            {
                await ExecuteGenerationAsync();
            }

            LocalGenBtn.IsEnabled = false;
            TxtStatus.Text = _isEnglish ? "Inference Running..." : "推論実行中...";
            
            var progress = new Progress<double>(v => GenProgress.Value = v);
            
            try
            {
                string prompt = PromptOutput.Text;
                string audioPath = await LocalInferenceService.Instance.GenerateAudioAsync(prompt, progress);
                
                TxtStatus.Text = _isEnglish ? "Generation Complete! Playing..." : "生成完了！再生中...";
                LocalInferenceService.Instance.PlayAudio(audioPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Inference Error");
                TxtStatus.Text = "Error";
            }
            finally
            {
                LocalGenBtn.IsEnabled = true;
                await Task.Delay(2000);
                TxtStatus.Text = _isEnglish ? "Ready to Forge" : "準備完了";
                GenProgress.Value = 0;
            }
        }

        private async void Preset_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                string key = btn.Tag.ToString() ?? string.Empty;
                string prompt = "";
                string presetText = "";

                switch (key)
                {
                    case "boss":
                        presetText = "[Preset] BOSS";
                        prompt = "Epic cinematic orchestral, powerful heroic brass, intense and majestic";
                        break;
                    case "lofi":
                        presetText = "[Preset] LO-FI";
                        prompt = "Chill lo-fi hip hop, warm jazzy Rhodes piano, relaxing rainy night";
                        break;
                    case "cyber":
                        presetText = "[Preset] CYBERPUNK";
                        prompt = "Cyberpunk synthwave, fat sawtooth leads, neon city night";
                        break;
                }

                UserInput.Text = presetText;
                var model = (ModelType)ModelSelector.SelectedIndex;
                var result = await ForgeEngine.GenerateAsync(prompt, model);
                _history.Insert(0, result);
                UpdateUI(result);
            }
        }

        private void UpdateUI(PromptResult result)
        {
            PromptOutput.Text = result.FullPrompt;
            ReportOutput.Text = ForgeEngine.GenerateReport(result, _isEnglish);
            LyricsOutput.Text = result.Lyrics;
            ArtOutput.Text = result.ArtPrompt;
            SkillList.ItemsSource = result.ActiveSkills;
        }

        private void HistoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HistoryList.SelectedItem is PromptResult selected)
            {
                UpdateUI(selected);
                UserInput.Text = selected.UserInput;
                ModelSelector.SelectedIndex = (int)selected.TargetModel;
            }
        }

        private void LangBtn_Click(object sender, RoutedEventArgs e)
        {
            _isEnglish = !_isEnglish;
            UpdateLanguageUI();
            
            if (_history.Count > 0)
            {
                ReportOutput.Text = ForgeEngine.GenerateReport(_history.First(), _isEnglish);
            }
        }

        private void UpdateLanguageUI()
        {
            if (_isEnglish)
            {
                TxtHistoryHeader.Text = "🕒 History";
                TxtModelLabel.Text = "Target Model: ";
                TxtInputHeader.Text = "🎹 Input Prompt";
                TxtInputSub.Text = "Enter your musical idea here";
                TxtSkillHeader.Text = "⚡ Active SKILLs";
                TxtSkillSub.Text = "Skills activated by AI";
                TxtPromptHeader.Text = "Optimized Prompt (v2.0)";
                TxtReportHeader.Text = "Generation Report";
                TxtLyricsHeader.Text = "Lyric Ideas";
                TxtArtHeader.Text = "Jacket Art Prompt";
                TabMusic.Header = "🎵 Music";
                TabLyrics.Header = "✍ Lyrics";
                TabArt.Header = "🎨 Visual Art";
                GenerateBtn.Content = "Generate Total Produce";
                LocalGenBtn.Content = "Generate & Play (Local)";
                TxtLocalEngineHeader.Text = "🤖 Local AI Music Engine";
                TxtStatus.Text = "Ready to Forge";
                BtnBoss.Content = "Boss Battle";
                BtnLofi.Content = "Lo-Fi Chill";
                BtnCyber.Content = "Cyberpunk";
            }
            else
            {
                TxtHistoryHeader.Text = "🕒 履歴";
                TxtModelLabel.Text = "ターゲットモデル: ";
                TxtInputHeader.Text = "🎹 インプット・プロンプト";
                TxtInputSub.Text = "楽曲のイメージを入力してください";
                TxtSkillHeader.Text = "⚡ 発動スキル";
                TxtSkillSub.Text = "AIが自動的に選択した専門スキル";
                TxtPromptHeader.Text = "最適化プロンプト (v2.0)";
                TxtReportHeader.Text = "生成レポート";
                TxtLyricsHeader.Text = "歌詞アイデア";
                TxtArtHeader.Text = "ジャケット画像用プロンプト";
                TabMusic.Header = "🎵 音楽構成";
                TabLyrics.Header = "✍ 歌詞案";
                TabArt.Header = "🎨 ビジュアル案";
                GenerateBtn.Content = "トータルプロデュース生成";
                LocalGenBtn.Content = "ローカルで音楽生成＆再生";
                TxtLocalEngineHeader.Text = "🤖 ローカルAI音楽エンジン";
                TxtStatus.Text = "準備完了";
                BtnBoss.Content = "ボスバトル";
                BtnLofi.Content = "Lo-Fi 作業用";
                BtnCyber.Content = "サイバーパンク";
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _ = ExecuteGenerationAsync();
            }
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (!PromptOutput.IsFocused && !UserInput.IsFocused)
                {
                    CopyPrompt_Click(null!, null!);
                }
            }
        }

        private void CopyPrompt_Click(object sender, RoutedEventArgs e) => SafeCopy(PromptOutput.Text);
        private void CopyReport_Click(object sender, RoutedEventArgs e) => SafeCopy(ReportOutput.Text);
        private void CopyLyrics_Click(object sender, RoutedEventArgs e) => SafeCopy(LyricsOutput.Text);
        private void CopyArt_Click(object sender, RoutedEventArgs e) => SafeCopy(ArtOutput.Text);

        private void SafeCopy(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                Clipboard.SetText(text);
            }
        }
    }
}