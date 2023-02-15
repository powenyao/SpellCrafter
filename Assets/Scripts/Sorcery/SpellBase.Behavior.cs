using UnityEngine;

//public partial class SpellBase : XrosGrabInteractableSubscriber_Base, IDamageDealer
public partial class SpellBase : MonoBehaviour, IDamageDealer
{
    // params about BEHAVIOR
    //[SerializeField] protected Enum_SpellBehaviors behaviorType = Enum_SpellBehaviors.Path;
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float rotateSpeed_PartialTracking = 95f;
    [SerializeField] protected float rotateSpeedFactor_FullTracking;
    [SerializeField] protected float forceFactor_CurvePath;
    [SerializeField] protected float elevationAngle_CurvePath;
    [SerializeField] protected float tangentialSpeed_SpiralPath;
    [SerializeField] protected float rotateRadius_SpiralPath;
    [SerializeField] protected float frequency_SineWavePath;
    [SerializeField] protected float amplitude_SineWavePath;
    [SerializeField] protected float nonStraightTime_ManhattanPath;
    [SerializeField] protected float straightTime_ManhattanPath;
    //[SerializeField] protected float rotateInit;
    //[SerializeField] protected float rotatePace;
    //[SerializeField] protected float enableFullHomingTime;
    //protected float enableFullHomingTimer;
    //[SerializeField] protected float disableFullHomingTime;
    //protected float disableFullHomingTimer;
    //[SerializeField] private float minimumRotateAngle;

    protected float rotateValue = 0f;

    protected float enableStableSpiralTime;
    protected float enableStableSpiralTimer;

    protected Vector3 initPoint;
    protected Vector3 initPointShifted;
    protected Vector3 initForward;

    protected float sineWaveTimer;

    protected bool isPhysical = false;

    protected int manhattanCounter;
    protected bool isPositiveManhattan = true;
    protected bool isForwardManhattan = true;
    protected float manhattanNonStraightTimer;
    protected float manhattanStraightTimer;

    [SerializeField]
    private Rigidbody _rigidbody;
    
    public virtual void SearchTarget()
    {

    }

    protected void ResetStatus()
    {
        //enableFullHomingTimer = 0f;
        _rigidbody.useGravity = true;
    }

    //public void ChangeBehavior(Enum_SpellBehaviors newBehavior)
    //{
    //    behaviorType = newBehavior;
    //}

