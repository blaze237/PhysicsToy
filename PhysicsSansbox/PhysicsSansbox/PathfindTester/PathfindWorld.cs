using PhysicsSansbox.TileRender;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsSansbox.PathfindTester
{
    internal class PathfindWorld : World
    {
        //-----------------------
        public PathfindWorld() 
        {
        }

        //-----------------------
        public override Renderer CreateRenderer
        (
        )
        {
            return new TileRenderer(25, Program.c_screenWidth, Program.c_screenHeight);
        }

        //-----------------------
        public override void Init
        (
        )
        {
           
        }

        //-----------------------
        public override void Destroy
        (
        )
        {
            
        }

        //-----------------------
        public override void FixedUpdate
        (
            float i_fixedDeltaTime
        )
        {
            //Color[] colours = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Magenta, Color.Black };
            //const float changeChance = 0.05f;
            //for ( int i = 0; i < 25; i++)
            //{
            //    for (int j = 0; j < 25; j++)
            //    {
            //        if (Random.Shared.NextSingle() < changeChance)
            //        {
            //            ((TileRenderer)m_renderer).SetTileColour(i, j, colours[Random.Shared.Next(0, colours.Length)]);
            //        }
            //    }
            //}

        }

        //-----------------------
        public override void Update
        (
            float i_deltaTime
        )
        {
            Color[] colours = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Magenta, Color.Black };
            const float changeChance = 0.05f;
            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    if (Random.Shared.NextSingle() < changeChance)
                    {
                        ((TileRenderer)m_renderer).SetTileColour(i, j, colours[Random.Shared.Next(0, colours.Length)]);
                    }
                }
            }
        }

       


        //Members
    }
}
