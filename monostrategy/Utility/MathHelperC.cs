using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace monostrategy.Utility
{
    class MathHelperC
    {
        public static float PI = (float)Math.PI;
        private static Random rand = new Random(2);

        public static Random Rand
        {
            get { return MathHelperC.rand; }
            set { MathHelperC.rand = value; }
        }

        public static float RandomFloat()
        {
            return (float)rand.NextDouble() - 0.5f;
        }

        public static float RandomFloat(Random rand)
        {
            return (float)rand.NextDouble() - 0.5f;
        }

        public static float Pow(float x, float y)
        {
            return (float)Math.Pow((double)x, (double)y);
        }

        public static float GetAngle(Vector2 v)
        {
            return (float)Math.Atan2((float)v.Y, (float)v.X);
        }

        public static Vector2 RandomVector()
        {
            Vector2 v = new Vector2(RandomFloat(), RandomFloat());
            v.Normalize();

            return v;
        }

        public static Vector2 GetVector2(float radians)
        {
            return new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
        }

        public static float ToRadians(float degrees)
        {
            return degrees * PI / 180;
        }

        public static float ToDegrees(float radians)
        {
            return radians * 180 / PI;
        }

        public static Vector2 RotateAroundPoint(Vector2 point, Vector2 origin, float radians)
        {
            float rotation = radians;
            Vector2 u = point - origin;

            if (u == Vector2.Zero)
                return point;

            float a = (float)Math.Atan2(u.Y, u.X);
            a += rotation;
            u = new Vector2((float)Math.Cos(a), (float)Math.Sin(a)) * u.Length();
            return u + origin;
        }

        public static List<Vector2> LinePositions(Vector2 start, Vector2 stop)
        {
            List<Vector2> coordinates = new List<Vector2>();
            int width = 0;
            int x1 = (int)((start.X + 0) / 100);
            int y1 = (int)((start.Y + 0) / 100);

            int x2 = (int)((stop.X + 0) / 100);
            int y2 = (int)((stop.Y + 0) / 100);


            int dx = x2 - x1;
            int dy = y2 - y1;


            int ax = Math.Abs(dx) * 2;
            int ay = Math.Abs(dy) * 2;

            int sx = Math.Sign(dx);
            int sy = Math.Sign(dy);


            int x = x1;
            int y = y1;

            if (ax >= ay) // x dominant
            {
                float yd = ay - ax / 2;

                while (true)
                {
                    for (int yw = -width; yw <= width; yw++)
                        coordinates.Add(new Vector2(x, y + yw) * 100);

                    if (x == x2)
                        break;
                    if (yd >= 0)
                    {
                        y = y + sy;
                        yd = yd - ax;

                        for (int yw = -width; yw <= width; yw++)
                            coordinates.Add(new Vector2(x, y + yw) * 100);
                    }

                    x = x + sx;
                    yd = yd + ay;
                }
            }
            else if (ay >= ax) //y dominant
            {
                float xd = ax - ay / 2;

                while (true)
                {
                    for (int xw = -width; xw <= width; xw++)
                        coordinates.Add(new Vector2(x + xw, y) * 100);
                    if (y == y2)
                        break;
                    if (xd >= 0)
                    {
                        x = x + sx;
                        xd = xd - ay;

                        for (int xw = -width; xw <= width; xw++)
                            coordinates.Add(new Vector2(x + xw, y) * 100);
                    }
                    y = y + sy;
                    xd = xd + ax;
                }
            }

            return coordinates;
        }

        #region Intersections
        public static bool IntersectsCircleCircle(Vector2 p1, float r1, Vector2 p2, float r2)
        {
            return (p1 - p2).Length() < r1 + r2;
        }

        private static bool FindLineIntersection(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2, out Vector2 poi)
        {
            poi = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
            float denom = ((end1.X - start1.X) * (end2.Y - start2.Y)) - ((end1.Y - start1.Y) * (end2.X - start2.X));

            //  AB & CD are parallel 
            if (denom == 0)
                return false;

            float numer = ((start1.Y - start2.Y) * (end2.X - start2.X)) - ((start1.X - start2.X) * (end2.Y - start2.Y));

            float r = numer / denom;

            float numer2 = ((start1.Y - start2.Y) * (end1.X - start1.X)) - ((start1.X - start2.X) * (end1.Y - start1.Y));

            float s = numer2 / denom;

            if ((r < 0 || r > 1) || (s < 0 || s > 1))
                return false;

            // Find intersection point
            poi = new Vector2();
            poi.X = start1.X + (r * (end1.X - start1.X));
            poi.Y = start1.Y + (r * (end1.Y - start1.Y));

            return true;
        }

        public static bool IntesectsLineCircle(Vector2 circleOrigin, float radius, Vector2 lineStart, Vector2 lineEnd)
        {
            Vector2 dir = lineEnd - lineStart;
            Vector2 diff = circleOrigin - lineStart;
            float t = Vector2.Dot(diff, dir) / Vector2.Dot(dir, dir);
            if (t < 0.0f)
                t = 0.0f;
            if (t > 1.0f)
                t = 1.0f;
            Vector2 closest = lineStart + t * dir;
            Vector2 d = circleOrigin - closest;
            float distsqr = Vector2.Dot(d, d);
            return distsqr <= radius * radius;



            /*Vector2 newOrigin = circleOrigin - lineStart;
            Vector2 vtRay = lineEnd - lineStart;
            if (vtRay == Vector2.Zero)
                return false;
            vtRay.Normalize();
            Vector2 pointOfIntersection = Vector2.Zero;
            float t = Vector2.Dot(newOrigin, vtRay) / Vector2.Dot(vtRay, vtRay);
            if (t < 0)
                return false;

            float b = newOrigin.X * vtRay.X + vtRay.Y * newOrigin.Y;
            float c = radius * radius - (newOrigin.X * newOrigin.X + newOrigin.Y * newOrigin.Y);
            float a = vtRay.X * vtRay.X + vtRay.Y * vtRay.Y;

            float diva = 1.0f / a;

            if((b*b + a*c) <= 0)
                return false;

            float b4ac = (float)Math.Sqrt((b * b + a * c));
            float l1 = (b - b4ac) * diva;
            float l2 = (b + b4ac) * diva;

            float l;
            if (l1 < l2)
                l = l1;
            else
                l = l2;

            pointOfIntersection = (vtRay * l) + lineStart;
            return true;*/
        }

        public static bool IntersectsCircleRectangle(Vector2 circleOrigin, float radius, Vector2 rectangleOrigin, float width, float height)
        {
            float circleDistanceX = Math.Abs(circleOrigin.X - rectangleOrigin.X - width / 2);
            float circleDistanceY = Math.Abs(circleOrigin.Y - rectangleOrigin.Y - height / 2);

            if (circleDistanceX > (width / 2 + radius)) { return false; }
            if (circleDistanceY > (height / 2 + radius)) { return false; }

            if (circleDistanceX <= (width / 2)) { return true; }
            if (circleDistanceY <= (height / 2)) { return true; }

            float cornerDistance_sq = (float)(Math.Pow((circleDistanceX - width / 2), 2) +
                                 Math.Pow((circleDistanceY - height / 2), 2));

            return (cornerDistance_sq <= (Math.Pow(radius, 2)));
        }

        public static float GetShortestAngle(float target, float current)
        {
            float angle = (target - current);
            while (angle > PI)
                angle -= 2 * PI;
            while (angle < -PI)
                angle += 2 * PI;

            return angle;
        }
        #endregion
    }

}
