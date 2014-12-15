using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace Captain_Of_Industry
{
    // Stores each tile type and texture
    public enum TILE_TYPE
    {
        ROAD, 
        WALL, 
        STORE, 
        DUMPSTER,
        RESIDENCE, 
        TYPE_COUNT
    };
    class MapInfo
    {
        // Stores the type of geography on this tile
        public TILE_TYPE[,] tileType;
        // Stores the frame number to be used on this tile
        public int[,] tile;
        // Stores width and height of entire map
        private int width;
        private int height;
        // Stores size of a single grid position on the map
        private int tileSize;
        public int GetWidth() { return width; }
        public int GetHeight() { return height; }

        // Create a map of provided height and width
        public MapInfo(int _width, int _height, int _tileSize)
        {
            width = _width;
            height = _height;
            tileSize = _tileSize;
            tileType = new TILE_TYPE[width,height];
            tile = new int[width, height];
        }

        // Sets up tiles for map - takes in width and height of map spritesheet ( in frames )
        public void Initialize(ObjectFactory _parentFactory)
        {
            StreamReader sr = new StreamReader("..\\..\\..\\MapInfo.txt");
            int line = 0;
            int i = 0;
            int j = 0;
            bool terminate = false;
            int setFrame;
            // If we have not read end of file
            while (terminate == false)
            {
                setFrame = 0;
                // Loop boolean should be flagged when a carriage return
                // or - is found
                bool isLooping = true;
                while (isLooping)
                {
                    line = sr.Read();
                    switch (line)
                    {
                        // Break loop on return or -
                        case 13:
                        case 45:
                            isLooping = false;
                            break;
                        // Will stop reading file and end loop after finding a :
                        case 58:
                            terminate = true;
                            isLooping = false;
                            break;
                        default:
                            // If set frame is not 0, then we found a number on previous loop
                            // and are about to increment it again - meaning the previous number was
                            // a power of 10
                            if (setFrame != 0) { setFrame *= 10; }
                            setFrame += (line - 48);
                            break;
                    }
                }
                // If line did not read a : (terminating character) set the frame, incremement width
                if (line != 58) 
                {
                    // Setup the visual information for this tile
                    tile[i, j] = setFrame;
                    // Set frame tile type information
                    if (setFrame < 10) 
                    {
                        tileType[i,j] = TILE_TYPE.ROAD;
                    }
                    else if (setFrame == 34 || setFrame == 35 || setFrame == 36)
                    {
                        tileType[i,j] = TILE_TYPE.RESIDENCE;
                    }
                    else if (setFrame == 58 || setFrame == 59 || setFrame == 60)
                    {
                        tileType[i, j] = TILE_TYPE.RESIDENCE;
                    }
                    else if (setFrame == 62)
                    {
                        tileType[i, j] = TILE_TYPE.STORE;
                        // Engine to create a store at this position
                        Vector2 tempPosition = new Vector2(i * tileSize, (j + 1) * tileSize);
                        _parentFactory.AddObjectOfTypeAtPosition(OBJECT_TYPE.STORE, tempPosition);
                    }
                    else
                    {
                        tileType[i, j] = TILE_TYPE.WALL;
                    }
                    
                    // Set colliders on any blockable areas
                    if (setFrame > 10 && setFrame != 35 && setFrame != 59 && setFrame != 62)
                    {
                        // Get engine to create a static object at this position
                        Vector2 tempPosition = new Vector2(i * tileSize, (j+1) * tileSize);
                        _parentFactory.AddObjectOfTypeAtPosition(OBJECT_TYPE.STATIC, tempPosition);
                    }
                    i++;
                }
                // If the last line was a carriage return, increment j, reset i
                if (line == 13)
                {
                    // Read an extra line after a carriage return, otherwise we get garbage for next value
                    line = sr.Read();
                    j++;
                    i = 0;
                }
                
                line = 0;
            }
            sr.Close();  
        }
        // Return the size of squares on the map
        public int GetTileSize() { return tileSize; }
    }
}
