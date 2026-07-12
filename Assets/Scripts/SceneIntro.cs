using UnityEngine;

public class SceneIntro : MonoBehaviour
{
    [SerializeField] private DialogueTyper typer;

    [TextArea(3, 10)]
    public string introText = 
    "In the distant future They is a evil dictator ruling viciously over people of the city one day he thought this wasn't enough and he had to conquer more so he devised a plan to take over all of time In Present day they is a young hero who hears word of this devious plan does this hero have the strength and will power to stop this time lord and his goons We'll have to see";

    private void Start()
    {
        typer.StartDialogue(introText);
    }
}

