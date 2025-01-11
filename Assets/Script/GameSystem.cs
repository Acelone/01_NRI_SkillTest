using System.Collections;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance { get; private set; }

    [Header("Game State")]
    public bool endGame;
    public bool win;
    [SerializeField] private CharacterData.CharacterType currentTurn;
    public CharacterAction player;
    public CharacterAction enemy;

    public EndGameConfig endGameConfig;
    public CanvasGroup playerCanvas;

    private CharacterAction currentPlayer;
    [HideInInspector]public int turnCount;

    public CharacterData.CharacterType CurrentTurn
    {
        get => currentTurn;
        set
        {
            if (currentTurn != value)
            {
                currentTurn = value;
                HandleTurnChange(currentTurn); 
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Invoke(nameof(UpdateTurn), 3);

    }
    public void UpdateTurn()
    {
        if (endGame) 
        {
            endGameConfig.EndGame();
            return;
        } 

        StartCoroutine(TurnRoutine());
    }
    private IEnumerator TurnRoutine()
    {
        turnCount++;
        ShowTurnNotification();

        yield return new WaitForSeconds(2);

        CurrentTurn = CurrentTurn == CharacterData.CharacterType.Player
            ? CharacterData.CharacterType.Enemy
            : CharacterData.CharacterType.Player;
    }
    private void ShowTurnNotification()
    {
        string message = CurrentTurn == CharacterData.CharacterType.Player
            ? "Enemy's Turn"
            : "Your Turn";

        NotificationSystem.Instance.ShowNotification(message, 2);
    }
    private void HandleTurnChange(CharacterData.CharacterType turn)
    {
        switch (turn)
        {
            case CharacterData.CharacterType.Player:
                currentPlayer = player;
                playerCanvas.interactable = true;
                ProcessPlayerTurn();
                break;

            case CharacterData.CharacterType.Enemy:
                currentPlayer = enemy;
                playerCanvas.interactable = false;
                ProcessEnemyTurn();
                break;
        }
    }
    private void ProcessPlayerTurn()
    {
        currentPlayer.AddCharge();
    }
    private void ProcessEnemyTurn()
    {
        currentPlayer.AddCharge();
        Invoke(nameof(EnemyTurn), 1);
    }
    private void EnemyTurn()
    {
        currentPlayer.CharacterTurn();
    }
}
