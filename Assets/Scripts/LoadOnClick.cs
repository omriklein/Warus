using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour
{

    public GameObject LoadingImage;
    public GameObject TutorialsImage;

    void Awake()
    {
        if (LoadingImage != null)
            LoadingImage.SetActive(false);
        if(TutorialsImage != null)
            TutorialsImage.SetActive(false);
    }

    public void LoadScene(int level)
    {
        if (level == -1)
        {
            Application.Quit();
            return;
        }

        if (level == 999)
        {
            TutorialsImage.SetActive(true);
            return;
        }

        if (LoadingImage != null)
            LoadingImage.SetActive(true);

        Application.LoadLevel(level);
    }

}
