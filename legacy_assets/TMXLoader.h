#ifndef TMXLOADER_H
#define TMXLOADER_H


#include <SDL.h>
#include <SDL_image.h>


#include <iostream>
#include <fstream>
#include <string>
#include <vector>
using namespace std;


#include "base64.h"
#include "tinyxml.h"

class TMXLoader : public TiXmlVisitor
{
    public:
        TMXLoader();
        virtual ~TMXLoader();


        void cleanup();


        virtual bool 	VisitEnter  (const TiXmlDocument  &);

        virtual bool 	VisitExit (const TiXmlDocument &);

        virtual bool 	VisitEnter (const TiXmlElement &, const TiXmlAttribute *);

        virtual bool 	VisitExit (const TiXmlElement &);

        virtual bool 	Visit (const TiXmlDeclaration &);

        virtual bool 	Visit (const TiXmlText &);

        virtual bool 	Visit (const TiXmlComment &);

        virtual bool 	Visit (const TiXmlUnknown &);


        bool loadDocument();

        SDL_Surface* getTilesetImage() { return img_tileset; }
        SDL_Surface* getMapImage();
		vector< vector< bool > > getMapCollisionData() { return m_MapCollisionData; }

        int getTileWidth() { return m_TileWidth; }
        int getTileHeight() { return m_TileWidth; }
        int getTileSpacing() { return m_TileSpacing; }
        int getTilesetMargin() { return m_TilesetMargin; }
        int getNumMapColumns() { return m_NumMapColumns; }
        int getNumMapRows() { return m_NumMapRows; }

    protected:

    private:
        int m_TileWidth;
        int m_TileHeight;
        int m_TileSpacing;
        int m_TilesetMargin;
		int m_TilesetRows;
		int m_TilesetColumns;
        int m_NumMapColumns;
        int m_NumMapRows;
		int m_ImageWidth;
		int m_ImageHeight;
		

		// used for reading in properties
		int currentTile;

        SDL_Surface * img_tileset,
                    * img_map;
        ofstream myfile;

		// Holds the gridID (the sprite location) for each map tile
        vector< vector< int > > m_LayerData;
		// Holds the collision data of each tile from the tilesheet
		vector< vector< bool > > m_TilesetCollisionData;
		// Holds the collision data of the constructed map
		vector< vector< bool > > m_MapCollisionData;


        void decode_and_store_map_data( string encoded_data );
        SDL_Surface* load_SDL_image( std::string filename );


        void buildMapImage();
        //TiXmlDocument *doc;

		void buildCollisionArray ();

};

#endif // TMXLOADER_H
