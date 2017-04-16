14/Apr/2017
-----------

Sample application that demonstrates the drawing of controls derived from
System.Windows.Forms.UserControl with C# and SlimDX. With controls deriving
from UserControl, this gives us access to the functionality behind a typical
control (e.g. events, bounds, heirachy etc.)

Issues to resolve:
- UIButton appears to ignore location values (e.g. 100, 100)
- MouseDown/Up/Hover appear to pick up alternate control bounds (bottom right off centre)
- Flickering button when window is dragged over the top of button
- General sluggish performance