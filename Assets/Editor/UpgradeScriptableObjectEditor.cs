using PickleClicker.Data.ScriptableObjects.Upgrade;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PickleClicker.Data
{
    [System.Serializable]
    [CustomEditor(typeof(UpgradeScriptableObject))]
    public class UpgradeScriptableObjectEditor : Editor 
    {
        UpgradeScriptableObject upgradeScriptableObject;

        SerializedProperty enableEditingProperty;

        SerializedProperty upgradeCategoryIndexProperty;
        SerializedProperty upgradeIndexProperty;
        SerializedProperty upgradeAliasProperty;
        SerializedProperty upgradeDescriptionProperty;
        SerializedProperty upgradePurchaseCostProperty;
        SerializedProperty upgradeCurrentAmountProperty;
        SerializedProperty upgradeMaxAmountProperty;

        private List<UpgradeCategory> upgradeCategories = new List<UpgradeCategory> 
        {
            new UpgradeCategory {
                alias = "Click",
                description = "A collection of ways to upgrade your clicking power.",
                upgrades = {
                    new UpgradePickle {
                        alias = "Pickly Hands",
                        description = "Increases the amount of pickles gained every click.",
                        purchaseCost = 500
                    },
                    new UpgradePickle {
                        alias = "Juicy Pickles",
                        description = "Increases the amount of experience gained every click.",
                        purchaseCost = 500
                    },
                    new UpgradePickle {
                        alias = "Sharper Spear",
                        description = "Increases damage against Poglins.",
                        purchaseCost = 750
                    },
                    new UpgradePickle {
                        alias = "Empowered Club",
                        description = "Increases the amount of critical damage against Poglins.",
                        purchaseCost = 1000
                    },
                    new UpgradePickle {
                        alias = "Anticipated Strike",
                        description = "Increases your chances of landing a critical strike.",
                        purchaseCost = 1000
                    },
                    new UpgradePickle {
                        alias = "Iron Pointed Spear",
                        description = "Normal damage against Earth Poglin shields. Doesn't apply critical damage.",
                        purchaseCost = 5000
                    },
                }
            },
            new UpgradeCategory {
                alias = "Jar",
                description = "A collection of ways to upgrade your pickle jar.",
                upgrades = {
                    new UpgradePickle {
                        alias = "Random Jars",
                        description = "Increases the chance for a pickle jar to spawn.",
                        purchaseCost = 1500
                    },
                    new UpgradePickle {
                        alias = "Juicier Jar",
                        description = "Gives more experience from breaking pickle jars.",
                        purchaseCost = 1500
                    },
                    new UpgradePickle {
                        alias = "Girthier Vessel",
                        description = "Gives more pickles and coins from breaking pickle jars.",
                        purchaseCost = 3000
                    },
                }
            },
            new UpgradeCategory {
                alias = "Poglin",
                description = "A collection of ways to upgrade your poglin hunting.",
                upgrades = {
                    new UpgradePickle {
                        alias = "Tough Skinned Pickle",
                        description = "Decreases the amount of pickles stolen from Poglins.",
                        purchaseCost = 1500
                    },
                    new UpgradePickle {
                        alias = "Shinier Poglins",
                        description = "Increases chances of sighting both pink and gold Poglins.",
                        purchaseCost = 1500
                    },
                    new UpgradePickle {
                        alias = "Extra Leftovers",
                        description = "Increases pickles given back from collecting Poglins.",
                        purchaseCost = 1500
                    },
                    new UpgradePickle {
                        alias = "Bounty Hunter",
                        description = "Increases experience gained from collecting Poglins.",
                        purchaseCost = 1500
                    },
                }
            },
            new UpgradeCategory {
                alias = "Bomb",
                description = "A collection of ways to upgrade your pickle bombs.",
                upgrades = {
                    new UpgradePickle {
                        alias = "Condensed Gunpowder",
                        description = "Increases the bomb's damage.",
                        purchaseCost = 500
                    },
                    new UpgradePickle {
                        alias = "Pungent Scent",
                        description = "Increases the bomb's detection radius.",
                        purchaseCost = 500
                    },
                    new UpgradePickle {
                        alias = "Extra Artillery",
                        description = "Increases bomb count.",
                        purchaseCost = 500
                    },
                }
            },
        };

        private void OnEnable() 
        {
            upgradeCategoryIndexProperty = serializedObject.FindProperty("categoryID");
            upgradeIndexProperty = serializedObject.FindProperty("id");
            upgradeAliasProperty = serializedObject.FindProperty("alias");
            upgradeDescriptionProperty = serializedObject.FindProperty("description");
            upgradePurchaseCostProperty = serializedObject.FindProperty("purchaseCost");
            upgradeCurrentAmountProperty = serializedObject.FindProperty("currentAmount");
            upgradeMaxAmountProperty = serializedObject.FindProperty("maxAmount");
            enableEditingProperty = serializedObject.FindProperty("enableEditing");
            upgradeScriptableObject = (UpgradeScriptableObject) target;  
        }

        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            enableEditingProperty.boolValue = EditorGUILayout.Toggle(new GUIContent("Enable Editing"), enableEditingProperty.boolValue);
            
            upgradeCategoryIndexProperty.intValue = EditorGUILayout.IntSlider(new GUIContent("Category ID"), upgradeCategoryIndexProperty.intValue, 0, upgradeCategories.Count - 1);
            UpgradeCategory upgradeCategory = upgradeCategories[upgradeCategoryIndexProperty.intValue];
            EditorGUILayout.TextField(new GUIContent("Category Alias"), upgradeCategory.alias);

            upgradeIndexProperty.intValue = EditorGUILayout.IntSlider(new GUIContent("ID"), upgradeIndexProperty.intValue, 0, upgradeCategory.upgrades.Count - 1);


            if (!enableEditingProperty.boolValue)
            {
                upgradeAliasProperty.stringValue = upgradeCategory.upgrades[upgradeIndexProperty.intValue].alias;
                upgradeDescriptionProperty.stringValue = upgradeCategory.upgrades[upgradeIndexProperty.intValue].description;
                upgradePurchaseCostProperty.intValue = upgradeCategory.upgrades[upgradeIndexProperty.intValue].purchaseCost;
                upgradeCurrentAmountProperty.intValue = upgradeCategory.upgrades[upgradeIndexProperty.intValue].currentAmount;
                upgradeMaxAmountProperty.intValue = upgradeCategory.upgrades[upgradeIndexProperty.intValue].maxAmount;
            }
            

            upgradeAliasProperty.stringValue = EditorGUILayout.TextField(new GUIContent("Alias"), upgradeAliasProperty.stringValue);
            GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea);
            EditorGUILayout.PrefixLabel(new GUIContent("Description"));
            upgradeDescriptionProperty.stringValue = EditorGUILayout.TextArea(upgradeDescriptionProperty.stringValue, textAreaStyle);
            upgradePurchaseCostProperty.intValue = EditorGUILayout.IntField(new GUIContent("Purchase Cost"), upgradePurchaseCostProperty.intValue);
            upgradeCurrentAmountProperty.intValue = EditorGUILayout.IntField(new GUIContent("Current Amount"), upgradeCurrentAmountProperty.intValue);
            upgradeMaxAmountProperty.intValue = EditorGUILayout.IntField(new GUIContent("Max Amount"), upgradeMaxAmountProperty.intValue);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
