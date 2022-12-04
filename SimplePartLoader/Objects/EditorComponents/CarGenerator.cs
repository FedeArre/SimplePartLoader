﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CarGenerator : MonoBehaviour
{
    public string CarName = "";
    public int CarPrice = 0;
    public CarBase BaseCarToUse = CarBase.Chad;
    public List<string> TransparentsToDelete = new List<string>();

    [Header("Custom meshes - Fuel line")]
    public Mesh Inline4FuelLine;
    public Mesh V8EngineFuelLine;
    public Mesh Inline6FuelLine;
    
    [Header("Custom meshes - Radiator hoses")]
    public Mesh Inline4RadiatorHoseUpper;
    public Mesh Inline4RadiatorHoseLower;
    public Mesh V8RadiatorHoseUpper;
    public Mesh V8RadiatorHoseLower;
    public Mesh I6RadiatorHoseUpper;
    public Mesh I6RadiatorHoseLower;
    
    [Header("Custom meshes - Brake line")]
    public bool EnableCustomBrakeLine;
    public Mesh BrakeLineMesh;
    public Vector3 FrontLeftPivot;
    public Vector3 FrontRightPivot;
    public Vector3 RearLeftPivot;
    public Vector3 RearRightPivot;

    [Header("Custom meshes - Battery wires")]
    public Mesh Inline4BatteryWires;
    public Mesh V8EngineBatteryWires;
    public Mesh Inline6BatteryWires;

    [Header("Fixes & settings (Don't touch if you don't know what are you doing!)")]
    public bool EnableAttachFix = true;
    public bool EnableAutomaticPartCount = true;
    public bool AutoMainCarPropertiesSetup = true;
    public bool TransparentReferenceUpdate = true;
    public bool BoneTargetTransformFix = true;
    public bool HandbrakeCableBoneFix = true;
    public bool FixLights = true;
    public bool DisableModUtilsTemplateSetup = false;
}

public enum CarBase
{
    Chad
}