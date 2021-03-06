﻿using DigitalRuby.Tween;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static partial class SLUtility
{
    public delegate void ExceptionHandle(string Message);

    public static ExceptionHandle OnGlobalException;
    public static bool IsDebug
    {
        get
        {
            return false;
        }
    }

    public static class IO
    {
        public static void Delete(string fileName)
        {
            File.Delete(Path.Combine(Application.persistentDataPath, fileName + ".sl"));
            File.Delete(Path.Combine(Application.persistentDataPath, fileName + ".json"));
        }

        public static T Load<T>(string fileName, FileParsingFlags type = FileParsingFlags.BSON, bool rootAsCollection = false)
        {
            string ext = (type == FileParsingFlags.BSON ? ".sl" : "");
            string fileLocation = Path.Combine(Application.persistentDataPath, fileName + ext);
            T returnObject = default(T);

            switch (type)
            {
                case FileParsingFlags.BSON:
                    {
                        if (File.Exists(fileLocation))
                        {

                            using (FileStream file = File.Open(fileLocation, FileMode.Open))
                            {
                                using (BsonReader reader = new BsonReader(file))
                                {
                                    reader.ReadRootValueAsArray = rootAsCollection;
                                    JsonSerializer serializer = new JsonSerializer();
                                    returnObject = serializer.Deserialize<T>(reader);
                                }
                            }
                        }
                    }
                    break;
                case FileParsingFlags.JSON:
                    {
                        TextAsset data = Resources.Load<TextAsset>(fileName);
                        if (data != null && !String.IsNullOrEmpty(data.text))
                        {
                            returnObject = JsonConvert.DeserializeObject<T>(data.text);
                        }
                    }
                    break;
            }

            return returnObject;
        }

        /// <summary>
        /// Saves the data to a stream, unless type is FlieParsingFlags.RAWN, which forces the save function to return the raw
        /// json data instead of the file path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="SaveData"></param>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string Save<T>(T SaveData, string fileName, FileParsingFlags type = FileParsingFlags.BSON)
        {
            string ext = "";

            switch (type)
            {
                case FileParsingFlags.BSON:
                    ext = ".sl";
                    break;

                case FileParsingFlags.JSON:
                    ext = ".json";
                    break;

                default:
                case FileParsingFlags.RAWN:
                    ext = "";
                    break;
            }

            string fileLocation = Path.Combine(Application.persistentDataPath, fileName + ext);

            if (type == FileParsingFlags.RAWN)
            {
                return JsonConvert.SerializeObject(SaveData, Formatting.Indented);
            }

            using (FileStream file = File.Open(fileLocation, FileMode.Create))
            {
                switch (type)
                {
                    case FileParsingFlags.BSON:
                        using (BsonWriter writer = new BsonWriter(file))
                        {
                            var serializer = new JsonSerializer();
                            serializer.Serialize(writer, SaveData);
                        }
                        break;
                    case FileParsingFlags.JSON:
                        using (StreamWriter writer = new StreamWriter(file))
                        {
                            string json = JsonConvert.SerializeObject(SaveData, Formatting.Indented);
                            writer.Write(json);
                        }
                        break;
                }
            }

            return fileLocation;
        }
    }

    public static class Trans
    {
        public static void ScaleTransformByFactor(Transform transform, float factor, Vector3 baseValues)
        {
            transform.localScale = new Vector3(baseValues.x * Mathf.Clamp(factor, 0f, 1f), baseValues.y, baseValues.z);
        }

        public static void ScaleTransformByXYFactor(Transform transform, float factorX, float factorY, Vector3 baseValues)
        {
            transform.localScale = new Vector3(baseValues.x * Mathf.Clamp(factorX, 0f, 1f), baseValues.y * Mathf.Clamp(factorY, 0f, 1f), baseValues.z);
        }

        public static void MoveRightSideTransformByFactor(RectTransform transform, float factor, Rect baseValues)
        {
            transform.offsetMax = new Vector2(baseValues.x * factor, baseValues.y);
        }

        public static void SetTransformWidthByFactor(RectTransform transform, float percent, float baseWidth)
        {
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, baseWidth * percent);
        }

        public static void SetRectTransformHeightWidth(RectTransform rect, float height, float width)
        {
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

    }

    public static class InputHelper
    {
        private static float _MaxClickTime = 0.40f;
        private static float _MinClickTime = 0.05f;

        private static float _CurrentMinClickTime = 0;
        private static float _CurrentMaxClickTime = 0;

        public static bool DoubleClick()
        {
            if (UnityEngine.Time.time >= _CurrentMinClickTime && UnityEngine.Time.time <= _CurrentMaxClickTime)
            {
                _CurrentMinClickTime = 0;
                _CurrentMaxClickTime = 0;
                return true;
            }

            _CurrentMinClickTime = UnityEngine.Time.time + _MinClickTime; _CurrentMaxClickTime = UnityEngine.Time.time + _MaxClickTime;
            return false;
        }

        /// <summary>
        /// Captures the world position equivalents of any positional inputs (touches, mouse clicks). 
        /// best used in 2D views. Only useable during Update function.
        /// </summary>
        /// <returns>collection of inputs screen to world point</returns>
        public static Vector3[] AnyBeginInputs()
        {
            historyInputs = null;
            List<Vector3> inputs = new List<Vector3>();
            if (Input.touches.Length > 0)
            {
                foreach (Touch finger in Input.touches)
                {
                    if (finger.phase == TouchPhase.Began)
                    {
                        var pos = Camera.main.ScreenToWorldPoint(finger.position);
                        inputs.Add(pos);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                inputs.Add(pos);
            }

            historyInputs = inputs.ToArray();

            return historyInputs;
        }

        static Vector3[] historyInputs = null;
        public static Vector3[] HistoryInputs()
        {
            return historyInputs;
        }
    }

    public static class Tween
    {
        public static IEnumerator FadeOut(CanvasGroup group, float duration)
        {
            if (group)
            {
                float newAlpha = 1;

                var iTween = TweenFactory.Tween(null, 1, 0, duration, TweenScaleFunctions.QuadraticEaseIn,
                    (progress) =>
                    {
                        newAlpha = progress.CurrentValue;
                    });

                while (!iTween.Update(UnityEngine.Time.deltaTime))
                {
                    group.alpha = newAlpha;
                    yield return new WaitForEndOfFrame();
                }

                group.alpha = 0;
            }
        }

        public static IEnumerator FadeIn(CanvasGroup group, float duration)
        {
            if (group)
            {
                float newAlpha = 0;

                var iTween = TweenFactory.Tween(null, 0, 1, duration, TweenScaleFunctions.QuadraticEaseIn,
                    (progress) =>
                    {
                        newAlpha = progress.CurrentValue;
                    });

                while (!iTween.Update(UnityEngine.Time.deltaTime))
                {
                    group.alpha = newAlpha;
                    yield return new WaitForEndOfFrame();
                }

                group.alpha = 1;
            }
        }
    }

    #region public static class Random ...
    public static class Random
    {
        public struct ChanceCase
        {
            public bool success;
            public float roll;

            public static implicit operator bool(ChanceCase chance)
            {
                return chance.success;
            }
            public static implicit operator float(ChanceCase chance)
            {
                return chance.roll;
            }
        }

        /// <summary>
        /// Using random values to simulate a random chance based on percentage success.
        /// </summary>
        /// <param name="percent"> the percentage chance this returns true. value should be [0, 1].</param>
        /// <param name="lowerBound"> the lower bound of the check</param>
        /// <param name="upperBound"> the upper limit to the check</param>
        /// <returns> true if lucky enough to land chance.</returns>
        //public static ChanceCase Chance(float percent, float lowerBound = 0.0f, float upperBound = 1.0f)
        //{
        //    ChanceCase chance = new ChanceCase();
        //    chance.roll = UnityEngine.Random.Range(lowerBound, upperBound);
        //    chance.success = chance.roll <= percent;
        //    return chance;
        //}

        public static ChanceCase Chance(double percent, float lowerBound = 0.0f, float upperBound = 1.0f)
        {
            ChanceCase chance = new ChanceCase();
            chance.roll = UnityEngine.Random.Range(lowerBound, upperBound);
            chance.success = chance.roll <= percent;
            return chance;
        }

        public static ChanceCase FlipCoin()
        {
            return Chance(0.5f);
        }

        internal static Vector3 InRectangle(Rect rect, float distX = 0, float distY = 0)
        {
            Vector2 fullArea = Rect.NormalizedToPoint(rect, (new Vector2(UnityEngine.Random.value, UnityEngine.Random.value)));
            fullArea = Rect.PointToNormalized(rect, new Vector2(fullArea.x > 0 ? fullArea.x + distX : fullArea.x - distX, fullArea.y > 0 ? fullArea.y + distY : fullArea.y - distY));
            return Rect.NormalizedToPoint(rect, fullArea);
        }
    }

    public static T RandomWeightedOne<T>(this IList<T> list, IList<int> weights)
    {

        Dictionary<T, int> map = ((IEnumerable<T>)list).Zip(weights, (k, v) => { return new { k, v }; }).ToDictionary(x => x.k, x => x.v);
        return WeightedRandomizer.From(map).TakeOne();
    }

    public static T RandomWeigthedOne<T>(this Dictionary<T, int> map)
    {
        return WeightedRandomizer.From(map).TakeOne();
    }

    /// <summary>
    /// Randomizes the contents of the container. does not alter dimensions. modifies original list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static List<T> Shuffle<T>(this List<T> list)
    {
        int count = list.Count;

        while (count > 1)
        {
            --count;
            int rIndex = UnityEngine.Random.Range(0, count + 1);
            T val = list[rIndex];
            list[rIndex] = list[count];
            list[count] = val;
        }

        return list;
    }

    public static T RandomOne<T>(this IList<T> list)
    {
        if (list.Count <= 0) return default(T);
        int index = UnityEngine.Random.Range(0, list.Count());
        return list[index];
    }

    /// <summary>
    /// Randomly selects specified number of items from lists, does not select duplicates from list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static IEnumerable<T> RandomMany<T>(this IList<T> list, int count)
    {
        if (list.Count <= 0) return default(IEnumerable<T>);
        return new List<T>(list).Shuffle().Take(count);
    }

    public static Vector3 ToVector3(this Vector2 me, float zComp = 0)
    {
        return new Vector3(me.x, me.y, zComp);
    }

    public static Vector2 ToVector2(this Vector3 me)
    {
        return new Vector2(me.x, me.y);
    }

    /// <summary>
    /// Randomizes the contents of the container. does not alter dimensions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T[] Shuffle<T>(this T[] list)
    {
        int count = list.Length;

        while (count > 1)
        {
            --count;
            int rIndex = UnityEngine.Random.Range(0, count + 1);
            T val = list[rIndex];
            list[rIndex] = list[count];
            list[count] = val;
        }

        return list;
    }
    #endregion

    public static int CyclicModulo(int value, int shift, int total)
    {
        int m = total;
        int a = value + (shift % total);

        return (a + m) % m;
    }

    public static float CyclicModulo(float value, float shift, float total)
    {
        float m = total;
        float a = value + shift;

        return a - m * Mathf.Floor(a / m);
    }

    public static class Time
    {
        public static long SecondsToTicks(float seconds)
        {
            return (long)Math.Floor(seconds * 10000000);
        }

        public static string GeneralTimeFormat(TimeSpan time, bool milliseconds = true)
        {
            if (!milliseconds) return string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);

            return string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D2}", time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
        }

        public static TimeSpan CountDownTime(DateTime value, DateTime now, TimeSpan delay)
        {
            TimeSpan clampTime = delay - (now - value);
            return clampTime > TimeSpan.Zero ? clampTime : TimeSpan.Zero;
        }
    }

    public static void Log(string message)
    {
        Debug.Log($"{DateTime.Now.ToString()}:{message}");
    }
}
