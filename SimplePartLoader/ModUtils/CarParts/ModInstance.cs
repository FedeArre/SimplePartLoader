﻿using SimplePartLoader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SimplePartLoader
{
    public class ModInstance
    {
        private Mod thisMod;
        private List<Part> loadedParts;
        private ModSettings settings;

        public List<Part> Parts
        {
            get { return loadedParts; }
            internal set { loadedParts = value; }
        }

        public Mod Mod
        {
            get { return thisMod; }
        }

        public ModSettings Settings
        {
            get { return settings; }
        }

        public string Name
        {
            get { return Mod.Name;  }
        }
        
        internal ModInstance(Mod mod)
        {
            thisMod = mod;
            loadedParts = new List<Part>();
            settings = new ModSettings(this);

            Debug.Log($"[ModUtils/RegisteredMods]: Succesfully registered " + mod.Name);
        }

        public Part Load(AssetBundle bundle, string prefabName)
        {
            // Safety checks
            if (!bundle)
                SPL.SplError("Tried to create a part without valid AssetBundle");

            if (String.IsNullOrWhiteSpace(prefabName))
                SPL.SplError("Tried to create a part without prefab name");

            if (Saver.modParts.ContainsKey(prefabName))
                SPL.SplError($"Tried to create an already existing prefab ({prefabName})");

            GameObject prefab = bundle.LoadAsset<GameObject>(prefabName);
            if (!prefab)
                SPL.SplError($"Tried to create a prefab but it was not found in the AssetBundle ({prefabName})");

            GameObject.DontDestroyOnLoad(prefab); // We make sure that our prefab is not deleted in the first scene change
            
            // Now we determinate the type of part we are loading.
            // If it has CarProperties and Partinfo is a full part.
            // If it has PrefabGenerator ww know is a dummy with prefab gen.
            // If nothing applies is a dummy part.
            CarProperties prefabCarProp = prefab.GetComponent<CarProperties>();
            Partinfo prefabPartInfo = prefab.GetComponent<Partinfo>();

            if(prefabCarProp && prefabPartInfo)
            {
                // Automatically add some components and also assign the correct layer.
                // Pickup and DISABLER for the part - Required so they work properly!
                // Also add CarProperties to all nuts of the part, unexpected behaviour can happen if the component is missing.
                prefab.layer = LayerMask.NameToLayer("Ignore Raycast");

                Pickup prefabPickup = prefab.AddComponent<Pickup>();
                prefabPickup.canHold = true;
                prefabPickup.tempParent = GameObject.Find("hand");
                prefabPickup.SphereCOl = GameObject.Find("SphereCollider");

                prefab.AddComponent<DISABLER>();

                prefabCarProp.PREFAB = prefab; // Saving will not work without this due to a condition located in Saver.Save()
                Functions.BoltingSetup(prefab);

                // Now we create the part and add it to the list.
                Part part = new Part(prefab, prefabCarProp, prefabPartInfo, prefab.GetComponent<Renderer>(), this);
                PartManager.modLoadedParts.Add(part);
                loadedParts.Add(part);
                
                part.PartType = PartTypes.FULL_PART;
                
                Saver.modParts.Add(part.CarProps.PrefabName, prefab);

                Debug.Log($"[ModUtils/SPL]: Succesfully loaded part (full part) {prefabName} from {thisMod.Name}");
                return part; // We provide the Part instance so the developer can setup the transparents
            }
            
            Part p = new Part(prefab, null, null, null, this);
            PrefabGenerator prefabGen = prefab.GetComponent<PrefabGenerator>();
            if (prefabGen)
            {
                p.Name = prefabGen.PrefabName;
                Saver.modParts.Add(p.Name, prefab);

                PartManager.prefabGenParts.Add(p);
                loadedParts.Add(p);

                p.PartType = PartTypes.DUMMY_PREFABGEN;
                Debug.Log($"[ModUtils/SPL]: Succesfully loaded part (dummy part with Prefab generator) {prefabName} from {thisMod.Name}");
            }
            else
            {
                p.Name = prefabName;
                Saver.modParts.Add(prefabName, prefab);
                
                PartManager.dummyParts.Add(p);
                loadedParts.Add(p);

                p.PartType = PartTypes.DUMMY;
                Debug.Log($"[ModUtils/SPL]: Succesfully loaded part (dummy part) {prefabName} from {thisMod.Name}");
            }

            return p;
        }
    }
}
