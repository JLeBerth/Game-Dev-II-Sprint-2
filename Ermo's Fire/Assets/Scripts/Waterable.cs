using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterable : MonoBehaviour
{
    public bool isUnderWater = false;

    // Underwater setting
    private bool fog = false;
    private Color fogColor = new Color(0, 0.4f, 0.7f, 0.6f);
    private float fogDensity = 0.04f;
    [SerializeField] private Material skybox;
    [SerializeField] private GameObject water;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStats.currentState == GameState.WaterScene)
        {
            if (player.transform.position.y < water.transform.position.y && !isUnderWater)
                isUnderWater = true;
            else if (player.transform.position.y > water.transform.position.y && isUnderWater)
                isUnderWater = false;

            Underwater();
        }
    }

    private void Underwater()
    {
        if (isUnderWater)
        {
            fog = true;
            RenderSettings.fog = fog;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.skybox = skybox;
        }
        else
        {
            fog = false;
            RenderSettings.fog = fog;
        }
    }
}
