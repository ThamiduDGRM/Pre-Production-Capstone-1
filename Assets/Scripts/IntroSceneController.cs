using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneController : MonoBehaviour
{
    
    
    
    [SerializeField] private DialogueTyper typer;
    void Start()
    {
        typer.onDialogueFinished = LoadLevelOne;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void LoadLevelOne()
    {
        SceneManager.LoadScene("FirstLevel");
    }


}
