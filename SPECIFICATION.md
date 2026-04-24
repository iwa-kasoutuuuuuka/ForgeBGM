# ForgeBGM ソフトウェア仕様書 / Software Specification

## 1. 概要 / Overview
ForgeBGMは、AI音楽生成エンジン（Suno, Udio, Stable Audio等）向けの高品質なプロンプトを構築するための専門アシスタントツールです。独自のSKILLシステムにより、ユーザーの意図を汲み取った最適な音楽構成案を自動生成します。

ForgeBGM is a specialized assistant tool designed to construct high-quality prompts for AI music generation engines (such as Suno, Udio, Stable Audio, etc.). Its unique SKILL system automatically generates optimal music structure plans based on user intent.

## 2. システム構成 / System Architecture
*   **プラットフォーム / Platform**: Windows Desktop (Native)
*   **フレームワーク / Framework**: .NET 10.0 / WPF
*   **言語 / Language**: C# 12
*   **UIデザイン / UI Design**: ダークモード / グラスモルフィズム (Dark Mode / Glassmorphism)

## 3. 主要機能 / Core Features
### 3.1 SKILLシステム (ver 1.0) / SKILL System (ver 1.0)
以下の8つの専門スキルから最大3つを自動選択します：
Automatically selects up to 3 specialized skills from the following 8:
1.  **Cinematic Master**: 壮大な映画・ゲーム音楽 / Epic cinematic & game music
2.  **LoFi & Chill Architect**: リラックス・作業用BGM / Relaxing & chill BGM
3.  **Cyberpunk / Synthwave Engineer**: 近未来シンセサウンド / Futuristic synth sounds
4.  **Emotional Composer**: 繊細な感情表現 / Delicate emotional expression
5.  **Loop Perfectionist**: シームレスなループ設計 / Seamless loop design
6.  **High Energy Action Specialist**: 激しいアクション・バトル / Intense action & battle
7.  **Acoustic & Organic Creator**: 生楽器の温もり / Warmth of organic instruments
8.  **Experimental Sound Designer**: 実験的・独創的な音響 / Experimental & creative soundscapes

### 3.2 最適化プロンプト構造 (ver 2.0) / Optimized Prompt Structure (ver 2.0)
生成されるプロンプトは以下の構造を厳守します：
The generated prompts strictly adhere to the following structure:
`[Genre/Style], [Main Instruments], [Sub Instruments], [Song Structure], [Mood], [Key], [Tempo], [Quality Tags]`

## 4. 技術的制約 / Technical Constraints
*   **ボーカル排除 / No Vocals**: インストゥルメンタル曲に特化し、歌声を排除します。 / Specialized for instrumental tracks; excludes vocals.
*   **高品質タグ / Quality Tags**: スタジオマスター級の品質を保証するタグを自動付与します。 / Automatically appends tags to ensure studio-master quality.

---
© 2026 ForgeBGM Project.
