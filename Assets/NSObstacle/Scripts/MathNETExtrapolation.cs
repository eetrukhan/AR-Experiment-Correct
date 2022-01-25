using System;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics;
using MathNet.Numerics.Interpolation;

public class MathNETExtrapolation : MonoBehaviour, IExtrapolation
{
    public enum InterpolationMethod
    {
        Linear,
        LogLinear,
        CubicAkimaSpline,
        NevillePolynomial,
        FloaterHormannRational,
        BulirschStoerRational
    }

    [SerializeField, Tooltip("Type of interpolation")]
    protected InterpolationMethod _interpolationMethod = InterpolationMethod.Linear;
    [SerializeField]
    private bool altRotationExtrapolation = false;

    [Header("Data Management")]
    [SerializeField, Tooltip("Buffer length")]
    protected uint _dataTicksToRemember = 3;
    [SerializeField, Tooltip("How sparse the data is")]
    protected uint _rememberEachNthTick = 3;

    private Queue<double> _times,
        _posXs,
        _posYs,
        _posZs,
        _rotXs,
        _rotYs,
        _rotZs,
        _rotWs;

    protected virtual void Start()
    {
        // Sanity check
        if (_dataTicksToRemember < 2)
        {
            Debug.LogError("MathNETExtrapolation: The value of the '_dataTicksToRemember' field cannot be less than 2. Disabling the script.");
            enabled = false;
            return;
        }

        if (_rememberEachNthTick == 0)
        {
            Debug.LogError("MathNETExtrapolation: The value of the '_rememberEachNthTick' has to be greater than 0. Disabling the script.");
            enabled = false;
            return;
        }

        InitBuffers();
    }

    public virtual RsPoseStreamTransformer.RsPose Extrapolate(RsPoseStreamTransformer.RsPose pose, float currentTime, float deltaTime)
    {
        if (!enabled)
            throw new UnityException("MathNETExtrapolation: Can't call the 'Extrapolate' method on a disabled component");

        RsPoseStreamTransformer.RsPose result = new RsPoseStreamTransformer.RsPose();
        if (!IsEnoughData())
        {
            result.translation = pose.translation;
            result.rotation = pose.rotation;

            StoreValuesInBuffers(pose, currentTime);

            return result;
        }


        float predictionTime = currentTime + deltaTime;
        double[] times = QueueToArray(_times, currentTime);

        result.translation.Set(
            (float)CreateInterpolation(_interpolationMethod, times, QueueToArray(_posXs, pose.translation.x)).Interpolate(predictionTime),
            (float)CreateInterpolation(_interpolationMethod, times, QueueToArray(_posYs, pose.translation.y)).Interpolate(predictionTime),
            (float)CreateInterpolation(_interpolationMethod, times, QueueToArray(_posZs, pose.translation.z)).Interpolate(predictionTime));

        if (altRotationExtrapolation)
        {
            // Выяснил, что дерганье вызвано не алгоритмом, а частотой передачи сигнала с мобилки (она недостаточна)
            Quaternion prevRot = new Quaternion(
                (float)QueueToArray(_rotXs, pose.rotation.x)[_dataTicksToRemember - 2],
                (float)QueueToArray(_rotYs, pose.rotation.y)[_dataTicksToRemember - 2],
                (float)QueueToArray(_rotZs, pose.rotation.z)[_dataTicksToRemember - 2],
                (float)QueueToArray(_rotWs, pose.rotation.w)[_dataTicksToRemember - 2]);
            float prevTime = (float)QueueToArray(_times, currentTime)[_dataTicksToRemember - 2];

            result.rotation = Extrapolate(prevRot, prevTime, pose.rotation, currentTime, predictionTime);
        }
        else
            /** FIX IT Extrapolating the rotation by this way causes a jerky behaviour around 0 degrees on all axes. It's subtle but it is here.
             *  I believe that causes of the behaviour are similar to the approach with Euler angles. It has something to do with how
             *  quaternion deals with values of its y, z, and w coefficients.
             */
            result.rotation.Set(
                (float)CreateInterpolation(_interpolationMethod, times, QueueToArray(_rotXs, pose.rotation.x)).Interpolate(predictionTime),
                (float)CreateInterpolation(_interpolationMethod, times, QueueToArray(_rotYs, pose.rotation.y)).Interpolate(predictionTime),
                (float)CreateInterpolation(_interpolationMethod, times, QueueToArray(_rotZs, pose.rotation.z)).Interpolate(predictionTime),
                (float)CreateInterpolation(_interpolationMethod, times, QueueToArray(_rotWs, pose.rotation.w)).Interpolate(predictionTime));

        StoreValuesInBuffers(pose, currentTime);

        return result;
    }

