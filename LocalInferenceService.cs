using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using NAudio.Wave;

namespace ForgeBGM
{
    public class LocalInferenceService
    {
        private static LocalInferenceService? _instance;
        public static LocalInferenceService Instance => _instance ??= new LocalInferenceService();

        private InferenceSession? _session;
        private bool _isLoaded = false;

        public bool IsLoaded => _isLoaded;

        private LocalInferenceService() { }

        public async Task LoadModelAsync(string modelPath)
        {
            if (_isLoaded) return;

            await Task.Run(() =>
            {
                try
                {
                    // DirectML (GPU) を有効化してロード
                    var options = new SessionOptions();
                    options.AppendExecutionProvider_DML(0); // GPU 0 を使用
                    
                    if (File.Exists(modelPath))
                    {
                        _session = new InferenceSession(modelPath, options);
                        _isLoaded = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Model Load Error: {ex.Message}");
                }
            });
        }

        public async Task<string> GenerateAudioAsync(string prompt, IProgress<double> progress)
        {
            if (!_isLoaded)
            {
                // 軽量モデルがロードされていない場合はダミー生成（シミュレーション）
                return await SimulateGeneration(progress);
            }

            return await Task.Run(() =>
            {
                // ここで実際の ONNX 推論を実行
                // ACE-Step Tiny の入力テンソルを作成し、推論を実行
                // 現時点ではパスを返す
                progress.Report(1.0);
                return "output.wav";
            });
        }

        private async Task<string> SimulateGeneration(IProgress<double> progress)
        {
            // 軽量モデルによる生成をシミュレート（デモ用）
            for (int i = 0; i <= 100; i += 5)
            {
                progress.Report(i / 100.0);
                await Task.Delay(100);
            }
            
            // 実際の実装ではここで一時フォルダにダミーのwavを作成するか
            // 既存のサンプルを返す
            return "Generated_Audio_Placeholder.wav";
        }

        public void PlayAudio(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var player = new AudioFileReader(filePath);
                    var outputDevice = new WaveOutEvent();
                    outputDevice.Init(player);
                    outputDevice.Play();
                }
            }
            catch { }
        }
    }
}
