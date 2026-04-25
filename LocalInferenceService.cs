using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using NAudio.Wave;

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

            await Task.Run(() =>
            {
                try
                {
                    var options = new SessionOptions();
                    // DirectML (GPU) を優先的に試行
                    try { options.AppendExecutionProvider_DML(0); } catch { /* CPU fallback implicitly */ }
                    
                    if (File.Exists(modelPath))
                    {
                        _session?.Dispose();
                        _session = new InferenceSession(modelPath, options);
                        _isLoaded = true;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Model Load Error: {ex.Message}");
                }
            });
        }

        public async Task<string> GenerateAudioAsync(string prompt, IProgress<double> progress)
        {
            if (!_isLoaded)
            {
                // モデル未ロード時はデモ用シミュレーションを実行
                return await SimulateGeneration(progress);
            }

            return await Task.Run(() =>
            {
                try
                {
                    // TODO: 実際の ONNX 推論ロジック (ACE-Step Tiny 等)
                    // 現時点では推論エンジンの疎通確認のみ
                    progress.Report(1.0);
                    return "output.wav";
                }
                catch (Exception ex)
                {
                    throw new Exception($"Inference Failed: {ex.Message}");
                }
            });
        }

        private async Task<string> SimulateGeneration(IProgress<double> progress)
        {
            for (int i = 0; i <= 100; i += 10)
            {
                progress.Report(i / 100.0);
                await Task.Delay(150); // 軽量モデルを想定したウェイト
            }
            return "Generated_Audio_Placeholder.wav";
        }

        public void PlayAudio(string filePath)
        {
            try
            {
                StopAudio(); // 前の再生を停止

                if (File.Exists(filePath))
                {
                    _audioFile = new AudioFileReader(filePath);
                    _outputDevice = new WaveOutEvent();
                    _outputDevice.Init(_audioFile);
                    _outputDevice.Play();
                    
                    // 再生終了時のクリーンアップ登録
                    _outputDevice.PlaybackStopped += (s, e) => StopAudio();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Playback Error: {ex.Message}");
            }
        }

        public void StopAudio()
        {
            _outputDevice?.Stop();
            _outputDevice?.Dispose();
            _audioFile?.Dispose();
            _outputDevice = null;
            _audioFile = null;
        }

        public void Dispose()
        {
            StopAudio();
            _session?.Dispose();
        }
    }
}
