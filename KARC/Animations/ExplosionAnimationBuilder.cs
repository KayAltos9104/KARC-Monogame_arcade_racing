using KARC.Settings;
using KARC.WitchEngine;
using KARC.WitchEngine.Animations;

namespace KARC.Animations;

public static class ExplosionAnimationBuilder
{
    public static Animator CreateExplosionAnimation()
    {
        AnimationAtlas explosionAtlas = new AnimationAtlas((int)Sprite.explosion, 5);
        AnimationFrame frame1 = new AnimationFrame(20, 151, 70, 70);
        AnimationFrame frame2 = new AnimationFrame(138, 131, 112, 96);
        AnimationFrame frame3 = new AnimationFrame(265, 104, 160, 152);
        AnimationFrame frame4 = new AnimationFrame(448, 33, 251, 259);
        AnimationFrame frame5 = new AnimationFrame(733, 0, 368, 323);
        explosionAtlas.AddFrame(frame1);
        explosionAtlas.AddFrame(frame2);
        explosionAtlas.AddFrame(frame3);
        explosionAtlas.AddFrame(frame4);
        explosionAtlas.AddFrame(frame5);

        Animator explosionAnimation = new Animator(explosionAtlas, 100, true, true);

        return explosionAnimation;
    }
}
