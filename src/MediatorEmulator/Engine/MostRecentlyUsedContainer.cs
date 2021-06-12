using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.MediatorEmulator.Engine
{
    /// <summary>
    /// A case-insensitive list of most-recently-used texts.
    /// </summary>
    public sealed class MostRecentlyUsedContainer
    {
        private readonly bool ignoreCase;
        private readonly List<string> mruList = new();

        public IReadOnlyCollection<string> Items => mruList.AsReadOnly();

        public MostRecentlyUsedContainer(bool ignoreCase = true)
        {
            this.ignoreCase = ignoreCase;
        }

        public void Import(IEnumerable<string?> items)
        {
            Guard.NotNull(items, nameof(items));

            IEnumerable<string> itemsReversed = items.Reverse().Where(item => !string.IsNullOrWhiteSpace(item)).Cast<string>();

            foreach (string item in itemsReversed)
            {
                MarkAsUsed(item);
            }
        }

        /// <summary>
        /// Updates the MRU list with the specified text. If the MRU list already contains this text, it is moved to the top of the list (preserving existing
        /// case).
        /// </summary>
        public void MarkAsUsed(string text)
        {
            Guard.NotNullNorEmpty(text, nameof(text));

            if (text.Length > 0)
            {
                int existingIndex = GetExistingIndex(text);

                if (existingIndex == -1)
                {
                    mruList.Insert(0, text);
                }
                else
                {
                    string originalText = mruList[existingIndex];
                    mruList.RemoveAt(existingIndex);
                    mruList.Insert(0, originalText);
                }
            }
        }

        public void Remove(string path)
        {
            Guard.NotNullNorEmpty(path, nameof(path));

            int index = GetExistingIndex(path);

            if (index != -1)
            {
                mruList.RemoveAt(index);
            }
        }

        private int GetExistingIndex(string text)
        {
            for (int itemIndex = 0; itemIndex < mruList.Count; itemIndex++)
            {
                if (string.Compare(text, mruList[itemIndex], ignoreCase, CultureInfo.InvariantCulture) == 0)
                {
                    return itemIndex;
                }
            }

            return -1;
        }
    }
}
