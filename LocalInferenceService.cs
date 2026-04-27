using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ML.OnnxRuntime;
using NAudio.Wave;
using TagLib;

namespace ForgeBGM
{
    public class LocalInferenceService : IDisposable
    {
        private static LocalInferenceService? _instance;
        public static LocalInferenceService Instance => _instance ??= new LocalInferenceService();

        private InferenceSession? _session;
        private IWavePlayer? _outputDevice;
        private AudioFileReader? _audioFile;
        private bool _isLoaded = false;

        public bool IsLoaded => _isLoaded;

        private LocalInferenceService() { }

        public async Task LoadModelAsync(string modelPath)
        {
            if (_isLoaded) return;
            await Task.Run(() => {
                try {
                    var options = new SessionOptions();
                    try { options.AppendExecutionProvider_DML(0); } catch { }
                    if (System.IO.File.Exists(modelPath)) {
                        _session = new InferenceSession(modelPath, options);
                        _isLoaded = true;
                    }
                } catch { }
            });
        }

        public async Task<string> GenerateAudioAsync(PromptResult result, IProgress<double> progress)
        {
            // 軽量モデルによる生成をシミュレート
            for (int i = 0; i <= 100; i += 10) {
                progress.Report(i / 100.0);
                await Task.Delay(150);
            }
            
            string fileName = $"ForgeBGM_{DateTime.Now:yyyyMMdd_HHmmss}.wav";
            // 実際はここで推論結果を保存するが、デモ用にダミーを作成
            // （NAudio等で無音ファイルやサンプルをコピーする処理を想定）
            
            // メタデータの埋め込み
            EmbedMetadata(fileName, result);
            
            return fileName;
        }

        public void EmbedMetadata(string filePath, PromptResult result)
        {
            try
            {
                if (!System.IO.File.Exists(filePath)) return;
                
                using (var tfile = TagLib.File.Create(filePath))
                {
                    tfile.Tag.Title = $"AI Produced: {result.UserInput}";
                    tfile.Tag.Comment = $"Prompt: {result.FullPrompt}\nBPM: {result.Bpm}\nKey: {result.Key}";
                    tfile.Tag.Genres = new[] { "AI Generated", result.TargetModel.ToString() };
                    tfile.Tag.Copyright = "Created with ForgeBGM v3.0";
                    tfile.Save();
                }
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Metadata Error: {ex.Message}"); }
        }

        public void PlayAudio(string filePath)
        {
            try {
                StopAudio();
                if (System.IO.File.Exists(filePath)) {
                    _audioFile = new AudioFileReader(filePath);
                    _outputDevice = new WaveOutEvent();
                    _outputDevice.Init(_audioFile);
                    _outputDevice.Play();
                    _outputDevice.PlaybackStopped += (s, e) => StopAudio();
                }
            } catch { }
        }

        public void StopAudio() {
            _outputDevice?.Stop(); _outputDevice?.Dispose(); _audioFile?.Dispose();
            _outputDevice = null; _audioFile = null;
        }

        public void Dispose() { StopAudio(); _session?.Dispose(); }
    }
}
