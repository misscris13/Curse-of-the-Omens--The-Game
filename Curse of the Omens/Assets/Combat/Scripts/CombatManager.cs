using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField]
    private Entity player;
    private Entity[] enemies;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void manageAttack (Entity dealer, Entity target, string dmgType, float dmg, bool isPlayer)
    {
        //target.Damage(dmgType, dmg);
    }
}
