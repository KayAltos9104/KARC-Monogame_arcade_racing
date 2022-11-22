using System;
namespace KARC.WitchEngine.Animations;
public class AnimationAtlas
{
    public int ImageId { get; }
    private int _counter;
    public AnimationFrame[] AnimationFrames { get; }

    public AnimationAtlas (int imageId, int frameCount)
    {
        ImageId = imageId;
        AnimationFrames = new AnimationFrame[frameCount];
        _counter = 0;
    }
    public void AddFrame (AnimationFrame animationFrame)
    {        
        if (_counter < AnimationFrames.Length)
        {
            AnimationFrames[_counter] = animationFrame;
            _counter++;
        }            
        else
        {
            throw new Exception("Too many frames");
        }            
    }
}
