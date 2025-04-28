
# 2D World Generator

This is a tool for developers who would like to save time or just do not want to do any manual level design. It uses a tilemap to procedurally generate a world through a few different stages which are all customizable by the end user.

This tool requires Unity version 2023.1.11f1 or newer.

#Set up steps

Navigate to the Assets window where you will find Import Package -> Custom Package...

Locate the file 'World Generation Package.unitypackage' and import it into your project.

Once imported, add a rectangular tilemap to your scene as usual and add the 'World Generation' and 'MyTilemap' script components.

Now navigate to the Window menu where you should see a 'World Generator' window. 
Open it and add the Tilemap object you just made, a width and height, as well as the desired generation stages you want.

The package includes some existing stages and tiles, but you can make and customize your own if you so wish. You can find existing stages in the 'Stages SO' folder and tiles are in 'Tilemap - Tiles'. To do so, you need to:

Right click in your content window, go to 'Create' -> 'Generation' for generation stages or 'Tile' for custom tiles. The stages and tiles can be customized inside the inspector.

There you have it. You now should be able to create 2D Worlds to your hearts content.

