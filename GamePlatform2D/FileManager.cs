using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GamePlatform2D
{
    public class FileManager
    {
        enum LoadType { Attributes, Contents };

        List<List<string>> attributes = new List<List<string>>();
        List<List<string>> contents = new List<List<string>>();
    }
}
