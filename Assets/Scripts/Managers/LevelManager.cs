using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheLibrarian.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public void LoadFirstLevel()
        {
            SceneManager.LoadScene(1);
        }
    
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void NextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
