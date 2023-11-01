public interface IGrab
{
    bool IsGrabbing { set; }
    void Grab();
    void Discard();
}
