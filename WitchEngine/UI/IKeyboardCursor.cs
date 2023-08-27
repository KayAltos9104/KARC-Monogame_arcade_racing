namespace WitchEngine.UI;
public interface IKeyboardCursor
{
    List<IComponent> InterfaceElements { get; }
    int CursorPos { get; set; }
    void MoveCursor (DiscreteDirection cursorDir)
    {
        switch (cursorDir)
        {
            case DiscreteDirection.Up:
                {
                    do
                    {
                        CursorPos--;                        
                    } while (GetCurrentElement().IsInteractive == false);
                    
                    if (CursorPos < 0)
                        CursorPos = InterfaceElements.Count - 1;

                    UpdateActivationOnElement();
                    break;
                    
                }
            case DiscreteDirection.Down:
                {
                    do
                    {
                        CursorPos++;
                    } while (GetCurrentElement().IsInteractive == false);
                    
                    if (CursorPos > InterfaceElements.Count - 1)
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
        return InterfaceElements[CursorPos];
    }
    void UpdateActivationOnElement()
    {
        InterfaceElements.ForEach(element => element.IsChosen = false);
        int firstActive = InterfaceElements.FindIndex(e => e.IsInteractive == true);
        CursorPos = firstActive;
        if (firstActive != -1) 
            InterfaceElements[CursorPos].IsChosen = true;
    }
}
