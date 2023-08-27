using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitchEngine.UI;

public class InterfaceManager: IKeyboardCursor
{
    public List<IComponent> InterfaceElements { get; private set; }
    public int CursorPos { get; set; }
    public int firstActiveElementIndex { get; set; }
    public int lastActiveElementIndex { get; set; }

    public InterfaceManager()
    { 
        InterfaceElements = new List<IComponent>();
        firstActiveElementIndex = InterfaceElements.FindIndex(e => e.IsInteractive == true);
        lastActiveElementIndex = InterfaceElements.FindLastIndex(e => e.IsInteractive == true);
        CursorPos = firstActiveElementIndex;
        if (CursorPos != -1)
            InterfaceElements[CursorPos].IsChosen = true;
       
    }

    public void AddElement(IComponent component)
    {
        InterfaceElements.Add(component);
        firstActiveElementIndex = InterfaceElements.FindIndex(e => e.IsInteractive == true);
        lastActiveElementIndex = InterfaceElements.FindLastIndex(e => e.IsInteractive == true);
        if (CursorPos == -1)
        {
            CursorPos = firstActiveElementIndex;
            ((IKeyboardCursor)this).UpdateActivationOnElement();
        }


    }

}
