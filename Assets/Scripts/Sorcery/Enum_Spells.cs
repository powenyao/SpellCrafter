//Reference to the various enum types
//https://docs.google.com/spreadsheets/d/1ecp5bPLOy_fXQO3aUuUc46Q6hXONMFKgxgODaQ4zC2k/edit#gid=0
public enum Enum_SpellShapes
{
    Sphere, //Sphere
    //Hemisphere,
    //Cube,
    //Cone,
    Cylinder,
    //Horizontal,
    //Cross,
    //Wall,
    //Spikes,
}

public enum Enum_SpellEffects
{
    Large,
    Power,
    Area,
    Propagate,
    Project,
    SelfGuide,
    Form,
    Persist,
    Bounce,
}

public enum Enum_SpellLauncherOrigin
{
    LeftHand,
    RightHand,
    Equipment,
    Eye,
    Mouth,
    GazePoint,
    /*
    AboveUser,
    BelowUser,
    BehindUser,
    AboveTarget,
    BelowTarget,
    Floor,
    SurroundEnemy
    */
}
public enum Enum_SpellBehaviors
{
    Tracking,
    Path,
}


public enum Enum_SpellLauncherComponents
{
    Origin,
    Method,
    FireMode
        
}
public enum Enum_SpellComponentCategories
{
    None,
    Element,
    Shape,
    Effects,
    Origin,
    Target
}

public enum Enum_SpellComponents_Effects
{
    None,
    Pull,
    Widen,
    Concentrate,
    SpeedUp,
    SpeedDown,
    AoE,
    PassThrough,
}

public enum Enum_SpellComponents_Tracking
{
    None,
    Partial,
    Full
}
public enum Enum_SpellComponents_Path
{
    None,
    //Spiral,
    Curved,
    Straight,
    //Manhattan,
    //SineWave,
    Parabola
}

public enum Enum_SpellComponents_Trigger
{
    None,
    OnContact,
    //OnTimeExpire,
    //OnUserChoice,
    //OnDistanceExpire,
    //OnManaExpire
}