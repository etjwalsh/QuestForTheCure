using UnityEngine;
using System.Collections.Generic;
using System;

public enum PharmaceuticalElement
{
    Hydrogen, 
    Carbon,   
    Oxygen,   
    Nitrogen, 
    Fluorine, 
    Chlorine, 
    Happy,         
    Sad
}


[System.Serializable]
public class MoleculeSprite
{
    public PharmaceuticalElement element1;
    public PharmaceuticalElement element2;
    public Sprite moleculeSprite;
}

public class MoleculeManager : MonoBehaviour
{
    [Header("Bonding Settings")]
    [Tooltip("How close elements need to be to bond (in world units)")]
    public float bondDistance = 1.5f;

    [Header("Molecule Sprites")]
    [Tooltip("Assign sprites for each molecule combination")]
    public List<MoleculeSprite> moleculeSprites = new List<MoleculeSprite>();

    private List<MoveElements> allElements = new List<MoveElements>();
    private Dictionary<string, GameObject> formedMolecules = new Dictionary<string, GameObject>();
    public GameObject explosionParticles; //reference to particle system

    void Start()
    {
        // Find all MoveElements components in the scene
        allElements.AddRange(FindObjectsOfType<MoveElements>());
    }

    void Update()
    {
        CheckProximityAndBond();
    }

    void CheckProximityAndBond()
    {
        // Check every pair of elements
        for (int i = 0; i < allElements.Count; i++)
        {
            for (int j = i + 1; j < allElements.Count; j++)
            {
                MoveElements element1 = allElements[i];
                MoveElements element2 = allElements[j];

                // Skip if either element is null or already part of a molecule
                if (element1 == null || element2 == null) continue;
                if (!element1.gameObject.activeSelf || !element2.gameObject.activeSelf) continue;

                float distance = Vector3.Distance(element1.transform.position, element2.transform.position);

                // Check if elements are close enough to bond
                if (distance < bondDistance)
                {
                    CombineElements(element1, element2);
                }
            }
        }
    }

    void CombineElements(MoveElements element1, MoveElements element2)
    {
        if (element1.isMolecule || element2.isMolecule)
        {
            //do not combine
            return;
        }
        // Check if this is a valid combination
        if (!IsValidCombination(element1.elementType, element2.elementType))
        {
            // Not a valid combination - elements just bounce off each other
            return;
        }

        // Get the midpoint between the two elements
        Vector3 midpoint = (element1.transform.position + element2.transform.position) / 2f;

        // Find the appropriate sprite for this combination
        Sprite moleculeSprite = GetMoleculeSprite(element1.elementType, element2.elementType);

        // Create new molecule GameObject
        GameObject molecule = new GameObject($"Molecule_{element1.elementType}_{element2.elementType}");
        molecule.transform.position = midpoint;

        // Add SpriteRenderer with the molecule sprite
        SpriteRenderer sr = molecule.AddComponent<SpriteRenderer>();
        sr.sprite = moleculeSprite;
        sr.sortingOrder = 1; // Above individual elements

        // Add collider for dragging
        molecule.AddComponent<BoxCollider2D>();

        // Add MoveElements component so it can be dragged
        MoveElements moleculeDrag = molecule.AddComponent<MoveElements>();
        moleculeDrag.elementType = element1.elementType; // Store first element type for reference
        moleculeDrag.dragSmoothness = element1.dragSmoothness;
        moleculeDrag.slideDeceleration = element1.slideDeceleration;
        moleculeDrag.stopThreshold = element1.stopThreshold;
        moleculeDrag.constrainToCameraBounds = element1.constrainToCameraBounds;
        moleculeDrag.boundaryPadding = element1.boundaryPadding;
        moleculeDrag.isMolecule = true;

        // Add a component to track what elements made this molecule
        MoleculeData moleculeData = molecule.AddComponent<MoleculeData>();
        moleculeData.element1Type = element1.elementType;
        moleculeData.element2Type = element2.elementType;

        // Disable the original elements
        element1.gameObject.SetActive(false);
        element2.gameObject.SetActive(false);

        // Add to elements list so it can bond with other things
        allElements.Add(moleculeDrag);

        // Handle molecule effects based on combination
        HandleMoleculeFormation(element1.elementType, element2.elementType, molecule);
    }

