using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WeWereBound
{
    public static class Calc
    {
        #region Enums

        public static int EnumLength(Type e)
        {
            return Enum.GetNames(e).Length;
        }

        public static T StringToEnum<T>(string str) where T : struct
        {
            if (Enum.IsDefined(typeof(T), str)) return (T)Enum.Parse(typeof(T), str);
            else throw new Exception("The string cannot be converted to the enum type");
        }

        public static T[] StringsToEnums<T>(string[] strs) where T : struct
        {
            T[] ret = new T[strs.Length];
            for (int i = 0; i < strs.Length; i++)
                ret[i] = StringToEnum<T>(strs[i]);

            return ret;
        }

        public static bool EnumHasString<T>(string str) where T : struct
        {
            return Enum.IsDefined(typeof(T), str);
        }

        #endregion

        #region Strings

        public static bool StartsWith(this string str, string match)
        {
            return str.IndexOf(match) == 0;
        }

        public static bool EndsWith(this string str, string match)
        {
            return str.LastIndexOf(match) == str.Length - match.Length;
        }

        public static bool IsIgnoreCase(this string str, params string[] matches)
        {
            if (string.IsNullOrEmpty(str)) return false;

            foreach (var match in matches)
                if (str.Equals(match, StringComparison.InvariantCultureIgnoreCase)) return true;

            return false;
        }

        public static string ToString(this int num, int minDigits)
        {
            string ret = num.ToString();
            while (ret.Length < minDigits) ret = "0" + ret;
            return ret;
        }

        public static string[] SplitLines(string text, SpriteFont font, int maxLineWidth, char newLine = '\n')
        {
            List<string> lines = new List<string>();

            foreach (var forcedLine in text.Split(newLine))
            {
                string line = "";

                foreach (string word in forcedLine.Split(' '))
                {
                    if (font.MeasureString($"{line} ${word}").X > maxLineWidth)
                    {
                        lines.Add(line);
                        line = word;
                    }
                    else
                    {
                        if (line != "") line += " ";
                        line += word;
                    }
                }

                lines.Add(line);
            }

            return lines.ToArray();
        }

        #endregion

        #region Count

        public static int Count<T>(T target, T a, T b)
        {
            int num = 0;

            if (a.Equals(target)) num++;
            if (b.Equals(target)) num++;

            return num;
        }

        public static int Count<T>(T target, T a, T b, T c)
        {
            int num = 0;

            if (a.Equals(target)) num++;
            if (b.Equals(target)) num++;
            if (c.Equals(target)) num++;

            return num;
        }

        public static int Count<T>(T target, T a, T b, T c, T d)
        {
            int num = 0;

            if (a.Equals(target)) num++;
            if (b.Equals(target)) num++;
            if (c.Equals(target)) num++;
            if (d.Equals(target)) num++;

            return num;
        }

        public static int Count<T>(T target, T a, T b, T c, T d, T e)
        {
            int num = 0;

            if (a.Equals(target)) num++;
            if (b.Equals(target)) num++;
            if (c.Equals(target)) num++;
            if (d.Equals(target)) num++;
            if (e.Equals(target)) num++;

            return num;
        }

        //ok. there has to be a better way to do this.
        public static int Count<T>(T target, T a, T b, T c, T d, T e, T f)
        {
            int num = 0;

            if (a.Equals(target)) num++;
            if (b.Equals(target)) num++;
            if (c.Equals(target)) num++;
            if (d.Equals(target)) num++;
            if (e.Equals(target)) num++;
            if (f.Equals(target)) num++;

            return num;
        }

        //this should do it in theory
        public static int Count<T>(T target, params T[] matchers)
        {
            int num = 0;

            foreach (var m in matchers)
                if (m.Equals(target)) num++;

            return num;
        }

        #endregion

        #region Give Me

        public static T GiveMe<T>(int index, T a, T b)
        {
            switch (index)
            {
                default:
                    throw new Exception("Index out of range.");
                case 0:
                    return a;
                case 1:
                    return b;
            }
        }

        //ok seriously. not sure why this is being overloaded the way it is so in theory this should work for an infinitesimal set of gives but maybe it won't work who knows
        public static T GiveMe<T>(int index, params T[] toGive)
        {
            for (int i = 0; i < toGive.Length; i++)
                if (index == i) return toGive[i];

            throw new Exception("Index out of range.");
        }

        #endregion

        #region Random

        public static Random Random = new Random();
        private static Stack<Random> randomStack = new Stack<Random>();

        public static void PushRandom(int newSeed)
        {
            randomStack.Push(Calc.Random);
            Calc.Random = new Random(newSeed);
        }

        public static void PushRandom(Random random)
        {
            randomStack.Push(Calc.Random);
            Calc.Random = random;
        }

        public static void PushRandom()
        {
            randomStack.Push(Calc.Random);
            Calc.Random = new Random();
        }

        public static void PopRandom()
        {
            Calc.Random = randomStack.Pop();
        }

        #region Choose

        public static T Choose<T>(this Random random, T a, T b)
        {
            return GiveMe<T>(random.Next(2), a, b);
        }

        //like... this should work.... for everything........... right.........
        public static T Choose<T>(this Random random, params T[] chooseOne)
        {
            // originally was return GiveMe<T>(random.Next(chooseOne.Length), chooseOne);
            return chooseOne[random.Next(chooseOne.Length)];
        }

        //wait a second............
        //public static T Choose<T>(this Random random, params T[] choices)
        //{
        //    return choices[random.Next(choices.Length)];
        //}

        public static T Choose<T>(this Random random, List<T> choices)
        {
            return choices[random.Next(choices.Count)];
        }

        #endregion

        #region Range


        //This method was removed from the C# specification or Matt defined it elsewhere.
        //This is my own implementation that I think does the job
        //technically less performant compared to if there was an actual NextFloat method since double precision but oh well
        public static float NextFloat(this Random random, float min, float max)
        {
            return (float)random.NextDouble() * (max - min);
        }

        public static float NextFloat(this Random random, float max)
        {
            return (float)random.NextDouble() * max;
        }

        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        public static int Range(this Random random, int min, int max)
        {
            return min + random.Next(max - min);
        }

        public static float Range(this Random random, float min, float max)
        {
            return min + random.NextFloat(max - min);
        }

        public static Vector2 Range(this Random random, Vector2 min, Vector2 max)
        {
            return min + new Vector2(random.NextFloat(min.X, max.X), random.NextFloat(min.Y, max.Y));
        }

        #endregion

        public static int Facing(this Random random)
        {
            return (random.NextFloat() < 0.5f ? -1 : 1);
        }

        public static bool Chance(this Random random, float chance)
        {
            return random.NextFloat() < chance;
        }

        public static float NextAngle(this Random random)
        {
            return random.NextFloat() * MathHelper.TwoPi;
        }

        private static int[] shakeVectorOffsets = new int[] { -1, -1, 0, 1, 1 };

        public static Vector2 ShakeVector(this Random random)
        {
            return new Vector2(random.Choose(shakeVectorOffsets), random.Choose(shakeVectorOffsets));
        }

        #endregion

        #region Lists

        public static Vector2 ClosestTo(this List<Vector2> list, Vector2 to)
        {
            Vector2 best = list[0];
            float distSq = Vector2.DistanceSquared(list[0], to);

            for (int i = 1; i < list.Count; i++)
            {
                float d = Vector2.DistanceSquared(list[i], to);
                if (d < distSq)
                {
                    distSq = d;
                    best = list[i];
                }
            }

            return best;
        }

        public static Vector2 ClosestTo(this Vector2[] list, Vector2 to)
        {
            Vector2 best = list[0];
            float distSq = Vector2.DistanceSquared(list[0], to);

            for (int i = 1; i < list.Length; i++)
            {
                float d = Vector2.DistanceSquared(list[i], to);
                if (d < distSq)
                {
                    distSq = d;
                    best = list[i];
                }
            }

            return best;
        }

        public static Vector2 ClosestTo(this Vector2[] list, Vector2 to, out int index)
        {
            index = 0;
            Vector2 best = list[0];
            float distSq = Vector2.DistanceSquared(list[0], to);

            for (int i = 1; i < list.Length; i++)
            {
                float d = Vector2.DistanceSquared(list[i], to);
                if (d < distSq)
                {
                    index = i;
                    distSq = d;
                    best = list[i];
                }
            }

            return best;
        }

        public static void Shuffle<T>(this List<T> list, Random random)
        {
            int i = list.Count;
            int j;
            T t;

            while (--i > 0)
            {
                t = list[i];
                list[i] = list[j = random.Next(i + 1)];
                list[j] = t;
            }
        }

        public static void Shuffle<T>(this List<T> list)
        {
            list.Shuffle(Random);
        }

        public static void ShuffleSetFirst<T>(this List<T> list, Random random, T first)
        {
            int amount = 0;
            while (list.Contains(first))
            {
                list.Remove(first);
                amount++;
            }

            list.Shuffle(random);

            for (int i = 0; i < amount; i++) list.Insert(0, first);
        }

        public static void ShuffleSetFirst<T>(this List<T> list, T first)
        {
            list.ShuffleSetFirst(Random, first);
        }

        public static void ShuffleNotFirst<T>(this List<T> list, Random random, T notFirst)
        {
            int amount = 0;
            while (list.Contains(notFirst))
            {
                list.Remove(notFirst);
                amount++;
            }

            list.Shuffle(random);

            for (int i = 0; i < amount; i++) list.Insert(random.Next(list.Count - 1) + 1, notFirst);
        }

        public static void ShuffleNotFirst<T>(this List<T> list, T notFirst)
        {
            list.ShuffleNotFirst(Random, notFirst);
        }

        #endregion

        #region Colors

        public static Color Invert(this Color color)
        {
            return new Color(255 - color.R, 255 - color.G, 255 - color.B, color.A);
        }

        public static Color HexToColor(string hex)
        {
            if (hex.Length >= 6)
            {
                float r = (HexToByte(hex[0]) * 16 + HexToByte(hex[1])) / 255.0f;
                float g = (HexToByte(hex[2]) * 16 + HexToByte(hex[3])) / 255.0f;
                float b = (HexToByte(hex[4]) * 16 + HexToByte(hex[5])) / 255.0f;
                return new Color(r, g, b);
            }
            return Color.White;
        }

        #endregion

        #region Time

        public static string ShortGameplayFormat(this TimeSpan time)
        {
            if (time.TotalHours >= 1) return $"{((int)time.Hours)}:${time.ToString(@"mm\:ss\.fff")}";
            else return time.ToString(@"m\:ss\.fff");
        }

        public static string LongGameplayFormat(this TimeSpan time)
        {
            StringBuilder str = new StringBuilder();

            if (time.TotalDays >= 2)
            {
                str.Append((int)time.TotalDays);
                str.Append(" days, ");
            }
            else if (time.TotalDays >= 1) str.Append("1 day, ");

            str.Append((time.TotalHours - ((int)time.TotalDays * 24)).ToString("0.0"));
            str.Append(" hours");

            return str.ToString();
        }

        #endregion

        #region Math

        public const float Right = 0;
        public const float Up = -MathHelper.PiOver2;
        public const float Left = MathHelper.Pi;
        public const float Down = MathHelper.PiOver2;
        public const float UpRight = -MathHelper.PiOver4;
        public const float UpLeft = -MathHelper.PiOver4 - MathHelper.PiOver2;
        public const float DownRight = MathHelper.PiOver4;
        public const float DownLeft = MathHelper.PiOver4 + MathHelper.PiOver2;
        public const float DegToRad = MathHelper.Pi / 180f;
        public const float RadToDeg = 180f / MathHelper.Pi;
        public const float DtR = DegToRad;
        public const float RtD = RadToDeg;
        public const float Circle = MathHelper.TwoPi;
        public const float HalfCircle = MathHelper.Pi;
        public const float QuarterCircle = MathHelper.PiOver2;
        public const float EighthCircle = MathHelper.PiOver4;
        private const string Hex = "0123456789ABCDEF";

        public static int Digits(this int num)
        {
            int digits = 1;
            int target = 10;

            while (num >= target)
            {
                digits++;
                target *= 10;
            }

            return digits;
        }

        public static byte HexToByte(char c)
        {
            return (byte)Hex.IndexOf(char.ToUpper(c));
        }

        public static float Percent(float num, float zeroAt, float oneAt)
        {
            return MathHelper.Clamp((num - zeroAt) / oneAt, 0, 1);
        }

        public static float SignThreshold(float value, float threshold)
        {
            if (Math.Abs(value) >= threshold) return Math.Sign(value);
            else return 0;
        }

        public static float Min(params float[] values)
        {
            float min = values[0];
            for (int i = 1; i < values.Length; i++) min = MathHelper.Min(values[i], min);

            return min;
        }

        public static float Max(params float[] values)
        {
            float max = values[0];
            for (int i = 1; i < values.Length; i++) max = MathHelper.Max(values[i], max);

            return max;
        }

        public static float ToRad(this float f)
        {
            return f * DegToRad;
        }

        public static float toDeg(this float f)
        {
            return f * RadToDeg;
        }

        //modified to be less verbose
        public static int Axis(bool negative, bool positive, int both = 0)
        {
            if (negative ^ positive) return positive ? 1 : -1;
            else if (negative && positive) return both;
            else return 0;
        }

        public static int Clamp(int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        public static float Clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        public static float YoYo(float value)
        {
            if (value <= .5f) return value * 2;
            else return 1 - ((value - .5f) * 2);
        }

        public static float Map(float val, float min, float max, float newMin = 0, float newMax = 1)
        {
            return ((val - min) / (max - min)) * (newMax - newMin) + newMin;
        }

        public static float SineMap(float counter, float newMin, float newMax)
        {
            return Calc.Map((float)Math.Sin(counter), 01, 1, newMin, newMax);
        }

        public static float ClampedMap(float val, float min, float max, float newMin = 0, float newMax = 1)
        {
            return MathHelper.Clamp((val - min) / (max - min), 0, 1) * (newMax - newMin) + newMin;
        }

        public static float LerpSnap(float value1, float value2, float amount, float snapThreshold = .1f)
        {
            float ret = MathHelper.Lerp(value1, value2, amount);
            if (Math.Abs(ret - value2) < snapThreshold) return value2;
            else return ret;
        }

        public static float LerpClamp(float value1, float value2, float lerp)
        {
            return MathHelper.Lerp(value1, value2, MathHelper.Clamp(lerp, 0, 1));
        }

        public static Vector2 LerpSnap(Vector2 value1, Vector2 value2, float amount, float snapThresholdSq = .1f)
        {
            Vector2 ret = Vector2.Lerp(value1, value2, amount);
            if ((ret - value2).LengthSquared() < snapThresholdSq) return value2;
            else return ret;
        }

        public static Vector2 Sign(this Vector2 vec)
        {
            return new Vector2(Math.Sign(vec.X), Math.Sign(vec.Y));
        }

        public static Vector2 SafeNormalize(this Vector2 vec, Vector2 ifZero, float length)
        {
            if (vec == Vector2.Zero) return ifZero * length;
            else
            {
                vec.Normalize();
                return vec * length;
            }
        }

        public static Vector2 SafeNormalize(this Vector2 vec, Vector2 ifZero)
        {
            if (vec == Vector2.Zero) return ifZero;
            else
            {
                vec.Normalize();
                return vec;
            }
        }

        public static Vector2 SafeNormalize(this Vector2 vec, float length)
        {
            return SafeNormalize(vec, Vector2.Zero, length);
        }

        public static Vector2 SafeNormalize(this Vector2 vec)
        {
            return SafeNormalize(vec, Vector2.Zero);
        }

        public static float ReflectAngle(float angle, float axis = 0)
        {
            return -(angle + axis) - axis;
        }

        public static float ReflectAngle(float angleRadians, Vector2 axis)
        {
            return ReflectAngle(angleRadians, axis.Angle());
        }

        public static Vector2 ClosestPointOnLine(Vector2 lineA, Vector2 lineB, Vector2 closestTo)
        {
            Vector2 v = lineB - lineA;
            Vector2 w = closestTo - lineA;
            float t = Vector2.Dot(w, v) / Vector2.Dot(v, v);
            t = MathHelper.Clamp(t, 0, 1);

            return lineA + v * t;
        }

        public static Vector2 Round(this Vector2 vec)
        {
            return new Vector2((float)Math.Round(vec.X), (float)Math.Round(vec.Y));
        }

        public static float Snap(float value, float increment)
        {
            return (float)Math.Round(value / increment) * increment;
        }

        public static float Snap(float value, float increment, float offset)
        {
            return ((float)Math.Round((value - offset) / increment) * increment) + offset;
        }

        public static float WrapAngleDeg(float angleDegrees)
        {
            return (((angleDegrees * Math.Sign(angleDegrees) + 180) % 360) - 180) * Math.Sign(angleDegrees);
        }

        public static float WrapAngle(float angleRadians)
        {
            return (((angleRadians * Math.Sign(angleRadians) + MathHelper.Pi) % (MathHelper.Pi * 2)) - MathHelper.Pi) * Math.Sign(angleRadians);
        }

        public static Vector2 AngleToVector(float angleRadians, float length)
        {
            return new Vector2((float)Math.Cos(angleRadians) * length, (float)Math.Sin(angleRadians) * length);
        }

        public static float AngleApproach(float val, float target, float maxMove)
        {
            var diff = AngleDiff(val, target);
            if (Math.Abs(diff) < maxMove)
                return target;
            return val + MathHelper.Clamp(diff, -maxMove, maxMove);
        }

        public static float AngleLerp(float startAngle, float endAngle, float percent)
        {
            return startAngle + AngleDiff(startAngle, endAngle) * percent;
        }

        public static float Approach(float val, float target, float maxMove)
        {
            return val > target ? Math.Max(val - maxMove, target) : Math.Min(val + maxMove, target);
        }

        public static float AngleDiff(float radiansA, float radiansB)
        {
            float diff = radiansB - radiansA;

            while (diff > MathHelper.Pi) { diff -= MathHelper.TwoPi; }
            while (diff <= -MathHelper.Pi) { diff += MathHelper.TwoPi; }

            return diff;
        }

        public static float AbsAngleDiff(float radiansA, float radiansB)
        {
            return Math.Abs(AngleDiff(radiansA, radiansB));
        }

        public static int SignAngleDiff(float radiansA, float radiansB)
        {
            return Math.Sign(AngleDiff(radiansA, radiansB));
        }

        public static float Angle(Vector2 from, Vector2 to)
        {
            return (float)Math.Atan2(to.Y - from.Y, to.X - from.X);
        }

        public static Color ToggleColors(Color current, Color a, Color b)
        {
            if (current == a)
                return b;
            else
                return a;
        }

        public static float ShorterAngleDifference(float currentAngle, float angleA, float angleB)
        {
            if (Math.Abs(Calc.AngleDiff(currentAngle, angleA)) < Math.Abs(Calc.AngleDiff(currentAngle, angleB)))
                return angleA;
            else
                return angleB;
        }

        public static float ShorterAngleDifference(float currentAngle, float angleA, float angleB, float angleC)
        {
            if (Math.Abs(Calc.AngleDiff(currentAngle, angleA)) < Math.Abs(Calc.AngleDiff(currentAngle, angleB)))
                return ShorterAngleDifference(currentAngle, angleA, angleC);
            else
                return ShorterAngleDifference(currentAngle, angleB, angleC);
        }

        public static bool IsInRange<T>(this T[] array, int index)
        {
            return index >= 0 && index < array.Length;
        }

        public static bool IsInRange<T>(this List<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }

        public static T[] Array<T>(params T[] items)
        {
            return items;
        }

        public static T[] VerifyLength<T>(this T[] array, int length)
        {
            if (array == null)
                return new T[length];
            else if (array.Length != length)
            {
                T[] newArray = new T[length];
                for (int i = 0; i < Math.Min(length, array.Length); i++)
                    newArray[i] = array[i];
                return newArray;
            }
            else
                return array;
        }

        public static T[][] VerifyLength<T>(this T[][] array, int length0, int length1)
        {
            array = VerifyLength<T[]>(array, length0);
            for (int i = 0; i < array.Length; i++)
                array[i] = VerifyLength<T>(array[i], length1);
            return array;
        }

        public static bool BetweenInterval(float val, float interval)
        {
            return val % (interval * 2) > interval;
        }

        public static bool OnInterval(float val, float prevVal, float interval)
        {
            return (int)(prevVal / interval) != (int)(val / interval);
        }

        #endregion
    }
}
