using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astro4x
{
    public static class Camera2D
    {
        public static Vector2 currentPosition = Vector2.Zero;
        public static Vector2 targetPosition = Vector2.Zero;
        public static Point levelMin = new Point(168, 90);
        public static Point levelMax = new Point(360, 1024 - 16 * 8);
        public static Boolean tracksLoosely = true; //moves, !teleport

        //how fast the camera moves
        public static float speed_x = 0.04f;
        public static float speed_y = 0.07f; //mutated by level screen
        public static Vector2 distance;
        public static float targetZoom = 1.0f; //always 1.0
        public static float currentZoom = 1.0f;
        public static float zoomSpeed = 0.05f;
        public static Matrix view = Matrix.Identity;
        public static Matrix matRotation = Matrix.CreateRotationZ(0.0f);
        public static Matrix matZoom;
        public static Vector3 translateCenter;
        public static Vector3 translateBody;




        static Camera2D() { }

        public static Point point; //used in conversion functions below

        public static Point ConvertScreenToWorld(int x, int y)
        {   //get the camera position minus half width/height of render surface
            //this location is the world position of top left screen position
            //add the x,y to this location
            point.X = x + (int)Camera2D.currentPosition.X - ScreenManager.GAME_WIDTH / 2;
            point.Y = y + (int)Camera2D.currentPosition.Y - ScreenManager.GAME_HEIGHT / 2;
            //this final value is the world position of the screen position
            return point;
        }

        public static Point ConvertWorldToScreen(int x, int y)
        {   //subtract world position of top left screen position from x,y
            point.X = x - ((int)Camera2D.currentPosition.X - ScreenManager.GAME_WIDTH / 2);
            point.Y = y - ((int)Camera2D.currentPosition.Y - ScreenManager.GAME_HEIGHT / 2);
            //this final value is the screen position of the world position
            return point;
        }

        public static void SetBounds()
        {
            levelMin.X = 280;
            levelMin.Y = 150; 

            levelMax.X = 930;
            levelMax.Y = 510; 
        }
        
        public static void SetView()
        {   //adapt the camera's center to the renderSurface.size
            translateCenter.X = ScreenManager.RT2D.Width / 2;
            translateCenter.Y = ScreenManager.RT2D.Height / 2;
            translateCenter.Z = 0;
            translateBody.X = -currentPosition.X;
            translateBody.Y = -currentPosition.Y;
            translateBody.Z = 0;
            //allows camera to properly zoom
            matZoom = Matrix.CreateScale(currentZoom, currentZoom, 1);
            view = Matrix.CreateTranslation(translateBody) *
                    matRotation * matZoom *
                    Matrix.CreateTranslation(translateCenter);
        }

        public static void Update()
        {
            //discard sub-pixel values from position
            targetPosition.X = (int)targetPosition.X;
            targetPosition.Y = (int)targetPosition.Y;

            if (tracksLoosely) //camera loosely follows 'target'
            {
                //get distance between current and target
                distance = targetPosition - currentPosition;
                //check to see if camera is close enough to snap, or should move
                if (Math.Abs(distance.X) < 1)
                { currentPosition.X = targetPosition.X; }
                else //camera should move
                { currentPosition.X += distance.X * speed_x; }

                if (Math.Abs(distance.Y) < 1)
                { currentPosition.Y = targetPosition.Y; }
                else //camera should move
                { currentPosition.Y += distance.Y * speed_y; }
            }

            else //camera tightly follows 'target'
            {
                //Console.WriteLine("tight tracking cam pos: " +
                //    Camera2D.currentPosition.X + "," + Camera2D.currentPosition.Y);
                //Console.WriteLine("target pos: " +
                //    Camera2D.targetPosition.X + "," + Camera2D.targetPosition.Y);

                //get distance between current and target
                float diffX = targetPosition.X - currentPosition.X;
                float diffY = targetPosition.Y - currentPosition.Y;

                //see if camera should move per axis
                if (Math.Abs(diffX) > 0.9f) //move cam if too far
                {
                    currentPosition.X += diffX * speed_x;
                    if (diffX > 0) { currentPosition.X++; }
                    else if (diffX < 0) { currentPosition.X--; }
                }
                if (Math.Abs(diffY) > 0.9f) //move cam if too far
                {
                    currentPosition.Y += diffY * speed_y;
                    if (diffY > 0) { currentPosition.Y++; }
                    else if (diffY < 0) { currentPosition.Y--; }
                }

                //camera 'teleports' to target, no movement
                //Camera2D.currentPosition = Camera2D.targetPosition;
            }



            //discard sub-pixel values from position
            currentPosition.X = (int)currentPosition.X;
            currentPosition.Y = (int)currentPosition.Y;


            //bound camera pos to inside level, so it presents nicely
            if(targetZoom <= 1.0f)
            {
                if (currentPosition.X < levelMin.X)
                { currentPosition.X += 2; targetPosition.X = currentPosition.X; }
                else if (currentPosition.X > levelMax.X)
                { currentPosition.X -= 2; targetPosition.X = currentPosition.X; }

                if (currentPosition.Y < levelMin.Y)
                { currentPosition.Y += 2; targetPosition.Y = currentPosition.Y; }
                else if (currentPosition.Y > levelMax.Y)
                { currentPosition.Y -= 2; targetPosition.Y = currentPosition.Y; }
            }
            



            if (currentZoom != targetZoom)
            {   //gradually match the zoom
                if (currentZoom > targetZoom) { currentZoom -= zoomSpeed; } //zoom out
                if (currentZoom < targetZoom) { currentZoom += zoomSpeed; } //zoom in
                if (Math.Abs((currentZoom - targetZoom)) < 0.05f)
                { currentZoom = targetZoom; } //limit zoom
            }

            SetView();


            //ScreenManager.Text_Debug.text += "\nCAM:" + currentPosition.X + ", " + currentPosition.Y;
        }



    }
}
