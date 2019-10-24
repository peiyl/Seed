using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SeedItemBtn : MonoBehaviour
{
    public SeedType seedType;
    public Image CDImage;
    public float CDTime;
    private bool isCDOver;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ThisButtonDown);
    }
    private void ThisButtonDown()
    {
        if (isCDOver)
        {
            GameManager.instance.SetTreeSeed(seedType, transform);
        }
        else
        {
            Debug.Log(seedType + ":还没有准备好");
        }
    }
    public void StartCD()
    {
        StartCoroutine(CD());
    }
    IEnumerator CD()
    {
        isCDOver = false;
        CDImage.fillAmount = 1;
        while (CDImage.fillAmount > 0)
        {
            yield return new WaitForSeconds(0.1f);
            CDImage.fillAmount -= 1/(CDTime / 0.1f);
        }
        isCDOver = true;
    }
}
