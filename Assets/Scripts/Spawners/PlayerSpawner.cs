using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject m_PlayerShipPrefab;

    public GameObject SpawnPlayer()
    {
        GameObject playerShip = null;

        if (DebugUtilities.Verify(m_PlayerShipPrefab != null, "Player ship prefab not assigned"))
        {
            playerShip = (GameObject)Instantiate(m_PlayerShipPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        }

        return playerShip;
    }
}