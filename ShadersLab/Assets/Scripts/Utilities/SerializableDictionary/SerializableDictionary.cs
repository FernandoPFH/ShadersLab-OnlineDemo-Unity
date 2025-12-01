using System;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
[CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
public class SerializableDictionaryDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement container = new();
        SerializedProperty items = property.FindPropertyRelative("items");
        container.Add(new PropertyField(items) { label = property.displayName });
        return container;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty items = property.FindPropertyRelative("items");
        EditorGUI.PropertyField(position, items, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty items = property.FindPropertyRelative("items");
        return EditorGUI.GetPropertyHeight(items, label);
    }
}
#endif

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<SerializableKeyValuePair> items = new();

    public void OnBeforeSerialize()
    {
        items.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
            items.Add(pair);
    }

    public void OnAfterDeserialize()
    {
        Clear();
        for (int i = 0; i < items.Count; i++)
            if (!ContainsKey(items[i].Key))
                this[items[i].Key] = items[i].Value;
            else if (items.Count >= 2 && i == items.Count - 1 && items[i].Key.Equals(items[i - 1].Key))
            {
                TKey newKey = default;
                newKey ??= typeof(TKey).Name switch
                {
                    "String" => (TKey)(object)string.Empty,
                    _ => Activator.CreateInstance<TKey>(),
                };
                this[newKey] = default;
            }
    }

    [Serializable]
    public struct SerializableKeyValuePair
    {
        public TKey Key;
        public TValue Value;

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public static implicit operator SerializableKeyValuePair(KeyValuePair<TKey, TValue> pair) => new(pair.Key, pair.Value);

        public static implicit operator KeyValuePair<TKey, TValue>(SerializableKeyValuePair pair) => new(pair.Key, pair.Value);
    }
}
