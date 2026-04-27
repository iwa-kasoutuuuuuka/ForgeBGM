# ForgeBGM ソフトウェア仕様書 / Software Specification (v3.0 GOLD)

## 1. 概要 / Overview
ForgeBGMは、AI音楽生成のためのトータルプロデュース・アシスタントです。v3.0では、プロンプト構築に加え、ローカル環境でのネイティブAI推論による音楽生成・再生機能を完全統合しました。

ForgeBGM is a total produce assistant for AI music generation. v3.0 fully integrates prompt construction along with local native AI inference for music generation and playback.

## 2. システム構成 / System Architecture
*   **プラットフォーム**: Windows 10/11 (ネイティブ)
    **Platform**: Windows 10/11 (Native)
*   **フレームワーク**: .NET 10.0 / WPF
    **Framework**: .NET 10.0 / WPF
*   **推論エンジン**: ONNX Runtime (DirectML GPU加速)
    **Inference Engine**: ONNX Runtime (DirectML GPU Acceleration)
*   **オーディオ出力**: NAudio (ネイティブ統合)
    **Audio Output**: NAudio (Native Integration)
*   **デザイン**: ダークモード / グラスモルフィズム
    **Design**: Dark Mode / Glassmorphism

## 3. 主要機能 / Core Features
### 3.1 トータルプロデュース・エンジン / Total Produce Engine
*   **音楽構成**: 最適化プロンプト (v3.0) の生成。
    **Music Structure**: Generation of optimized prompts (v3.0).
*   **歌詞案**: ジャンル・ムードに合わせた歌詞の自動生成。
    **Lyrics**: Automatic generation of lyrics tailored to genre and mood.
*   **ビジュアル案**: ジャケット画像用プロンプトの同時出力。
    **Visual Art**: Simultaneous output of prompts for jacket artwork.

### 3.2 ローカルAI推論 (v3.0) / Local AI Inference (v3.0)
*   **ACE-Step / HeartMuLa 最適化**: 最新のオープンソースモデルに対応。
    **ACE-Step / HeartMuLa Optimization**: Compatible with the latest open-source models.
*   **オンデバイス生成**: GPU (DirectML) を活用した、外部サーバー不要の音楽生成基盤。
    **On-device Generation**: Music generation infrastructure using GPU (DirectML) without external servers.
*   **内蔵プレイヤー**: 生成された .wav ファイルを即座に再生。
    **Built-in Player**: Immediate playback of generated .wav files.

## 4. 技術詳細 / Technical Details
*   **ONNX Runtime**: Python不要のネイティブAI推論を実現。
    **ONNX Runtime**: Realizes native AI inference without requiring Python.
*   **DirectML**: Windows標準のGPU加速を利用し、幅広いハードウェアで動作。
    **DirectML**: Utilizes standard Windows GPU acceleration, compatible with wide range of hardware.
*   **I18n**: 日本語・英語の完全なバイリンガルUI。
    **I18n**: Full bilingual UI supporting Japanese and English.

---
© 2026 ForgeBGM Project. v3.0 Local AI HyperSpeed Edition.
