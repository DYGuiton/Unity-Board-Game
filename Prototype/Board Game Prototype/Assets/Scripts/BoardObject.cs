using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardObject: MonoBehaviour {
    public Material originalMaterial;

    public abstract bool Highlight();

    public abstract bool Highlight(Material highlightMaterial);

    public abstract void Unhighlight();

}