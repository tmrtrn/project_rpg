using System;
using System.IO;
using UnityEngine;

namespace Utils
{
    public static class TextHelper
    {
        public static TextReader OpenFile(string filename)
        {
            TextReader textReader = TryOpenFile(filename);
            if (textReader == null)
            {
                throw new Exception("Text Asset didn't load properly! " + filename);
            }
            return textReader;
        }
        private static TextReader TryOpenFile(string filename)
        {
            TextAsset textAsset = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
            return (!(textAsset != null)) ? null : new StringReader(textAsset.text);
        }
    }
}