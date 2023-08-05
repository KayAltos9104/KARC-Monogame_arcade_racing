namespace WitchEngine.UI;
public interface IKeyboardCursor
{
    List<IComponent> InteractiveElements { get; }
    int CursorPos { get; set; }
    void MoveCursor (DiscreteDirection cursorDir)
    {
        switch (cursorDir)
        {
            case DiscreteDirection.Up:
                {
                    CursorPos++;
                    if (CursorPos > InteractiveElements.Count - 1)
                        CursorPos = 0;
                    break;
                }
            case DiscreteDirection.Down:
                {
                    CursorPos--;
                    if (CursorPos < 0)
                        CursorPos = InteractiveElements.Count - 1;
                    break;
                }
            default:
                {
                    throw new ArgumentException("Wrong cursor direction");                    
                }
        }
    }
}
