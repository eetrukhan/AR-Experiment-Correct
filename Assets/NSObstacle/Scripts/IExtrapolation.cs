public interface IExtrapolation
{
    RsPoseStreamTransformer.RsPose Extrapolate(RsPoseStreamTransformer.RsPose pose, float currentTime, float deltaTime);
    RsPoseStreamTransformer.RsPose Extrapolate(float currentTime, float deltaTime);
}
