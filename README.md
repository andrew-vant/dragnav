# Draggable Navball

Draggable Navball is a small mod for Kerbal Space Program. It does exactly
what it says on the tin: Allows you to drag the navball somewhere more
convenient than its default position directly under your rocket.

The ball's position will be remembered across scenes and game restarts. Note
that the UI\_POS\_NAVBALL option in settings.cfg is ignored while using
Draggable Navball.

## Building

To build the source, you need:

* make
* mcs (the mono compiler)
* KSP/Unity's .dll files

Assuming you have that, `make` should build it. `make dist` should produce a
zip.

By default the makefile looks in a subdirectory called `libs` for the
dlls; you can change that at the top if you need to. You can find them in
KSP's game directory under `/KSP_x64_Data/Managed`.

If you're on Windows and don't have a unixish shell available, you're on
your own. Looking through the makefile may help you set up your own build.

## Installing

Like any other KSP mod, you can copy or unzip the DraggableNavball directory
into the `GameData` KSP folder. Or, preferably, install it from CKAN.
