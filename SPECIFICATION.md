# ForgeBGM ソフトウェア仕様書 / Software Specification (v2.0)

## 1. 概要 / Overview
ForgeBGMは、AI音楽生成のためのトータルプロデュース・アシスタントです。v2.0では、プロンプト構築に加え、ローカル環境でのAI推論による音楽生成・再生機能を統合しました。

ForgeBGM is a total produce assistant for AI music generation. v2.0 integrates prompt construction along with local AI inference for music generation and playback.

## 2. システム構成 / System Architecture
*   **プラットフォーム / Platform**: Windows 10/11 (Native)
*   **フレームワーク / Framework**: .NET 10.0 / WPF
*   **推論エンジン / Inference Engine**: ONNX Runtime (DirectML)
*   **オーディオ出力 / Audio Output**: NAudio
*   **デザイン / Design**: ダークモード / グラスモルフィズム (Dark Mode / Glassmorphism)

## 3. 主要機能 / Core Features
### 3.1 トータルプロデュース・エンジン / Total Produce Engine
*   **音楽構成 / Music Structure**: 最適化プロンプト (v2.0) の生成。
*   **歌詞案 / Lyrics**: ジャンル・ムードに合わせた歌詞の自動生成。
*   **ビジュアル案 / Visual Art**: ジャケット画像用プロンプトの同時出力。

### 3.2 ローカルAI推論 (v2.0) / Local AI Inference (v2.0)
*   **ACE-Step / HeartMuLa 最適化**: 最新のオープンソースモデルに対応。
*   **オンデバイス生成**: GPU (DirectML) を活用した、外部サーバー不要の音楽生成基盤。
*   **内蔵プレイヤー**: 生成された `.wav` ファイルを即座に再生。

## 4. 技術詳細 / Technical Details
*   **ONNX Runtime**: Python不要のネイティブAI推論を実現。 / Native AI inference without Python.
*   **DirectML**: Windows標準のGPU加速を利用。 / Utilizes standard Windows GPU acceleration.
*   **I18n**: 日本語・英語の完全切り替え。 / Full toggle between JP and EN.

---
© 2026 ForgeBGM Project. v2.0 Local AI HyperSpeed Edition.
