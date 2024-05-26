using System.Collections;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using HarmonyLib;
using TheKartersModdingAssistant;
using UnityEngine;

namespace AtLifePace.Core;

[HarmonyPatch(typeof(HpBarController), nameof(HpBarController.Hit))]
public class HpBarController__Hit {
    public static bool Prefix(HpBarController __instance, int damage, int playerMakingDamage, PixelWeaponObject.EWeaponType eWeaponType) {
        if (!AtLifePace.Get().data.isAlternativeVersionEnabled) {
            return true;
        }
        
        if (!__instance.isImmune  && !__instance.deathImmune) {
            __instance.RefillHp(damage, false);
        }

        //Player targetPlayer = Player.FindByAntPlayer(__instance.player);
        Player authorPlayer = Player.FindByIndex((Ant_Player.EAntPlayerNumber)playerMakingDamage);
        HpBarController authorHpBarController = authorPlayer.uHpBarController;

        if (authorPlayer is null) {
            AtLifePace.Get().logger.Error($"(HpBarController::Hit Postfix) Player {playerMakingDamage} not found.");
            return false;
        }

        HpBarController__Helper.Hit(authorHpBarController, damage, playerMakingDamage, eWeaponType);

        return false;
    }
}

[HarmonyPatch(typeof(HpBarController), nameof(HpBarController.HealthBotStartRefilingHealth))]
public class HpBarController__HealthBotStartRefilingHealth {
    public static void Postfix(HpBarController __instance, int hpToRefill, float duration) {
        if (!AtLifePace.Get().data.isAlternativeVersionEnabled) {
            return;
        }

        __instance.hpRefillCoroutine = __instance.StartCoroutine(HpBarController__Helper.HpAddC(__instance, hpToRefill, duration).WrapToIl2Cpp());
    }
}

[HarmonyPatch(typeof(HpBarController), nameof(HpBarController.HpAddC))]
public class HpBarController__HpAddC {
    public static bool Prefix() {
        return !AtLifePace.Get().data.isAlternativeVersionEnabled;
    }
}

public static class HpBarController__Helper {
    public static void Hit(HpBarController uHpBarController, int damage, int playerMakingDamage, PixelWeaponObject.EWeaponType eWeaponType) {
        uHpBarController.eLastHitFromWeaponType = eWeaponType;
        uHpBarController.whoDamagedLast = playerMakingDamage;

        if ((bool) Ant_CurrentGameConfiguration.IsOnlineGame_InRoom_WithInternet
            && !uHpBarController.player.visualInstanceSyncedParams.photonView.IsMine) {
            return;
        }

        if (!uHpBarController.isImmune && !uHpBarController.deathImmune) {
            uHpBarController.whoDamagedUs[playerMakingDamage] = true;

            int finalDamage;

            if (damage <= (int)uHpBarController.currentHp) {
                finalDamage = damage;
            } else {
                finalDamage = (int)uHpBarController.currentHp;
            }

            if (finalDamage > 0) {
                uHpBarController.currentHp = (int)uHpBarController.currentHp - finalDamage;

                if (uHpBarController.iHealthBotRefilling_HpToRefillLeft > 0 && (int)uHpBarController.currentHp <= 5) {
                    uHpBarController.currentHp = 5;
                }

                uHpBarController.CheckHp();
            }
        }

        uHpBarController.RefreshVisualInstHPParams();
    }

    public static IEnumerator HpAddC(HpBarController uHpBarController, int hpToRefill, float duration) {
        float timer = 0.0f;
        int hpRefilled = 0;
        bool bUseHPReffilingEveryTick = true;

        while ((double)timer < (double)duration && bUseHPReffilingEveryTick) {
            timer += Time.deltaTime;

            int hp = Mathf.Clamp(Mathf.FloorToInt(hpToRefill * (timer / duration)), 0, hpToRefill) - hpRefilled;
            
            hpRefilled += hp;
            uHpBarController.iHealthBotRefilling_HpToRefillLeft = hpToRefill - hpRefilled;
            //uHpBarController.RefillHp(hp, true);

            HpBarController__Helper.Hit(
                uHpBarController,
                hp,
                (int)uHpBarController.player.eAntPlayerNr,
                (PixelWeaponObject.EWeaponType)Item.HEAL_BOT
            );

            yield return (object)null;
        }

        uHpBarController.iHealthBotRefilling_HpToRefillLeft = hpToRefill - hpRefilled;

        int refillingHpToRefillLeft = uHpBarController.iHealthBotRefilling_HpToRefillLeft;

        if (refillingHpToRefillLeft > 0) {
            //uHpBarController.RefillHp(refillingHpToRefillLeft, true);

            HpBarController__Helper.Hit(
                uHpBarController,
                refillingHpToRefillLeft,
                (int)uHpBarController.player.eAntPlayerNr,
                (PixelWeaponObject.EWeaponType)Item.HEAL_BOT
            );
        }
            
        uHpBarController.iHealthBotRefilling_HpToRefillLeft = 0;
    }
}