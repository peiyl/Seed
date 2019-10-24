using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FSeedItemBtn : MonoBehaviour
{
    private SeedType seedType = SeedType.FristSeed;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(()=> { GameManager.instance.SetTreeSeed(seedType, transform); });
    }
}
