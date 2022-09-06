using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.MeshSelecting
{
    public static class ScreenRectUtility
    {
        private static readonly Texture2D _texture;

        static ScreenRectUtility()
        {
            _texture = new Texture2D(1, 1);
            _texture.SetPixel(0, 0, Color.white);
            _texture.Apply();
        }
        
        public static void DrawRect(Rect rect, Color color)
        {
            using (new GUIColor(color)) 
                GUI.DrawTexture(rect, _texture);
        }

        public static void DrawBorder(Rect rect, float thickness, Color color)
        {
            DrawRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            DrawRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            DrawRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
            DrawRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        }

        public static Rect RectFromPositions(Vector2 first, Vector2 second)
        {
            // Move origin from bottom left to top left
            first.y = Screen.height - first.y;
            second.y = Screen.height - second.y;
            
            Vector3 topLeft = Vector3.Min(first, second);
            Vector3 bottomRight = Vector3.Max(first, second);
            
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }

        public static Rect ScreenToWorldRect(Rect screenRect, Camera camera)
        {
            Vector3 topLeft = camera.ScreenToWorldPoint(screenRect.min);
            Vector3 botRight = camera.ScreenToWorldPoint(screenRect.max);

            return Rect.MinMaxRect(topLeft.x, topLeft.y, botRight.x, botRight.y);
        }
    }
}
