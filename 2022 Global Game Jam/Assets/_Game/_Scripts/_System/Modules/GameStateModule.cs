using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace blu
{
    public class GameStateModule : Module
    {
        public enum GameState
        {
            NONE,
            MAIN_MENU,
            IN_GAME,
            PAUSED
        }

        public enum RotationState
        {
            NONE,
            SIDE_ON,
            TOP_DOWN
        }

        private static GameState m_currentGameState = GameState.NONE;
        private static RotationState m_currentRotationState = RotationState.NONE;

        public static GameState CurrentGameState { get => m_currentGameState; }
        public static RotationState CurrentRotationState { get => m_currentRotationState; }
    }
}