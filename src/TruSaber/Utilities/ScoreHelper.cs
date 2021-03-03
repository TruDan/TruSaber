using System;
using RocketUI.Utilities.Helpers;

namespace TruSaber
{
    public class ScoreHelper
    {
        public float Score { get; private set; }

        public int Combo { get; private set; }

        public int ComboMultiplier
        {
            get => MathHelpers.Clamp((int) Math.Sqrt(Combo), 1, 8);
        }

        public void Reset()
        {
            Score = 0;
            Combo = 0;
        }

        public void RegisterMissedBlock()
        {
            Combo = 0;
        }

        public void RegisterHitBlock(float scoreIncrease)
        {
            Combo++;
            Score += (scoreIncrease * ComboMultiplier);
        }
    }
}