    public virtual RsPoseStreamTransformer.RsPose Extrapolate(float currentTime, float deltaTime) // for testing purposes
    {
        if (!enabled)
            throw new UnityException("MathNETExtrapolation: Can't call the 'Extrapolate' method on a disabled component");

        RsPoseStreamTransformer.RsPose result = new RsPoseStreamTransformer.RsPose();
        if (!IsEnoughData())
        {
            result.translation = Vector3.zero;
            result.rotation = Quaternion.identity;

            return result;
        }

        float predictionTime = currentTime + deltaTime;
        double[] times = QueueToArray(_times);

        result.translation.Set(
            (float)CreateInterpolation(_interpolationMethod, times, QueueToArray(_posXs)).Interpolate(predictionTime),
            (float)CreateInterpolation(_interpolationMethod, times, QueueToArray(_posYs)).Interpolate(predictionTime),
            (float)CreateInterpolation(_interpolationMethod, times, QueueToArray(_posZs)).Interpolate(predictionTime));

        result.rotation.Set(
                (float)CreateInterpolation(_interpolationMethod, times, QueueToArray(_rotXs)).Interpolate(predictionTime),
                (float)CreateInterpolation(_interpolationMethod, times, QueueToArray(_rotYs)).Interpolate(predictionTime),
                (float)CreateInterpolation(_interpolationMethod, times, QueueToArray(_rotZs)).Interpolate(predictionTime),
                (float)CreateInterpolation(_interpolationMethod, times, QueueToArray(_rotWs)).Interpolate(predictionTime));

        return result;
    }

    protected static IInterpolation CreateInterpolation(InterpolationMethod interpolationMethod, double[] points, double[] values)
    {
        IInterpolation interpolation = null;
        switch (interpolationMethod)
        {
            case InterpolationMethod.Linear:
                interpolation = LinearSpline.InterpolateSorted(points, values);
                break;
            case InterpolationMethod.LogLinear:
                interpolation = LogLinear.InterpolateSorted(points, values);
                break;
            case InterpolationMethod.CubicAkimaSpline:
                interpolation = CubicSpline.InterpolateAkimaSorted(points, values);
                break;
            case InterpolationMethod.NevillePolynomial:
                interpolation = NevillePolynomialInterpolation.InterpolateSorted(points, values);
                break;
            case InterpolationMethod.FloaterHormannRational:
                interpolation = Barycentric.InterpolateRationalFloaterHormannSorted(points, values);
                break;
            case InterpolationMethod.BulirschStoerRational:
                interpolation = BulirschStoerRationalInterpolation.InterpolateSorted(points, values);
                break;
        }
        return interpolation;
    }

    private bool IsEnoughData()
    {
        return (_times.Count >= getBuffersMaxCapacity());
    }

    private T[] QueueToArray<T>(Queue<T> queue, T additionalValue)
    {
        T[] result = new T[_dataTicksToRemember];
        int i = 0, counter = 0;
        foreach(T value in queue)
        {
            if (counter == 0)
                result[i++] = value;

            if (++counter == _rememberEachNthTick)
                counter = 0;
        }
        result[_dataTicksToRemember - 1] = additionalValue;
        return result;
    }

    private T[] QueueToArray<T>(Queue<T> queue) // for testing purposes
    {
        T[] qArray = queue.ToArray(), res = new T[2];
        res[1] = qArray[queue.Count - 1]; // adding the latest value
        res[0] = qArray[queue.Count - _rememberEachNthTick - 1]; // the value of the prev tick
        return res;
    }

    private Quaternion Extrapolate(Quaternion prevRot, float prevTime, Quaternion currentRot, float currentTime, float predictionTime)
    {
		// Source: https://answers.unity.com/questions/168779/extrapolating-quaternion-rotation.html
        Quaternion rotation = currentRot * Quaternion.Inverse(prevRot);
        float extrapolationFactor = (predictionTime - prevTime) / (currentTime - prevTime);

        rotation.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180f) angle -= 360f;
        angle = angle * extrapolationFactor % 360f;

        return Quaternion.AngleAxis(angle, axis) * prevRot;
    }

    private int getBuffersMaxCapacity()
    {
        return (int)(_dataTicksToRemember - 1) * (int)_rememberEachNthTick;
    }

    private void InitBuffers()
    {
        int capacity = getBuffersMaxCapacity();

        // Initialize a time biffer 
        _times = new Queue<double>(capacity);

        // Initialize position buffers 
        _posXs = new Queue<double>(capacity);
        _posYs = new Queue<double>(capacity);
        _posZs = new Queue<double>(capacity);

        // Initialize rotation buffers 
        _rotXs = new Queue<double>(capacity);
        _rotYs = new Queue<double>(capacity);
        _rotZs = new Queue<double>(capacity);
        _rotWs = new Queue<double>(capacity);
    }

    private void StoreValuesInBuffers(RsPoseStreamTransformer.RsPose pose, float time)
    {
        // If the buffers are already full
        if (_times.Count == getBuffersMaxCapacity())
        {
            _times.Dequeue();

            _posXs.Dequeue();
            _posYs.Dequeue();
            _posZs.Dequeue();

            _rotXs.Dequeue();
            _rotYs.Dequeue();
            _rotZs.Dequeue();
            _rotWs.Dequeue();
        }

        _times.Enqueue(time);

        _posXs.Enqueue(pose.translation.x);
        _posYs.Enqueue(pose.translation.y);
        _posZs.Enqueue(pose.translation.z);

        _rotXs.Enqueue(pose.rotation.x);
        _rotYs.Enqueue(pose.rotation.y);
        _rotZs.Enqueue(pose.rotation.z);
        _rotWs.Enqueue(pose.rotation.w);
    }
}
