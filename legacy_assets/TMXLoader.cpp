
// The visitor pattern visits each node of the XML document individually. This means any information in parent nodes seems to be inaccesible. 
// Since we need the tile ID for checking tile collision, this system is not ideal. Current work around is to store the tile ID globally when we reach the tile node, and reference
// that ID when we traverse further and reach the property node. This is an incredibly poor solution, but to fix it will likely need to scrap the visitor pattern.

// There is also a number of discrepancies between column and row between the different vectors used. I plan on standardizing them when I have time


#include "TMXLoader.h"

TMXLoader::TMXLoader()
{
    //mapWidth = mapHeight = 0;
    img_tileset = NULL;


    // Debug crap
    //ofstream myfile;
    myfile.open ("debug.txt");


}

TMXLoader::~TMXLoader()
{
    //dtor
}

void TMXLoader::cleanup()
{
    //Free the surface
    SDL_FreeSurface( img_tileset );
    SDL_FreeSurface( img_map );


    myfile.close();
}

bool 	TMXLoader::VisitEnter  (const TiXmlDocument  &doc)
{
    return true; //TODO: for performance, we may not want to return true for each of these callbacks for the visitor pattern.
}

bool 	TMXLoader::VisitExit (const TiXmlDocument &doc)
{
    return true;
}

bool 	TMXLoader::VisitEnter (const TiXmlElement &elem, const TiXmlAttribute *attrib)
{
    if (string("map") == elem.Value()) {
        elem.Attribute("width", &m_NumMapColumns);
        elem.Attribute("height", &m_NumMapRows);

        //elem.Attribute("tileheight");
        //elem.Attribute("tilewidth");

        //TODO: get width and height, and tilewidth and tileheight
        //m_layer_width =

    }
    else if (string("tileset") == elem.Value()) {

        // Need a new class called Tileset

        elem.Attribute("tilewidth", & m_TileWidth);
        elem.Attribute("tileheight", &m_TileHeight);
        elem.Attribute("spacing", &m_TileSpacing);
        elem.Attribute("margin", &m_TilesetMargin);

        //TODO: get spacing and margin
    }
    else if (string("image") == elem.Value()) {
        //string attrib = attrib.ValueStr();
        const char* attrib = elem.Attribute("source");

        img_tileset = load_SDL_image(attrib);

		elem.Attribute("width", &m_ImageWidth);
		elem.Attribute("height", &m_ImageHeight);

		// At this point we need everything to build the collision array. Not an intuitive location to make the call, but it works
		buildCollisionArray();
    }
    else if (string("layer") == elem.Value()) {
        // We don't neet to use layer width and height yet.
        //elem.Attribute("name");
        //elem.Attribute("width");
        //elem.Attribute("height");
    }
    else if (string("data") == elem.Value()) {
        const char* text = elem.GetText();
        decode_and_store_map_data( text );

        buildMapImage();
    }
	// Current bandaid solution for reading collision properties. Read comment at top of file
	else if (string("tile") == elem.Value()) {
		const char* text = elem.Attribute("id");
		currentTile = atoi(text);
	}
	else if (string("property") == elem.Value()) {
		if (elem.Attribute("name") == string("Collidable"))
		{
			std::cout << "Tile " << currentTile << ": " << elem.Attribute("value") << std::endl;
			if (elem.Attribute("value") == string("True") )
				m_TilesetCollisionData[currentTile%m_TilesetColumns][currentTile/m_TilesetColumns] = true;
			// technically not necessary since they are initialized to false. kept for clarity
			else if (elem.Attribute("value") == string("False") )
				m_TilesetCollisionData[currentTile%m_TilesetColumns][currentTile/m_TilesetColumns] = false;
			//else
			//	m_TilesetCollisionData[currentTile%m_TilesetColumns][currentTile/m_TilesetColumns] = true;
		}
	}

    return true;
}

 bool 	TMXLoader::VisitExit (const TiXmlElement &elem)
{
    return true;
}

 bool 	TMXLoader::Visit (const TiXmlDeclaration &dec)
{
    return true;
}

 bool 	TMXLoader::Visit (const TiXmlText &text)
{
    return true;
}

 bool 	TMXLoader::Visit (const TiXmlComment &comment)
{
    return true;
}

 bool 	TMXLoader::Visit (const TiXmlUnknown &unknown)
{
    return true;
}

