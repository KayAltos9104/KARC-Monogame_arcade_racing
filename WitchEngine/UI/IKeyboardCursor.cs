namespace WitchEngine.UI;
public interface IKeyboardCursor
{
    List<IComponent> InterfaceElements { get; }
    int CursorPos { get; set; }
    int firstActiveElementIndex { get; set; }
    int lastActiveElementIndex { get; set; }
    void MoveCursor (DiscreteDirection cursorDir)
    {
        switch (cursorDir)
        {
            case DiscreteDirection.Up:
                {
                    do
                    {
                        CursorPos--;
                        if (CursorPos < firstActiveElementIndex)
                        {
                            CursorPos = lastActiveElementIndex;
                        }
                    } while (GetCurrentElement().IsInteractive == false);
                    UpdateActivationOnElement();
                    break;
                    
                }
            case DiscreteDirection.Down:
                {
                    do
                    {
                        CursorPos++;
                        if (CursorPos > lastActiveElementIndex)
                            CursorPos = firstActiveElementIndex;
                    } while (GetCurrentElement().IsInteractive == false);
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
        return InterfaceElements[CursorPos];
    }
    void UpdateActivationOnElement()
    {
        InterfaceElements.ForEach(element => element.IsChosen = false);       
        if (CursorPos != -1) 
            InterfaceElements[CursorPos].IsChosen = true;
    }
}
