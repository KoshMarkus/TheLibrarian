using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheLibrarian
{
    public class MainMenu : MonoBehaviour
    {
        //I call it after animation event of Main_Menu
    
        public void LoadFirstLevel()
        {
            SceneManager.LoadScene(1);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
