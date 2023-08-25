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
                    CursorPos--;
                    if (CursorPos < 0)
                        CursorPos = InteractiveElements.Count - 1;
                    UpdateActivationOnElement();
                    break;
                    
                }
            case DiscreteDirection.Down:
                {
                    CursorPos++;
                    if (CursorPos > InteractiveElements.Count - 1)
                        CursorPos = 0;
                    UpdateActivationOnElement();
                    break;
                }
            default:
                {
                    throw new ArgumentException("Wrong cursor direction");                    
                }
        }
    }
    IComponent GetCurrentElement()
    {
        return InteractiveElements[CursorPos];
    }
    void UpdateActivationOnElement()
    {
        InteractiveElements.ForEach(element => element.IsChosen = false);
        InteractiveElements[CursorPos].IsChosen = true;
    }
}
