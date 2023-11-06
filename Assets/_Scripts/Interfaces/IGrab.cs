public interface IGrab
{
    bool IsGrabbing { set; }
    PlayerController PlayerController { set; }
    void Grab();
    void Discard();
}
