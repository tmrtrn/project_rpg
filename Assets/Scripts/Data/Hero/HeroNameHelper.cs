using System;
using System.IO;
using System.Text.RegularExpressions;
using Utils;
using Random = UnityEngine.Random;

namespace Data.Hero
{
    /// <summary>
    /// super hero names https://gist.github.com/rpbaltazar/bdc04c868d208c8be38e850c0faf92d1
    /// loads all hero names initially
    /// </summary>
    public static class HeroNameHelper
    {
        private static MatchCollection heroMatches;

        static HeroNameHelper()
        {
            TextReader reader = TextHelper.OpenFile("GameAssets/HeroNames");
            string input = reader.ReadToEnd();
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }
            heroMatches = Regex.Matches(input, "\\\"(.*?)\\\"");
        }

        /// <summary>
        /// Picks a random name from the collection
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ReadRandomHeroName()
        {
            if (heroMatches == null || heroMatches.Count == 0)
            {
                throw new Exception("Failed to cache hero names");
            }
            int randIndex = Random.Range(0, heroMatches.Count);
            return heroMatches[randIndex].Value.Split("\"", StringSplitOptions.RemoveEmptyEntries)[0];
        }
    }
}