using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyAILooker))]
public class EnemyAILookerEditor : Editor {

    private void OnSceneGUI() {
        EnemyAILooker enemyAILooker = (EnemyAILooker)target;

        Handles.color = Color.green; 
        Handles.DrawWireArc(enemyAILooker.transform.position, Vector3.up, 
        enemyAILooker.transform.forward, 360, enemyAILooker.viewRadius);

        Vector3 viewAngleA = enemyAILooker.DirFromAngle(enemyAILooker.viewAngle / 2, false);
        Vector3 viewAngleB = enemyAILooker.DirFromAngle(-enemyAILooker.viewAngle / 2, false);

        Handles.DrawLine(enemyAILooker.transform.position, 
        enemyAILooker.transform.position + viewAngleA * enemyAILooker.viewRadius);
        Handles.DrawLine(enemyAILooker.transform.position, 
        enemyAILooker.transform.position + viewAngleB * enemyAILooker.viewRadius);

        if (enemyAILooker.player == null) return;

        Handles.color = Color.red; 
        Handles.DrawLine(enemyAILooker.transform.position, enemyAILooker.player.position);
    }

}
