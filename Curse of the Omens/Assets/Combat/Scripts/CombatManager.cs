using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField]
    private Entity playerObject;
    
    // Start is called before the first frame update
    void Start()
    {
        // recover player stats like ult points, hp and mana points
        // just for testing:
        playerObject.hp = 50;
        playerObject.mana = 50;
        playerObject.ult = 0;
        playerObject.maxUlt = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void manageAttack (Entity dealer, Entity target, string dmgType, float dmg, bool isPlayer)
    {
        //target.Damage(dmg);

        if (dealer.ult < dealer.maxUlt)
        {
            dealer.ult++;
        }
        
        //target.Damage(dmgType, dmg);
    }
}