bool TMXLoader::loadDocument()
{
    //TiXmlDocument doc("testCollision.tmx");
	//TiXmlDocument doc("testCollision.tmx");
	TiXmlDocument doc("HoboVille.tmx");

    if ( ! doc.LoadFile() ) {
		return false;
	}

    //TiXmlElement* elem = doc.RootElement();

    doc.Accept(this);

	// DEBUG output collision array to check correctness
	std::cout << std::endl;
	// NOT CORRECT
	for (int i = 0; i < m_NumMapRows; ++i)
	{
		for (int j = 0; j < m_NumMapColumns; ++j)
		{
			if (m_MapCollisionData[j][i])
				std::cout << "T";
			else
				std::cout << "F";

		}
		std::cout << std::endl;	
	}

	// correct
	for (int i = 0; i < m_TilesetRows; ++i)
	{
		for (int j = 0; j < m_TilesetColumns; ++j)
		{
			if (m_TilesetCollisionData[j][i])
				std::cout << "T";
			else
				std::cout << "F";
		}
		std::cout << std::endl;	
	}

    return true;
}

void TMXLoader::decode_and_store_map_data( string encoded_data )
{
    //const int NUM_LAYER_COLS = 3;
   // const int NUM_LAYER_ROWS = 3;
   // const int NUM_TILES = NUM_LAYER_ROWS * NUM_LAYER_COLS;
    //int m_LayerData[NUM_LAYER_ROWS][NUM_LAYER_COLS];
    //string encoded_data = elem.GetText();
    //string unencoded_data = base64_decode(encoded_data);
    //const char* unencoded_c_str = unencoded_data.c_str();
    //const char* unencoded_c_str = unencoded_data.c_str();
    //m_LayerData.push_back( vector<> );

    vector< int > layerDataRow( getNumMapColumns() );
    int m_LayerRow = 0;
    int m_LayerCol = 0;
    vector<int> unencoded_data = base64_decode(encoded_data);
    for (int i = 0; i < getNumMapRows(); i++)
    {
        m_LayerData.push_back( layerDataRow );
    }	

	vector< bool > mapCollisionDataColumn(getNumMapRows() );
	for (int i = 0; i < getNumMapColumns(); ++i)
	{
		m_MapCollisionData.push_back( mapCollisionDataColumn );
	}

    for (int i = 0; i < unencoded_data.size(); i += 4)
    {
        // Get the grid ID

        int a = unencoded_data[i];
        int b = unencoded_data[i + 1];
        int c = unencoded_data[i + 2];
        int d = unencoded_data[i + 3];

        int gid = a | b << 8 | c << 16 | d << 24;

        m_LayerData[m_LayerRow][m_LayerCol] = gid;
		m_MapCollisionData[m_LayerCol][m_LayerRow] = m_TilesetCollisionData[(gid-1)%m_TilesetColumns][(gid-1)/m_TilesetColumns];

        if ((i + 4) % ( getNumMapColumns() * 4) == 0) {
            m_LayerRow++;
            m_LayerCol = 0;
            //myfile << endl;
        }
        else {
            m_LayerCol++;
        }
    }



//    for (int row = 0; row < NUM_LAYER_ROWS; row++)
//    {
//        for (int col = 0; col < NUM_LAYER_COLS; col++)
//        {
//           myfile << " (" << m_LayerData[row][col] << ") ";
//        }
//
//        myfile << endl;
//    }
}
/*****************************************************/
SDL_Surface* TMXLoader::load_SDL_image( std::string filename )
{
    //The image that's loaded
    SDL_Surface* loadedImage = NULL;

    //The optimized surface that will be used
    SDL_Surface* optimizedImage = NULL;

    //Load the image
    loadedImage = IMG_Load( filename.c_str() );

	if( loadedImage == NULL )
    {
        printf( "Unable to load image %s! SDL_image Error: %s\n", filename.c_str(), IMG_GetError() );
    }

	else
    {
        //Convert surface to screen format
		// NOTE: the loadedImage->format should be screenSurface->format, but we dont have access. This works, but perhaps it isnt optimizing correctly
		optimizedImage = SDL_ConvertSurface( loadedImage, loadedImage->format, NULL );
        if( optimizedImage == NULL )
        {
            printf( "Unable to optimize image %s! SDL Error: %s\n", filename.c_str(), SDL_GetError() );
        }

        //Get rid of old loaded surface
		SDL_FreeSurface( loadedImage );
    }

    //Return the optimized surface
    //return optimizedImage;
	return optimizedImage;
}





