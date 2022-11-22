using Microsoft.Xna.Framework;
namespace KARC.WitchEngine.Animations;
public class Animator
{
    private AnimationAtlas _animationAtlas;
    private int _frameTime;
    private int _deltaTime;
    private int _currentFrameIndex;
    private bool _isCycled;    
    public bool IsActive { get; private set; }
    public bool IsCentered { get; private set; }
    public AnimationFrame CurrentFrame { get; private set; }
    public Animator (AnimationAtlas animationAtlas, int frameTime, bool isCycled, bool isCentered)
    {
        _animationAtlas = animationAtlas;
        _frameTime = frameTime;
        _isCycled = isCycled;
        _deltaTime = 0;
        _currentFrameIndex = 0;
        IsActive = false;
        IsCentered = isCentered;
    }
    public int GetPictureId()
    {
        return _animationAtlas.ImageId;
    }
    public void Activate()
    {
        IsActive = true;
        _currentFrameIndex = 0;
        CurrentFrame = _animationAtlas.AnimationFrames[_currentFrameIndex];
    }
    public void Deactivate()
    {
        IsActive = false;
    }
    public void Update(GameTime gameTime)
    {
        if (IsActive == false)
            return;

        _deltaTime += gameTime.ElapsedGameTime.Milliseconds;
        if (_deltaTime > _frameTime)
        {
            _deltaTime = 0;
            _currentFrameIndex++;
            if (_currentFrameIndex >= _animationAtlas.AnimationFrames.Length)
            {
                if (_isCycled == true)
                    _currentFrameIndex = 0;
                else
                    IsActive = false;
            }
            if (IsActive == true)
                CurrentFrame = _animationAtlas.AnimationFrames[_currentFrameIndex];
        }
    }
}