    protected void TrackingTarget(Vector3 direction, GameObject target = null)
    {
        if (target == null)
        {
            StraightMove(direction);
            return;
        }

        _rigidbody.velocity = transform.forward * moveSpeed;

        switch (_composition.GetTracking())
        {
            case Enum_SpellComponents_Tracking.Partial:
            {
                //rotateValue = rotateSpeed * Time.deltaTime;

                var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

                rotateValue = rotateSpeed_PartialTracking * Time.deltaTime;

                _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotateValue));

                break;
            }
            case Enum_SpellComponents_Tracking.Full:
            {
                //rotateValue += rotatePace;
                //if (enableFullHomingTimer > enableFullHomingTime)
                //{
                //    rotateValue = 180f;

                //    var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                //    _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotateValue));
                //}
                //else
                //{
                //    StraightMove(transform.forward);
                //}

                var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

                rotateValue = rotateSpeedFactor_FullTracking * Quaternion.Angle(transform.rotation, targetRotation) * Time.deltaTime;

                _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotateValue));

                break;
            }
        }

        //enableFullHomingTimer += Time.deltaTime;
    }

    protected void StraightMove(Vector3 direction)
    {
        _rigidbody.velocity = direction * moveSpeed;
    }

    protected void SetRigidBodyForParabola()
    {
        _rigidbody.velocity = transform.forward * moveSpeed;
    }

    protected void SetCurvedMode()
    {
        switch (_composition.GetPath())
        {
            case Enum_SpellComponents_Path.Curved_Left:
            {
                _rigidbody.velocity = Quaternion.Euler(0f, -elevationAngle_CurvePath, 0f) * transform.forward * moveSpeed;

                break;
            }
            case Enum_SpellComponents_Path.Curved_Right:
            {
                _rigidbody.velocity = Quaternion.Euler(0f, elevationAngle_CurvePath, 0f) * transform.forward * moveSpeed;

                break;
            }
            case Enum_SpellComponents_Path.Curved_Up:
            {
                _rigidbody.velocity = Quaternion.Euler(-elevationAngle_CurvePath, 0f, 0f) * transform.forward * moveSpeed;

                break;
            }
        }

        _rigidbody.useGravity = false;
    }

    protected void SetSpiralMode()
    {
        //The time for rotating half circle
        enableStableSpiralTime = Mathf.PI * (rotateRadius_SpiralPath / 2) / tangentialSpeed_SpiralPath;
        enableStableSpiralTimer = 0f;

        initForward = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z);
        initPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //The point that is half radius away above the starting point
        //This is used to locate the center for the first half circle from the launcher to the stable rotate orbit
        GameObject offset = new GameObject();
        offset.transform.SetParent(transform);
        offset.transform.localPosition = Vector3.zero;
        offset.transform.localPosition += new Vector3(0f, rotateRadius_SpiralPath / 2f, 0f);
        initPointShifted = new Vector3(offset.transform.position.x, offset.transform.position.y, offset.transform.position.z);
        Destroy(offset);

        _rigidbody.useGravity = false;

        //GameObject launcher = GameObject.Find("PF_Launcher PlayerVariant");
        //transform.SetParent(launcher.transform);
        //transform.localPosition = Vector3.zero;
        //transform.localPosition += new Vector3(0f, 0f, 5f);
        //initPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //transform.localPosition += new Vector3(0f, rotateRadius_SpiralPath, 0f);
        //transform.SetParent(null);
        _rigidbody.velocity = -transform.right * tangentialSpeed_SpiralPath + transform.forward * moveSpeed;

        switch (_composition.GetPath())
        {
            case Enum_SpellComponents_Path.Spiral:
            {
                _rigidbody.velocity = -transform.right * tangentialSpeed_SpiralPath + transform.forward * moveSpeed;

                break;
            }
            case Enum_SpellComponents_Path.Spiral_TestLocal:
            {
                GameObject launcher = GameObject.Find("PF_Launcher PlayerVariant");
                transform.SetParent(launcher.transform);
                transform.localPosition = Vector3.zero;
                transform.localPosition += new Vector3(0f, 0f, 5f);
                //initPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                transform.localPosition += new Vector3(0f, rotateRadius_SpiralPath, 0f);
                transform.SetParent(null);
                _rigidbody.velocity = -transform.right * tangentialSpeed_SpiralPath;

                break;
            }
        }

        //Vector3 force = Quaternion.Euler(0f, 0f, -90f) * (-transform.right) *
        //    _rigidbody.mass * tangentialSpeed_SpiralPath * tangentialSpeed_SpiralPath / (rotateRadius_SpiralPath / 2f);
        //_rigidbody.AddForce(force);
        //Dev.Log("Initial Force = " + force);

        //Vector3 force = -transform.up * //Quaternion.AngleAxis(-90f, initForward) * tangentialVelocity.normalized *
        //        _rigidbody.mass * tangentialSpeed_SpiralPath * tangentialSpeed_SpiralPath / rotateRadius_SpiralPath;
        //_rigidbody.AddForce(force);

        //float angle = 360f * Time.fixedDeltaTime / (2 * Mathf.PI * rotateRadius_SpiralPath / tangentialSpeed_SpiralPath);
        //transform.Rotate(0f, 0f, -angle);
    }

    protected void SetSineWaveMode()
    {
        switch (_composition.GetPath())
        {
            case Enum_SpellComponents_Path.SineWave_Horizontal:
            {
                _rigidbody.velocity = transform.forward * moveSpeed - transform.right * 
                    amplitude_SineWavePath / (2 * Mathf.PI * frequency_SineWavePath * _rigidbody.mass);

                break;
            }
            case Enum_SpellComponents_Path.SineWave_Vertical:
            {
                _rigidbody.velocity = transform.forward * moveSpeed + transform.up *
                    amplitude_SineWavePath / (2 * Mathf.PI * frequency_SineWavePath * _rigidbody.mass);

                break;
            }
        }

        _rigidbody.useGravity = false;

        sineWaveTimer = 0f;
    }

    protected void SetManhattanMode()
    {
        _rigidbody.velocity = transform.forward * moveSpeed;

        _rigidbody.useGravity = false;

        manhattanNonStraightTimer = 0f;
        manhattanStraightTimer = 0f;
        manhattanCounter = 1;
    }

    protected void CurvedMove()
    {
        switch (_composition.GetPath())
        {
            case Enum_SpellComponents_Path.Curved_Left:
            {
                _rigidbody.AddForce(transform.right * forceFactor_CurvePath);

                break;
            }
            case Enum_SpellComponents_Path.Curved_Right:
            {
                _rigidbody.AddForce(-transform.right * forceFactor_CurvePath);

                break;
            }
            case Enum_SpellComponents_Path.Curved_Up:
            {
                _rigidbody.AddForce(-transform.up * forceFactor_CurvePath);

                break;
            }
        }
    }

    private bool isFirstFrame = true;
    //private Vector3 prev;

    protected void SpiralMove()
    {
        //Vector3 tangentialVelocity = _rigidbody.velocity;// - initForward * moveSpeed;

        switch (_composition.GetPath())
        {
            case Enum_SpellComponents_Path.Spiral:
            {
                if (enableStableSpiralTimer < enableStableSpiralTime)
                {
                    //First rotate a half circle with half radius, in order to arrive at the stable spiral orbit
                    _rigidbody.AddForce(GetSpiralForceDirection(initPointShifted) * //Quaternion.AngleAxis(-90f, initForward) * tangentialVelocity.normalized *
                        _rigidbody.mass * tangentialSpeed_SpiralPath * tangentialSpeed_SpiralPath / (rotateRadius_SpiralPath / 2f));
                }
                else
                {
                    _rigidbody.AddForce(GetSpiralForceDirection(initPoint) * //Quaternion.AngleAxis(-90f, initForward) * tangentialVelocity.normalized *
                        _rigidbody.mass * tangentialSpeed_SpiralPath * tangentialSpeed_SpiralPath / rotateRadius_SpiralPath);
                }

                break;
            }
            case Enum_SpellComponents_Path.Spiral_TestLocal:
            {
                if (isFirstFrame)
                {
                    _rigidbody.velocity = transform.right * tangentialSpeed_SpiralPath;
                    isFirstFrame = false;
                }
                else
                {
                    float angle = 360f * Time.fixedDeltaTime / (2 * Mathf.PI * rotateRadius_SpiralPath / tangentialSpeed_SpiralPath);
                    transform.Rotate(0f, 0f, -angle);
                }

                Vector3 force = -transform.up * //Quaternion.AngleAxis(-90f, initForward) * tangentialVelocity.normalized *
                        _rigidbody.mass * tangentialSpeed_SpiralPath * tangentialSpeed_SpiralPath / rotateRadius_SpiralPath;
                _rigidbody.AddForce(force);

                break;
            }
        }

        enableStableSpiralTimer += Time.fixedDeltaTime;
    }

    private Vector3 GetSpiralForceDirection(Vector3 init)
    {
        Vector3 offset = transform.position - init;
        Vector3 direction = offset - Vector3.Project(offset, initForward);

        return -direction.normalized;
    }

    protected void SineWaveMove()
    {
        switch (_composition.GetPath())
        {
            case Enum_SpellComponents_Path.SineWave_Horizontal:
            {
                _rigidbody.AddForce(transform.right *
                    amplitude_SineWavePath * Mathf.Sin(2 * Mathf.PI * frequency_SineWavePath * sineWaveTimer));

                break;
            }
            case Enum_SpellComponents_Path.SineWave_Vertical:
            {
                _rigidbody.AddForce(-transform.up *
                    amplitude_SineWavePath * Mathf.Sin(2 * Mathf.PI * frequency_SineWavePath * sineWaveTimer));

                break;
            }
        }

        sineWaveTimer += Time.fixedDeltaTime;
    }

    protected void ManhattanMove()
    {
        if (isForwardManhattan)
        {
            if (manhattanStraightTimer < straightTime_ManhattanPath)
            {
                manhattanStraightTimer += Time.fixedDeltaTime;
            }
            else
            {
                switch (_composition.GetPath())
                {
                    case Enum_SpellComponents_Path.Manhattan_Horizontal:
                    {
                        _rigidbody.velocity = (isPositiveManhattan ? transform.right : -transform.right) * moveSpeed;

                        break;
                    }
                    case Enum_SpellComponents_Path.Manhattan_Vertical:
                    {
                        _rigidbody.velocity = (isPositiveManhattan ? transform.up : -transform.up) * moveSpeed;

                        break;
                    }
                }

                manhattanCounter++;
                if (manhattanCounter == 2)
                {
                    manhattanCounter = 0;
                    isPositiveManhattan = !isPositiveManhattan;
                }

                isForwardManhattan = false;
                manhattanStraightTimer = 0f;
            }
        }
        else
        {
            if (manhattanNonStraightTimer < nonStraightTime_ManhattanPath)
            {
                manhattanNonStraightTimer += Time.fixedDeltaTime;
            }
            else
            {
                _rigidbody.velocity = transform.forward * moveSpeed;
                isForwardManhattan = true;
                manhattanNonStraightTimer = 0f;
            }
        }
    }
}