    bool IsValidCombination(PharmaceuticalElement type1, PharmaceuticalElement type2)
    {
        // Normalize order for checking
        PharmaceuticalElement smaller = type1;
        PharmaceuticalElement larger = type2;

        if ((int)type1 > (int)type2)
        {
            smaller = type2;
            larger = type1;
        }

        // List of all 21 valid combinations
        // Same element bonds
        if (smaller == larger)
        {
            return (smaller == PharmaceuticalElement.Hydrogen ||
                    smaller == PharmaceuticalElement.Carbon ||
                    smaller == PharmaceuticalElement.Oxygen ||
                    smaller == PharmaceuticalElement.Nitrogen ||
                    smaller == PharmaceuticalElement.Fluorine ||
                    smaller == PharmaceuticalElement.Chlorine);
        }

        // Hydrogen bonds (H is always first)
        if (smaller == PharmaceuticalElement.Hydrogen)
        {
            return true; // H bonds with everything
        }

        // Carbon bonds
        if (smaller == PharmaceuticalElement.Carbon)
        {
            return (larger == PharmaceuticalElement.Oxygen ||
                    larger == PharmaceuticalElement.Nitrogen ||
                    larger == PharmaceuticalElement.Fluorine ||
                    larger == PharmaceuticalElement.Chlorine);
        }

        // Oxygen bonds
        if (smaller == PharmaceuticalElement.Oxygen)
        {
            return (larger == PharmaceuticalElement.Nitrogen ||
                    larger == PharmaceuticalElement.Fluorine ||
                    larger == PharmaceuticalElement.Chlorine);
        }

        // Nitrogen bonds
        if (smaller == PharmaceuticalElement.Nitrogen)
        {
            return (larger == PharmaceuticalElement.Fluorine ||
                    larger == PharmaceuticalElement.Chlorine);
        }

        // Fluorine-Chlorine bond
        if (smaller == PharmaceuticalElement.Fluorine && larger == PharmaceuticalElement.Chlorine)
        {
            return true;
        }

        // If we got here, it's not a valid combination
        Debug.Log($"INVALID COMBINATION: {type1} + {type2} - Not bonding!");
        return false;
    }

    Sprite GetMoleculeSprite(PharmaceuticalElement type1, PharmaceuticalElement type2)
    {
        // Check both orderings since we don't know which order they'll be in
        foreach (MoleculeSprite ms in moleculeSprites)
        {
            if ((ms.element1 == type1 && ms.element2 == type2) ||
                (ms.element1 == type2 && ms.element2 == type1))
            {
                return ms.moleculeSprite;
            }
        }

        // Return null if no sprite found (you should see this in console)
        Debug.LogWarning($"No sprite found for {type1}-{type2} molecule!");
        return null;
    }