void TMXLoader::buildMapImage()
{
    // We must find a way to get these values from the layer at runtime!
    //const int NUM_TILESET_COLS = 18;
   // const int NUM_TILESET_ROWS = 11;
	//const int NUM_TILESET_COLS = (m_ImageWidth-m_TilesetMargin) / (m_TileWidth + m_TileSpacing);
	//const int NUM_TILESET_ROWS = (m_ImageHeight-m_TilesetMargin) / (m_TileHeight + m_TileSpacing);

	std::cout << "Rows:Cols of tileset -> " << m_TilesetRows << ":" << m_TilesetColumns << std::endl;

    //const int MARGIN = 2;
    //const int NUM_TILES = NUM_LAYER_ROWS * NUM_LAYER_COLS;

    img_map = SDL_CreateRGBSurface( SDL_SWSURFACE, getNumMapColumns() * 16, getNumMapRows() * 16, 16, 0, 0, 0, 0 );

	if(img_map == NULL) 
	{
        fprintf(stderr, "CreateRGBSurface failed: %s\n", SDL_GetError());
        exit(1);
    }

    //SDL_Surface *src;
    //SDL_Surface *dst;
    SDL_Rect srcrect;
    SDL_Rect dstrect;

    for (int row = 0; row < getNumMapRows(); row++)
    {
        for (int col = 0; col < getNumMapColumns(); col++)
        {
            int gid = m_LayerData[row][col];

            if (gid == 0)
                continue;


            myfile << "\nGID is " << gid;

            int tileset_col;
            int tileset_row;



            gid--; // convert to something we can use with the 0 based system.

			//tileset_col = (gid % m_TilesetRows);
			tileset_col = (gid % m_TilesetColumns);
			tileset_row = gid / m_TilesetColumns;


            srcrect.x = getTilesetMargin() + (getTileSpacing() + 16) * tileset_col;
            srcrect.y = getTilesetMargin() + (getTileSpacing() + 16) * tileset_row;
            srcrect.w = getTileWidth();
            srcrect.h = getTileHeight();

            dstrect.x = col * getTileWidth();
            dstrect.y = row * getTileHeight();
            dstrect.w = getTileWidth();
            dstrect.h = getTileHeight();

            SDL_BlitSurface(img_tileset, &srcrect, img_map, &dstrect);
        }
    }
}

void TMXLoader::buildCollisionArray ()
{
	m_TilesetColumns = (m_ImageWidth-m_TilesetMargin) / (m_TileWidth + m_TileSpacing);
	m_TilesetRows = (m_ImageHeight-m_TilesetMargin) / (m_TileHeight + m_TileSpacing);

	vector< bool > collisionDataColumn( getNumMapRows(), true );

	/*
	for (int i = 0; i < collisionDataColumn.size(); ++i)
	{
		collisionDataColumn[i] = false;
	}*/

	for (int i = 0; i < getNumMapColumns(); ++i)
	{
		m_TilesetCollisionData.push_back( collisionDataColumn );
	}
}


SDL_Surface* TMXLoader::getMapImage()
{
    return img_map;
}
