using TheKartersModdingAssistant;
using TheKartersModdingAssistant.Event;
using UnityEngine;

namespace AtLifePace.EventHandler;

public static class PlayerEventHandler {
    public static void Initialize() {
        PlayerEvent.onFixedUpdateAfter += PlayerEventHandler.OnFixedUpdateAfter;
    }

    public static void OnFixedUpdateAfter(Player player) {
        // 17 raw reserve is ~150%.
        float amountOfReservePercAtMinHealth = 100;
        float amountOfReservePercAtMaxHealth = 150;
        float reservePercDiff = amountOfReservePercAtMaxHealth - amountOfReservePercAtMinHealth;
        int maxHealth = player.GetMaximumHealth();
        int currentHealth = player.GetCurrentHealth();

        // Linear relationship
        float a = reservePercDiff / maxHealth;
        float b = amountOfReservePercAtMinHealth;

        float newReservePerc = a * currentHealth + b;

        player.SetCurrentReserve(PlayerEventHandler.ConvertPercentageReserveToRaw(newReservePerc));
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