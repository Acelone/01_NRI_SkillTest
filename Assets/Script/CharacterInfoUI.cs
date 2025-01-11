using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUI : MonoBehaviour
{
    [Header("Character Data")]
    [SerializeField] private CharacterAction baseCharacter;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text attackInfo;
    [SerializeField] private TMP_Text defenseInfo;
    [SerializeField] private Image healthBar;
    [SerializeField] private ChargeData chargeData;

    private int attack, defense, health, charge, useCharge;

    private int Attack
    {
        get => attack;
        set
        {
            if (attack != value)
            {
                attack = value;
                UpdateAttackInfo();
            }
        }
    }

    private int Defense
    {
        get => defense;
        set
        {
            if (defense != value)
            {
                defense = value;
                UpdateDefenseInfo();
            }
        }
    }

    private int Health
    {
        get => health;
        set
        {
            if (health != value)
            {
                health = value;
                UpdateHealthBar();
            }
        }
    }

    private int Charge
    {
        get => charge;
        set
        {
            if (charge != value)
            {
                charge = value;
                UpdateChargeDisplay();
            }
        }
    }

    private int UseCharge
    {
        get => useCharge;
        set
        {
            if (useCharge != value)
            {
                useCharge = value;
                UpdateChargeUsage();
            }
        }
    }

    private void Start()
    {
        if (baseCharacter == null)
        {
            Debug.LogError("BaseCharacter is not assigned in CharacterInfoUI.");
        }
    }

    private void Update()
    {
        SyncCharacterStats();
    }
    private void SyncCharacterStats()
    {
        if (baseCharacter == null) return;

        Attack = baseCharacter.attack;
        Defense = baseCharacter.defense;
        Health = baseCharacter.health;
        Charge = baseCharacter.charge;
        UseCharge = baseCharacter.useCharge;
    }
    private void UpdateAttackInfo()
    {
        attackInfo.text = $"Attack: {attack}";
    }

    private void UpdateDefenseInfo()
    {
        defenseInfo.text = $"Defense: {defense}";
    }

    private void UpdateHealthBar()
    {
        float targetFill = (float)baseCharacter.health / baseCharacter.characterData.baseHealth;

        LeanTween.value(healthBar.gameObject, healthBar.fillAmount, targetFill, 0.5f)
            .setOnUpdate(FillHealth)
            .setEaseInOutSine();
    }
    private void FillHealth(float value)
    {
        healthBar.fillAmount = value;
    }

    private void UpdateChargeDisplay()
    {
        if (chargeData != null)
        {
            chargeData.UpdateCharge(charge);
        }
    }
    private void UpdateChargeUsage()
    {
        if (chargeData != null)
        {
            chargeData.UseCharge(useCharge);
        }
    }
}
