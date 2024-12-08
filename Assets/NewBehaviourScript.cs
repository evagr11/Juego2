using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] TMP_Text objectiveUI;
    [SerializeField] TMP_Text scoreUI;
    [SerializeField] TMP_Text nextLevelUI;
    [SerializeField] TMP_Text levelNumberUI;
    [SerializeField] GameObject recordUI;  // UI para mostrar el récord
    [SerializeField] TMP_Text recordTextUI;
    [SerializeField] TMP_Text instructionsUI;  // Añadimos la referencia a las instrucciones
    [SerializeField] GameObject victoryScreenUI;
    [SerializeField] GameObject defeatScreenUI;
    [SerializeField] float timer = 0;
    [SerializeField] public float timeResetDuration = 1;
    private int counter = 0;
    private int objective;
    private int levelNumber = 0;

    private float pauseTimer = 0f;  // Temporizador para la pausa
    private bool isGamePaused = false;  // Control de la pausa

    private bool gameStarted = false;  // Control de inicio del juego

    private static int highestLevel = 0;  // Variable estática para guardar el nivel más alto

    void Start()
    {
        // Mostrar el récord de nivel en la UI
        recordTextUI.text = highestLevel.ToString();

        // Mostrar instrucciones al inicio
        instructionsUI.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!gameStarted)  // Si el juego no ha comenzado
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                instructionsUI.gameObject.SetActive(false);
                scoreUI.gameObject.SetActive(true);
                objectiveUI.gameObject.SetActive(true);
                gameStarted = true;
                RestartGame();
            }
            return;
        }

        // Lógica normal del juego
        if (isGamePaused)
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= 1f) ResumeGame();
            return;
        }

        timer += Time.deltaTime;

        if (timer > timeResetDuration)
        {
            timer = 0;
            counter++;
            scoreUI.text = counter.ToString();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ValidateVictory();
        }

        if (counter == 15)
        {
            ValidateVictory();
        }
    }

    void RestartGame()
    {
        counter = 0;
        scoreUI.text = counter.ToString();
        objective = Random.Range(1, 15);
        objectiveUI.text = objective.ToString();

        victoryScreenUI.SetActive(false);
        defeatScreenUI.SetActive(false);
        nextLevelUI.gameObject.SetActive(false);
        levelNumberUI.gameObject.SetActive(false);
    }

    void ValidateVictory()
    {
        if (counter == objective)
        {
            levelNumber++;

            // Ajustar timeReset según el número de nivel
            if (levelNumber == 1)
            {
                timeResetDuration = 1;  // Restablecer el valor de timeReset a 1 cuando comenzamos el primer nivel
            }

            levelNumberUI.text = levelNumber.ToString();
            nextLevelUI.gameObject.SetActive(true);
            levelNumberUI.gameObject.SetActive(true);
            victoryScreenUI.SetActive(true);
            recordUI.gameObject.SetActive(true);
            recordTextUI.gameObject.SetActive(true);

            CheckAndUpdateRecord();
            AdjustTimeReset();
            PauseGame();
        }
        else
        {
            defeatScreenUI.SetActive(true);
            levelNumber = 0;  // Nivel vuelve a 0 cuando se pierde

            // Restablecer timeReset a 1 cuando se pierde y el nivel vuelve a 0
            timeResetDuration = 1;

            PauseGame();
        }
    }

    private void AdjustTimeReset()
    {
        timeResetDuration -= 0.1f;
    }

    private void PauseGame()
    {
        isGamePaused = true;
        pauseTimer = 0f;
    }

    private void ResumeGame()
    {
        isGamePaused = false;
        RestartGame();
    }

    private void CheckAndUpdateRecord()
    {
        if (levelNumber > highestLevel)
        {
            highestLevel = levelNumber;  // Actualizar el récord en memoria
            recordTextUI.text = highestLevel.ToString();  // Actualizar la UI con el nuevo récord
        }
    }
}

