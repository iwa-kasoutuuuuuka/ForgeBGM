using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ForgeBGM
{
    public partial class MainWindow : Window
    {
        private bool _isEnglish = false;
        private ObservableCollection<PromptResult> _history = new ObservableCollection<PromptResult>();
        private SkillManager _skillManager = new SkillManager();
        private DispatcherTimer _visualizerTimer = new DispatcherTimer();
        private Random _rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
            _skillManager.LoadSkills();
            ForgeEngine.CurrentSkills = _skillManager.CustomSkills;

            HistoryList.ItemsSource = _history;
            this.KeyDown += MainWindow_KeyDown;
            this.Closed += (s, e) => LocalInferenceService.Instance.Dispose();

            InitVisualizer();
            UpdateLanguageUI();
        }

        private void InitVisualizer()
        {
            _visualizerTimer.Interval = TimeSpan.FromMilliseconds(50);
            _visualizerTimer.Tick += (s, e) => UpdateVisualizer();
            VisualizerCanvas.Children.Clear();
            for (int i = 0; i < 40; i++)
            {
                var bar = new Rectangle { Width = 8, Fill = new SolidColorBrush(Color.FromRgb(79, 70, 229)), RadiusX = 2, RadiusY = 2 };
                Canvas.SetLeft(bar, i * 12 + 5);
                Canvas.SetBottom(bar, 0);
                VisualizerCanvas.Children.Add(bar);
            }
        }

        private void UpdateVisualizer()
        {
            foreach (Rectangle bar in VisualizerCanvas.Children)
            {
                double targetHeight = _rnd.Next(5, 70);
                bar.Height = bar.Height + (targetHeight - bar.Height) * 0.3;
            }
        }

        private async void GenerateBtn_Click(object sender, RoutedEventArgs e) { await ExecuteGenerationAsync(); }

        private async Task ExecuteGenerationAsync()
        {
            string text = UserInput.Text.Trim();
            GenerateBtn.IsEnabled = false;
            try
            {
                var model = (ModelType)ModelSelector.SelectedIndex;
                var result = await ForgeEngine.GenerateAsync(text, model);
                _history.Insert(0, result);
                UpdateUI(result);
            }
            finally { GenerateBtn.IsEnabled = true; }
        }

        private async void LocalGenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_history.Count == 0) return;
            LocalGenBtn.IsEnabled = false;
            TxtStatus.Text = _isEnglish ? "Forging Audio..." : "音声を鍛造中...";
            _visualizerTimer.Start();
            var progress = new Progress<double>(v => Dispatcher.Invoke(() => GenProgress.Value = v));
            try
            {
                string path = await LocalInferenceService.Instance.GenerateAudioAsync(_history[0], progress);
                LocalInferenceService.Instance.PlayAudio(path);
            }
            finally
            {
                LocalGenBtn.IsEnabled = true;
                await Task.Delay(3000);
                _visualizerTimer.Stop();
                foreach (Rectangle bar in VisualizerCanvas.Children) bar.Height = 5;
                TxtStatus.Text = _isEnglish ? "Ready to Forge" : "準備完了";
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

        private void SkillEditor_Click(object sender, RoutedEventArgs e)
        {
            try { System.Diagnostics.Process.Start("notepad.exe", "skills.json"); } catch { }
        }

        private void LangBtn_Click(object sender, RoutedEventArgs e) { _isEnglish = !_isEnglish; UpdateLanguageUI(); }

        private void UpdateLanguageUI()
        {
            if (_isEnglish) {
                TxtHistoryHeader.Text = "🕒 History"; TabMusic.Header = "🎵 Music"; TabLyrics.Header = "✍ Lyrics"; TabArt.Header = "🎨 Art";
                GenerateBtn.Content = "Total Produce"; LocalGenBtn.Content = "Generate (Local)";
            } else {
                TxtHistoryHeader.Text = "🕒 履歴"; TabMusic.Header = "🎵 音楽構成"; TabLyrics.Header = "✍ 歌詞案"; TabArt.Header = "🎨 ビジュアル案";
                GenerateBtn.Content = "トータルプロデュース生成"; LocalGenBtn.Content = "ローカルで生成";
            }
        }

        private void HistoryList_SelectionChanged(object sender, SelectionChangedEventArgs e) { if (HistoryList.SelectedItem is PromptResult s) UpdateUI(s); }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) _ = ExecuteGenerationAsync();
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (!UserInput.IsFocused && !PromptOutput.IsFocused)
                {
                    try { Clipboard.SetText(PromptOutput.Text); } catch { }
                }
            }
        }
    }
}