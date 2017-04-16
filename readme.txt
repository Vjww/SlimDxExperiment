16/Apr/2017
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

Future development:
- A button will need to track and illustrate a number of states
--- whether the button is focused or not (tabbing/keyboard)
--- whether the button is selected or not (toggle button)
--- whether the button is hovered by the mouse cursor or not
--- whether the button is pressed down by the mouse cursor/keyboard or not
- As each state is independant, separate textures or overlays/effects will be required
  rather than the use of a texture for each different state
