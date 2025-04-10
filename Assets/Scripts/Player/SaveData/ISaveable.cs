
public interface ISaveable
{
    string SaveKey(); 
    object CaptureState();
    void RestoreState(object state);
}
