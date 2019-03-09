using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SH_ColliderDelegate)), CanEditMultipleObjects]
public class SH_ColliderDelegateEditor : Editor {

    public SerializedProperty
        interactionType_Prop,
        colliderTag_Prop,
        colliderCondition_Prop,
        condition_Prop,
        invokeDelay_Prop,
        onEnter_Prop,
        onExit_Prop;

    private void OnEnable()
    {
        interactionType_Prop = serializedObject.FindProperty("interactionType");
        colliderTag_Prop = serializedObject.FindProperty("colliderTag");
        colliderCondition_Prop = serializedObject.FindProperty("colliderCondition");
        condition_Prop = serializedObject.FindProperty("condition");
        invokeDelay_Prop = serializedObject.FindProperty("invokeDelay");
        onEnter_Prop = serializedObject.FindProperty("onEnter");
        onExit_Prop = serializedObject.FindProperty("onExit");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(interactionType_Prop);

        SH_ColliderDelegate.InteractionType interactionType = (SH_ColliderDelegate.InteractionType)interactionType_Prop.enumValueIndex;
        if (interactionType == SH_ColliderDelegate.InteractionType.Script)
        {
            EditorGUILayout.PropertyField(colliderTag_Prop);
        }

        EditorGUILayout.PropertyField(colliderCondition_Prop);
        SH_ColliderDelegate.ColliderCondition colliderCondition = (SH_ColliderDelegate.ColliderCondition)colliderCondition_Prop.enumValueIndex;
        GUIContent conditionName;
        switch (colliderCondition)
        {
            case SH_ColliderDelegate.ColliderCondition.Name:
                conditionName = new GUIContent("Name:");
                EditorGUILayout.PropertyField(condition_Prop, conditionName);
                break;
            case SH_ColliderDelegate.ColliderCondition.Tag:
                conditionName = new GUIContent("Tag:");
                EditorGUILayout.PropertyField(condition_Prop, conditionName);
                break;
        }

        EditorGUILayout.PropertyField(invokeDelay_Prop);
        EditorGUILayout.PropertyField(onEnter_Prop);
        EditorGUILayout.PropertyField(onExit_Prop);

        serializedObject.ApplyModifiedProperties();
    }
}
