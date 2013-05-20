# Font Awesome + Windows Forms

This little project came about from needing to add a bunch of icon buttons to a Windows Forms project. I originally created a custom image-button control which worked well enough, but the buttons couldn't scale in size, we didn't have a designer to draw decent images, and everything we tried just looked tacky.

I've been using Font Awesome on my web projects - it's great to have icons as vectors so they can be coloured and scaled on demand! I really wanted to do the same things with this Windows Forms project.

The FontAwesomeIcons project in the solution creates a DLL that you can use in any windows forms project. Add it to your toolbox, and you can drag the icon straight onto the form in the Designer View.

Font Awesome (see http://fortawesome.github.io/Font-Awesome/) is already embedded - there is no need to install the font on the client machine.

The IconType enum contains a few icon codes ready to go... not enough time to add them all. Maybe I'll add them one day. You can easily add new codes by adding to the enum from here: http://fortawesome.github.io/Font-Awesome/cheatsheet/

See the WinFormsTest project included for a sample/test project.

Transparency works over the container's background, but not over sibling controls (see the textbox in the WinFormsTest project).

The properties available are:

* IconType - the icon to use
* Width/Height - keep it square! eg 16x16 or 40x40... don't use 200x20.
* InActiveColor - the icon will be this color when not hovered over.
* ActiveColor - the icon will be this color when hovered over.
* ToolTipText - if set, a tooltip will be created for the icon with this text.

If you want to use it as a button, just bind an event handler to the click event.

**TODO:**

* Make buttons selectable by keyboard (tab-key)

