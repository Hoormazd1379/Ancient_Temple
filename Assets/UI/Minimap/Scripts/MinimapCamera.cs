using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [Header("Minimap rotations")]
    public Transform playerReference;
    public float playerOffset = 10f;

    [Header("Minimap scenemanager")]
    public SceneManager scenemanager;

    private void Update()
    {
        if (scenemanager != null) {
            int size = scenemanager.gameLevelNumber;
            size = Mathf.Clamp(size, 20, 90);
            GetComponent<Camera>().orthographicSize = size;
        }
        if (playerReference != null)
        {
            transform.position = new Vector3(playerReference.position.x, playerReference.position.y + playerOffset, playerReference.position.z);
            transform.rotation = Quaternion.Euler(90f, playerReference.eulerAngles.y, 0f);
        }
    }
}
