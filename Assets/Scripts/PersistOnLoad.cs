using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistOnLoad : MonoBehaviour
{
    private static bool exists = false;

    void Awake()
    {
        if (!exists || SceneManager.GetActiveScene().buildIndex != 0)
        {
            DontDestroyOnLoad(gameObject);
            exists = true;
        }
        else
        {
            exists = false;
            Destroy(gameObject);
        }
    }
}