    // ====================================================================
    // CUSTOMIZE YOUR MOLECULE REACTIONS HERE
    // ====================================================================
    void HandleMoleculeFormation(PharmaceuticalElement type1, PharmaceuticalElement type2, GameObject molecule)
    {
        // Make sure we always check in the same order
        if ((int)type1 > (int)type2)
        {
            PharmaceuticalElement temp = type1;
            type1 = type2;
            type2 = temp;
        }

        Debug.Log("this is type 1 " + type1 + " and this is type 2" + type2);

        // SAFE/BENEFICIAL COMBINATIONS
        if (type1 == PharmaceuticalElement.Carbon && type2 == PharmaceuticalElement.Hydrogen)
        {
            Debug.Log("Formed C-H bond: Hydrocarbon base (safe, basic building block)");
            // Add your effects here - maybe green glow, positive sound
        }
        else if (type1 == PharmaceuticalElement.Carbon && type2 == PharmaceuticalElement.Oxygen)
        {
            Debug.Log("Formed C-O bond: Alcohol/Ether group (safe, common in drugs)");
            // Add effects
        }
        else if (type1 == PharmaceuticalElement.Carbon && type2 == PharmaceuticalElement.Nitrogen)
        {
            Debug.Log("Formed C-N bond: Amine group (safe, found in many medications)");
            // Add effects
        }
        else if (type1 == PharmaceuticalElement.Hydrogen && type2 == PharmaceuticalElement.Oxygen)
        {
            Debug.Log("Formed H-O bond: Hydroxyl group (safe, common functional group)");
            // Add effects
        }

        // TOXIC COMBINATIONS
        else if (type1 == PharmaceuticalElement.Fluorine && type2 == PharmaceuticalElement.Chlorine)
        {
            Debug.Log("TOXIC: Cl-F bond creates unstable halogen compound!");
            // Add toxic effect - maybe red glow, damage player, warning sound
            StartCoroutine(ToxicEffect(molecule));
        }
        else if (type1 == PharmaceuticalElement.Chlorine && type2 == PharmaceuticalElement.Nitrogen)
        {
            Debug.Log("TOXIC: Chloramine formed! Respiratory irritant!");
            StartCoroutine(ToxicEffect(molecule));
        }
        else if (type1 == PharmaceuticalElement.Fluorine && type2 == PharmaceuticalElement.Hydrogen)
        {
            Debug.Log("TOXIC: Hydrofluoric acid! Extremely corrosive!");
            StartCoroutine(ToxicEffect(molecule));
        }

        // EXPLOSIVE COMBINATIONS
        else if (type1 == PharmaceuticalElement.Nitrogen && type2 == PharmaceuticalElement.Oxygen)
        {
            Debug.Log("EXPLOSIVE: Nitrogen oxide! Unstable oxidizer!");
            StartCoroutine(ExplosiveEffect(molecule));
        }
        else if (type1 == PharmaceuticalElement.Hydrogen && type2 == PharmaceuticalElement.Chlorine)
        {
            Debug.Log("EXPLOSIVE: Hydrogen chloride under pressure!");
            StartCoroutine(ExplosiveEffect(molecule));
        }
        else if (type1 == PharmaceuticalElement.Fluorine && type2 == PharmaceuticalElement.Oxygen)
        {
            Debug.Log("EXPLOSIVE: Oxygen difluoride! Highly reactive oxidizer!");
            StartCoroutine(ExplosiveEffect(molecule));
        }

        // DEFAULT - EXPERIMENTAL/UNKNOWN
        else
        {
            Debug.Log($"Formed {type1}-{type2} bond: Experimental compound (unknown effects)");
        }
    }

    System.Collections.IEnumerator ToxicEffect(GameObject molecule)
    {
        SpriteRenderer sr = molecule.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // Pulse red color
            while (true)
            {
                sr.color = Color.red;
                yield return new WaitForSeconds(0.3f);
                sr.color = Color.white;
                yield return new WaitForSeconds(0.3f);
            }
        }
        // Add more effects here: particles, sounds, screen effects, etc.
    }

    System.Collections.IEnumerator ExplosiveEffect(GameObject molecule)
    {
        SpriteRenderer sr = molecule.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // Flash and shake
            Vector3 originalPos = molecule.transform.position;
            sr.color = Color.yellow;

            for (int i = 0; i < 10; i++)
            {
                molecule.transform.position = originalPos + (Vector3)UnityEngine.Random.insideUnitCircle * 0.1f;
                yield return new WaitForSeconds(0.05f);
            }

            // Explode!
            sr.color = Color.red;
            molecule.transform.localScale = Vector3.one * 2f;
            yield return new WaitForSeconds(0.2f);

            // Destroy the molecule and explode
            Destroy(molecule);
            Instantiate(explosionParticles, originalPos, Quaternion.identity);
        }
        // Add more effects: particle explosion, camera shake, sound, etc.
    }
}

// Component to store what elements created this molecule
public class MoleculeData : MonoBehaviour
{
    public PharmaceuticalElement element1Type;
    public PharmaceuticalElement element2Type;
}