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

    public InterfaceManager()
    { 
        InterfaceElements = new List<IComponent>();
        CursorPos = 0;
    }

    public void AddElement(IComponent component)
    {
        InterfaceElements.Add(component);
    }

}
