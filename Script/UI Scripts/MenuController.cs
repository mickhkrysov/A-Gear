using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject menuCanvas;

    private void Awake()
    {
        if (menuCanvas == null)
        {
            Debug.LogError("MenuController: menuCanvas is NOT assigned in the Inspector!");
        }
    }

    private void Start()
    {
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false); // start hidden
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        if (menuCanvas == null) return;

        bool showMenu = !menuCanvas.activeSelf;
        menuCanvas.SetActive(showMenu);

        // Tie into PauseController so gameplay stops when menu is open
        PauseController.SetPause(showMenu);  // uses PauseController class 
    }
}
