using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourutineHandler : MonoBehaviour
{
    public void LevelCompletedORGameOvr(GameObject panel)
    {
        StartCoroutine(LevelCompletedORGameOver(panel));
    }
    IEnumerator LevelCompletedORGameOver(GameObject panel)
    {
        Debug.Log("CAll"+ panel.name);
        yield return new WaitForSeconds(1.5f);
        Debug.Log("CAwwwll" + panel.name);
        panel.SetActive(true);
    }


}
