using TheKartersModdingAssistant;
using TheKartersModdingAssistant.Event;
using UnityEngine;

namespace AtLifePace.EventHandler;

public static class PlayerEventHandler {
    public static void Initialize() {
        PlayerEvent.onFixedUpdateAfter += PlayerEventHandler.OnFixedUpdateAfter;
    }

    public static void OnFixedUpdateAfter(Player player) {
        if (AtLifePace.Get().data.isAlternativeVersionEnabled) {
            PlayerEventHandler.SetReserveBasedOnHealth_Reverse(player);
        } else {
            PlayerEventHandler.SetReserveBasedOnHealth(player);
        }

        //PlayerEventHandler.SetJumpStrengthBasedOnHealth(player);
    }

    public static void SetReserveBasedOnHealth(Player player) {
        float amountOfReservePercAtMinHealth = AtLifePace.Get().data.reservePercentageAtMinimumHealth;
        float amountOfReservePercAtMaxHealth = AtLifePace.Get().data.reservePercentageAtMaximumHealth;
        float reservePercDiff = amountOfReservePercAtMaxHealth - amountOfReservePercAtMinHealth;

        int maxHealth = player.GetMaximumHealth();
        int currentHealth = player.GetCurrentHealth();

        // Linear relationship
        float a = reservePercDiff / maxHealth;
        float b = amountOfReservePercAtMinHealth;

        float newReservePerc = a * currentHealth + b;

        player.SetCurrentReserve(PlayerEventHandler.ConvertPercentageReserveToRaw(newReservePerc));
    }

    public static void SetReserveBasedOnHealth_Reverse(Player player) {
        float amountOfReservePercAtMinHealth = AtLifePace.Get().data.reservePercentageAtMinimumHealth;
        float amountOfReservePercAtMaxHealth = AtLifePace.Get().data.reservePercentageAtMaximumHealth;
        float reservePercDiff = amountOfReservePercAtMaxHealth - amountOfReservePercAtMinHealth;

        int maxHealth = player.GetMaximumHealth();
        int currentHealth = player.GetCurrentHealth();

        // Linear relationship
        float a = amountOfReservePercAtMaxHealth;
        float b = reservePercDiff / maxHealth;
        float newReservePerc = a - b * currentHealth;

        player.SetCurrentReserve(PlayerEventHandler.ConvertPercentageReserveToRaw(newReservePerc));
    }

    public static void SetJumpStrengthBasedOnHealth(Player player) {
        float jumpStrengthAtMinHealth = 21f;
        float jumpStrengthAtMaxHealth = jumpStrengthAtMinHealth * 2;
        float jumpStrengthDiff = jumpStrengthAtMaxHealth - jumpStrengthAtMinHealth;

        int maxHealth = player.GetMaximumHealth();
        int currentHealth = player.GetCurrentHealth();

        // Linear relationship
        float a = jumpStrengthDiff / maxHealth;
        float b = jumpStrengthAtMinHealth;

        float newJumpStrength = a * currentHealth + b;

        player.uPixelKartPhysics.fJumpStrength = newJumpStrength;
    }

    /// <summary>
    /// Convert percentage reserve value to raw value.
    /// 
    /// Used formulas might not be 100% precise, but they are far enough.
    /// </summary>
    /// 
    /// <param name="percReserve">float</param>
    /// <returns>float</returns>
    public static float ConvertPercentageReserveToRaw(float percReserve) {
        if (percReserve <= 140) {
            float a = -0.33991009f;
            float b = 7.87537463f;
            float c = 99.60164835f;

            float above = -b + Mathf.Sqrt(Mathf.Pow(b, 2) - 4 * a * (c - percReserve));
            float below = 2 * a;

            return above / below;
        }

        float intercept = 137.42533197f;
        float slope = 0.74320735f;

        return (percReserve - intercept) / slope;
    }
}