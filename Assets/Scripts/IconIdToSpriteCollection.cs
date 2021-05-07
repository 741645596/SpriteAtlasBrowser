using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkyGame.Foundation.UI
{
    [CreateAssetMenu(fileName = "IconCollection", menuName = "Icon/Icon")]
    public class IconIdToSpriteCollection : ScriptableObject
    {
        [Serializable]
        public class IconData
        {
            public int id;
            public Sprite sprite;
        }

        [SerializeField]
        protected List<IconData> IconList = new List<IconData>();

        private Dictionary<int, IconData> _iconMap;

        public int Count => IconList.Count;

        public Sprite GetSprite(int iconId)
        {
            if (_iconMap == null) BuildMap();

            if (_iconMap.TryGetValue(iconId, out IconData data))
            {
                return data.sprite;
            }

            return null;
        }

        public void Clear()
        {
            IconList.Clear();
            _iconMap?.Clear();
        }

        public void Reset()
        {
            IconList.Clear();
            _iconMap?.Clear();
        }

        public void Add(IconData data)
        {
            IconList.Add(data);
        }

        protected void BuildMap()
        {
            if (_iconMap == null)
            {
                _iconMap = new Dictionary<int, IconData>();
            }
            else
            {
                _iconMap.Clear();
            }

            for (var i = 0; i < IconList.Count; i++)
            {
                var icon = IconList[i];
                _iconMap.Add(icon.id, icon);
            }
        }

        public List<Sprite> GetAllSprites()
        {
            List<Sprite> res = new List<Sprite>();
            foreach (var data in IconList)
            {
                if (data.sprite)
                {
                    res.Add(data.sprite);
                }
            }

            return res;
        }
    }
}