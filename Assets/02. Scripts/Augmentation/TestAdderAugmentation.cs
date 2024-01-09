using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAdderAugmentation : MonoBehaviour
{
    public PlayableCtrl player;
    
    void Start()
    {
        player.AddAugmentation(new PoisonField(player, 1, AugmentationEventType.ON_START));
    }
}
