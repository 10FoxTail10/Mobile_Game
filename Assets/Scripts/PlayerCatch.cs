using UnityEngine;

public class PlayerCatch : MonoBehaviour
{
    [SerializeField] private Transform safeRoomSpawn;
    [SerializeField] private Transform bot;
    [SerializeField] private float catchDistance = 1f;
    [SerializeField] private GameObject gameOverPanel;

    private CharacterController _controller;
    private int _catchCount = 0;
    private bool _isGameOver = false;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (_isGameOver)
        {
            return;
        }

        Catch();
    }

    private void Catch()
    {
        float distance = Vector3.Distance(transform.position, bot.position);

        if (distance <= catchDistance)
        {
            _controller.enabled = false;
            transform.position = safeRoomSpawn.position;
            _controller.enabled = true;
            _catchCount++;
        }
        ConsumeAttempt();
    }

    public void ConsumeAttempt()
    {
        if (_isGameOver)
        {
            return;
        }

        _catchCount++;

        Debug.Log($"Потеряна попытка. Использовано: {_catchCount} из 3.");

        if (_catchCount >= 3)
        {
            _isGameOver = true;
            gameOverPanel.SetActive(true);
        }
    }

}
