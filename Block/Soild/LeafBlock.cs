using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafBlock : SolidBlock
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Destruction(Item equip)
    {
        if(equip != null)
        {
            base.Destruction(equip);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
