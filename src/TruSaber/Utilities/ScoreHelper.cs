using System;
using RocketUI.Utilities.Helpers;

namespace TruSaber
{
    public class ScoreHelper
    {
        public float Score { get; private set; }

        public int Combo { get; private set; }

        public int ComboMultiplier { get; private set; }
        public void Reset()
        {
            Score = 0;
            Combo = 0;
            UpdateComboMultiplier();
        }

        public void RegisterMissedBlock()
        {
            Combo = 0;
            UpdateComboMultiplier();
        }

        public void RegisterHitBlock(float scoreIncrease)
        {
            Combo++;
            UpdateComboMultiplier();
            Score += (scoreIncrease * ComboMultiplier);
        }

        private void UpdateComboMultiplier()
        {
            ComboMultiplier = CalculateComboMultiplier();
        }
        
        private int CalculateComboMultiplier()
        {
            if (Combo >= 16)
                return 16;
            if (Combo >= 8)
                return 8;
            if (Combo >= 6)
                return 6;
            if (Combo >= 4)
                return 4;
            if (Combo >= 2)
                return 2;
            
            return 1;
        }
    }
}