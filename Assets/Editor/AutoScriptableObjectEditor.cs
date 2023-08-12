using PickleClicker.Data.ScriptableObjects.Auto;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PickleClicker.Data
{
    [System.Serializable]
    [CustomEditor(typeof(AutoScriptableObject))]
    public class AutoScriptableObjectEditor : Editor 
    {
        AutoScriptableObject autoScriptableObject;

        SerializedProperty enableEditingProperty;

        SerializedProperty autoIndexProperty;
        SerializedProperty autoAliasProperty;
        SerializedProperty autoDescriptionProperty;
        SerializedProperty autoPurchaseCostProperty;
        SerializedProperty autoUpgradeCostProperty;
        SerializedProperty autoRecieveProperty;

        private List<AutoPickle> autos = new List<AutoPickle>
        {
            new AutoPickle { 
                alias = "Glove", 
                description = "Today is one of the many great days of harvest at the pickle farm. You get ready in worn out attire and put on the musky gloves that reek of pickles. You then begin pick the pickles into your basket and soon after finish up in the afternoon, walking back home.", 
                purchaseCost = 50, 
                upgradeCost = 1000, 
                recieve = 1
            },
            new AutoPickle { 
                alias = "Jar", 
                description = "Heading back inside your home, you greet your loyal companion, Ustayak, an abandoned Poglin you've found eating your crops. Nowadays they assist in selling quality juicy moist pickles with their great sense of smell. Afterwards you go into the storeroom to confine today's harvest in pickle jars. Tired you proceed to your bedroom.", 
                purchaseCost = 100, 
                upgradeCost = 1500, 
                recieve = 2
            },
            new AutoPickle { 
                alias = "Book", 
                description = "You sit at your desk and pull out a rugged book given by your late grandmother. With contains fable of an artifact that grants the user the ability to create the most moist and juiciest of pickles. Flipping through a piece of stained, folded up paper falls out. You unfold to reveal to what seems to be a map. Too tired to investigate any further you decide to hit the hay.", 
                purchaseCost = 250, 
                upgradeCost = 2500, 
                recieve = 3
            },
            new AutoPickle { 
                alias = "Tavern", 
                description = "No Description", 
                purchaseCost = 750, 
                upgradeCost = 5000, 
                recieve = 4
            },
            new AutoPickle { 
                alias = "Forest", 
                description = "No Description", 
                purchaseCost = 1500, 
                upgradeCost = 7500, 
                recieve = 5
            },
            new AutoPickle { 
                alias = "Beast", 
                description = "No Description", 
                purchaseCost = 3000, 
                upgradeCost = 15000, 
                recieve = 6
            },
            new AutoPickle { 
                alias = "Cave", 
                description = "No Description", 
                purchaseCost = 7500, 
                upgradeCost = 25000, 
                recieve = 7
            },
            new AutoPickle { 
                alias = "Dungeon", 
                description = "No Description", 
                purchaseCost = 17500, 
                upgradeCost = 50000, 
                recieve = 8
            },
            new AutoPickle { 
                alias = "Town", 
                description = "No Description", 
                purchaseCost = 25000, 
                upgradeCost = 75000, 
                recieve = 9
            },
            new AutoPickle { 
                alias = "Kingdom", 
                description = "No Description", 
                purchaseCost = 50000, 
                upgradeCost = 100000, 
                recieve = 10
            },
        };

        private void OnEnable() 
        {
            autoIndexProperty = serializedObject.FindProperty("id");
            autoAliasProperty = serializedObject.FindProperty("alias");
            autoDescriptionProperty = serializedObject.FindProperty("description");
            autoPurchaseCostProperty = serializedObject.FindProperty("purchaseCost");
            autoUpgradeCostProperty = serializedObject.FindProperty("upgradeCost");
            autoRecieveProperty = serializedObject.FindProperty("recieve");
            enableEditingProperty = serializedObject.FindProperty("enableEditing");
            autoScriptableObject = (AutoScriptableObject) target;  
        }
        
        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            enableEditingProperty.boolValue = EditorGUILayout.Toggle(new GUIContent("Enable Editing"), enableEditingProperty.boolValue);

            autoIndexProperty.intValue = EditorGUILayout.IntSlider(new GUIContent("ID"), autoIndexProperty.intValue, 0, autos.Count - 1);

            if (!enableEditingProperty.boolValue)
            {
                autoAliasProperty.stringValue = autos[autoIndexProperty.intValue].alias; 
                autoDescriptionProperty.stringValue = autos[autoIndexProperty.intValue].description; 
                autoPurchaseCostProperty.intValue = autos[autoIndexProperty.intValue].purchaseCost;
                autoUpgradeCostProperty.intValue = autos[autoIndexProperty.intValue].upgradeCost;
                autoRecieveProperty.intValue = autos[autoIndexProperty.intValue].recieve;
            }

            autoAliasProperty.stringValue = EditorGUILayout.TextField(new GUIContent("Alias"), autoAliasProperty.stringValue);
            GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea);
            EditorGUILayout.PrefixLabel(new GUIContent("Description"));
            autoDescriptionProperty.stringValue = EditorGUILayout.TextArea(autoDescriptionProperty.stringValue, textAreaStyle);
            autoPurchaseCostProperty.intValue = EditorGUILayout.IntField(new GUIContent("Purchase Cost"), autoPurchaseCostProperty.intValue);
            autoUpgradeCostProperty.intValue = EditorGUILayout.IntField(new GUIContent("Upgrade Cost"), autoUpgradeCostProperty.intValue);
            autoRecieveProperty.intValue = EditorGUILayout.IntField(new GUIContent("Recieve"), autoRecieveProperty.intValue);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

