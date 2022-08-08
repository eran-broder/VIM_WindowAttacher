# VIM_WindowAttacher

This whole solution is a playground to test the core, which is the EXE named **WindowAttacher**.

The exe gets as its input (format specified by running the exe itself) from the commnand line, specifying:

1. the window to which another window will be attached to
2. the window that will be attached
3. the offset relative to the windows to which we are attaching to

we manipulate several windows flags in order to achive a Parent-Child relationship.

is is worth noting, that uppon closing the parent window there will **not** be a close message sent to the child.
the application layer needs to monitor this and give thought.

The solution contains several "dummy" apps to test the whole thing

**VimConnectMock** - an exe that spwans two windows, then calls the tool to attach them to each other

**EhrMock** - the parent window

**electron-quick-start** - a simple electron app that will be used as the child window

