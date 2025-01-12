using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterAction : MonoBehaviour
{
    [Header("Character Data")]
    public CharacterData characterData;
    public int health;
    public int attack;
    public int defense;
    public int charge;

    [Header("References")]
    public Animator characterAnimator;
    public GameObject ultimateEffect;
    public CharacterAction target;

    [Header("Modifiers")]
    public List<AddedModifier> characterModifiers;
    public int useCharge;

    [System.Serializable]
    public class AddedModifier
    {
        public string modifierName;
        public int attack;
        public int defense;
        public int duration;
    }

    private void Awake()
    {
        InitializeCharacterStats();
    }

    private void Update()
    {
        HandlePlayerInput();
        CheckHealth();
    }

    private void InitializeCharacterStats()
    {
        health = characterData.baseHealth;
        attack = characterData.baseAttack;
        defense = characterData.baseDefense;
    }

    private void CheckHealth()
    {
        if (health <= 0 && !GameSystem.Instance.endGame)
        {
            health = 0;
            GameSystem.Instance.endGame = true;
            characterAnimator.SetTrigger("Lose");
        }
        else if (GameSystem.Instance.endGame && health > 0)
        {
            characterAnimator.SetTrigger("Win");
            if (characterData.characterType == CharacterData.CharacterType.Player)
                GameSystem.Instance.win = true;
        }
    }
    public void AdjustUseCharge(int adder)
    {
        if (characterData.characterType == CharacterData.CharacterType.Player)
        {
            // Clamp useCharge within valid bounds
            useCharge = Mathf.Clamp(useCharge + adder, 0, Mathf.Min(3, charge));
        }
    }

    private void HandlePlayerInput()
    {
        if (characterData.characterType == CharacterData.CharacterType.Player)
        {
            // Keyboard input handling
            if (Input.GetKeyDown(KeyCode.E))
            {
                AdjustUseCharge(1); // Increment useCharge
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                AdjustUseCharge(-1); // Decrement useCharge
            }
        }
    }

    public void CharacterTurn()
    {
        characterAnimator.SetBool("Defense", false);
        HandleModifiers();

        if (characterData.characterType == CharacterData.CharacterType.Enemy)
        {
            HandleEnemyTurn();
        }
        else
        {
            HandlePlayerTurn();
        }
    }

    public void AddCharge()
    {
        if (charge < 5)
        {
            charge++;
        }
    }

    private void HandleModifiers()
    {
        List<AddedModifier> expiredModifiers = new List<AddedModifier>();

        foreach (AddedModifier modifier in characterModifiers)
        {
            modifier.duration--;
            if (modifier.duration <= 0)
            {
                if (modifier.modifierName == ModifierContainer.instance.attackBuff.modifierName)
                {
                    UpdateSwordShader(0);
                }
                attack -= modifier.attack;
                defense -= modifier.defense;
                expiredModifiers.Add(modifier);
            }
        }

        foreach (AddedModifier modifier in expiredModifiers)
        {
            characterModifiers.Remove(modifier);
        }
    }

    private void HandleEnemyTurn()
    {
        float randMove = StaticFunction.TrueRandom(0f, 100f);

        if (randMove < GetThreshold("heal") && charge >= 2)
        {
            Notification("Enemy is healing itself!");
            Heal();
        }
        else if (randMove < GetThreshold("attack") && charge >= 1)
        {
            Notification("Enemy is buffing its attack!");
            AttackUp();
        }
        else if (randMove < GetThreshold("defense"))
        {
            charge--;
            DefenseUp();
        }
        else if (charge >= 3)
        {
            Notification("Enemy unleashes its ultimate attack!");
            UltimateAttack();
        }
        else
        {
            Notification("Enemy is attacking!");
            BasicAttack();
        }
    }

    private float GetThreshold(string action)
    {
        bool isLowHealth = health < characterData.baseHealth / 3;

        return action switch
        {
            "heal" => isLowHealth ? 80 : 15,
            "defense" => isLowHealth ? 60 : 30,
            "attack" => isLowHealth ? 90 : 50,
            _ => 0,
        };
    }

    private void HandlePlayerTurn()
    {
        switch (useCharge)
        {
            case 3:
                Notification("You unleash your ultimate attack!");
                UltimateAttack();
                break;
            case 2:
                Notification("You are healing yourself!");
                Heal();
                break;
            case 1:
                Notification("You are buffing your attack!");
                AttackUp();
                break;
            default:
                Notification("You are attacking!");
                BasicAttack();
                break;
        }
    }

    private void UpdateSwordShader(float value)
    {
        Material swordMat = ultimateEffect.transform.parent.GetComponent<MeshRenderer>().material;
        swordMat.SetFloat("_GradientNoiseScale", value);
    }

    private void UseCharge(int cost)
    {
        charge -= cost;
    }

    public void Notification(string message)
    {
        NotificationSystem.Instance.ShowNotification(message, 2);
    }
    public void BasicAttack()
    {
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(2);
        characterAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        target.ReceiveDamage(attack);
        yield return new WaitForSeconds(2);
        UpdateTurn();
    }

    private void UpdateTurn()
    {
        useCharge = 0;
        GameSystem.Instance.UpdateTurn();
    }

    public void ReceiveDamage(int damageAmount)
    {
        characterAnimator.SetTrigger("Hit");
        int damage = Mathf.Max(Mathf.CeilToInt(damageAmount - defense), 0);
        health -= damage;
    }

    public void AttackUp()
    {
        UseCharge(1);
        ApplyModifier(ModifierContainer.instance.attackBuff, 15);
        Invoke(nameof(UpdateTurn), 2);
    }

    public void DefenseUp()
    {
        Notification("Defending...");
        UseCharge(1);
        ApplyModifier(ModifierContainer.instance.defenseBuff);
        Invoke(nameof(UpdateTurn), 2);
    }

    public void Heal()
    {
        UseCharge(2);
        StartCoroutine(HealRoutine());
    }

    private IEnumerator HealRoutine()
    {
        yield return new WaitForSeconds(2);
        health = Mathf.Min(health + 20, characterData.baseHealth);
        UpdateTurn();
    }

    public void UltimateAttack()
    {
        UseCharge(3);
        StartCoroutine(UltimateAttackRoutine());
    }

    private IEnumerator UltimateAttackRoutine()
    {
        yield return new WaitForSeconds(2);
        ultimateEffect.GetComponent<VisualEffect>().SetFloat("SpawnRate", 16);
        ApplyModifier(ModifierContainer.instance.ultimateAttack, 0);
        characterAnimator.SetTrigger("Ultimate");
        yield return new WaitForSeconds(1);
        target.ReceiveDamage(attack);
        yield return new WaitForSeconds(2);
        ultimateEffect.GetComponent<VisualEffect>().SetFloat("SpawnRate", 0);
        UpdateTurn();
    }

    private void ApplyModifier(StatsModifier modifierData, float shaderValue = 0)
    {
        foreach (var modifier in characterModifiers)
        {
            if (modifier.modifierName == modifierData.modifierName)
            {
                modifier.duration = modifierData.duration;
                return;
            }
        }

        if (shaderValue > 0) UpdateSwordShader(shaderValue);

        var newModifier = new AddedModifier
        {
            modifierName = modifierData.modifierName,
            duration = modifierData.duration,
            attack = modifierData.AttackUp(characterData.baseAttack),
            defense = modifierData.DefenseUpBuff(characterData.baseDefense)
        };

        characterModifiers.Add(newModifier);
        attack += newModifier.attack;
        defense += newModifier.defense;
    }
}
