# Slot Machine Game

## Overview

This is a Unity-based slot machine game featuring smooth reel animations, randomized outcomes, and a scoring system. The game simulates a classic slot machine experience with sound effects and UI feedback.

---

## How to Play

1. Touch **Lever** to spin all reels
2. Press **Stop** to stop reels sequentially
3. Match symbols to earn points

---

## Winning Logic

* **777** → Jackpot → **1000 points**
* **Triple Match** → **100 points**
* **Two Match** → **50 points**
* No Match → **0 points**

---

## Features

* Smooth reel animations using DOTween
* Randomized reel stopping (RNG-based)
* Sound system:

  * Handle pull
  * Reel spinning loop
  * Reel stop per reel
  * Jackpot sound
* UI feedback:

  * Blink effect
  * Handle animation
  * Score updates

---

## WebGL Build

To run the game:

1. Navigate to:

   ```
   Build/WebGL/
   ```
2. Open `index.html` in a browser

---

## Approach

* Used **Object-Oriented Programming**
* Separated systems:

  * `ReelController` → Game flow
  * `ReelSimpleLoop` → Reel animation + RNG
  * `SlotSoundManager` → Audio system
* Used **coroutines** for sequential stopping
* Used **events (OnReelStopped)** for clean logic flow

---

## Bonus Features

* Sequential reel stopping system
* Sound layering for realistic feel
* Smooth deceleration logic for reels

---

## Project Structure

* `Assets/Scripts/`
* `Assets/UI/`
* `Assets/Sounds/`
* `Assets/Fonts/`

---

## Notes

* RNG is implemented per reel using `Random.Range`
* Reel stopping is calculated dynamically to ensure smooth animation
