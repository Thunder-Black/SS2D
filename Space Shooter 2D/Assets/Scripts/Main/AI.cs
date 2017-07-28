using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class AI : MonoBehaviour
{
    public float visionRange;
    public float minWaypointRad;
    public Transform[] waypointTr;

    public float pathLength { get; private set; }
    private int index = 0;
    private float sign;
    Vector2 myPos;
    Vector2 upPos;
    Vector2 wayPos;
    UnitController uC;
    List<UnitController> uCs = new List<UnitController>();

    Grid grid;
    PathFinding pf;

    void Start()
    {

        uC = GetComponent<UnitController>();
        grid = GetComponent<Grid>();
        pf = GetComponent<PathFinding>();

        GameObject[] units = GameObject.FindGameObjectsWithTag("Vehicle");
        for (int i = 0; i < units.Length; i++)
            uCs.Add(units[i].GetComponent<UnitController>());
        uCs.Remove(uC);
    }
    private void MovingOnWaypoints()
    {
        float angle = 0f;
        // Собираем значения
        if (index < waypointTr.Length)
            angle = GetAngleTo(waypointTr[index].position);

        // Скрипт существует - двигаемся
        // Логика перехода к след. точке
        if (pathLength < minWaypointRad)
        {
            index++;
            // Цикл
            /*if (waypointIndex >= pathFinding.waypointTr.Length)
                waypointIndex = 0;*/
        }

        if (index <= waypointTr.Length)
            MoveOnAngle(angle);
    }
    private void MovingTo(Vector3 position)
    {
        if (minWaypointRad < GetDistanceTo(position))
        {
            float angle = GetAngleTo(position);
            // Moving
            MoveOnAngle(angle);
        }
    }
    private void MovingTowardTo(Vector3 position)
    {
        if (minWaypointRad < GetDistanceTo(position))
        {
            float angle = GetAngleTo(position);
            // Moving
            MoveTowardOnAngle(angle);
        }
    }
    private void MoveOnAngle(float angle)
    {
        // Moving
        if (angle < -uC.rotatingSpeed / 50 || angle > uC.rotatingSpeed / 50)
            RotateTo(angle);
        else
            uC.MoveForwBack(1);
    }
    private void MoveTowardOnAngle(float angle)
    {
        if (angle < 180 - uC.rotatingSpeed / 50 && angle > -180 + uC.rotatingSpeed / 50)
            RotateTo(angle - 180);
        else
            uC.MoveForwBack(-1);
    }
    private void RotateTo(float angle)
    {
        if (angle < -180)
            angle += 360;
        if (angle > uC.rotatingSpeed / 50)
            uC.Rotation(1);
        else uC.Rotation(-1);
    }
    private float GetDistanceTo(Vector3 position)
    {
        return (this.transform.position - position).sqrMagnitude;
    }
    private float GetAngleTo(Vector3 position)
    {
        float angle = 0f;
        myPos = new Vector2(this.transform.position.x, this.transform.position.y);
        upPos = new Vector2(this.transform.position.x + this.transform.up.x, this.transform.position.y + this.transform.up.y);
        wayPos = new Vector2(position.x, position.y);

        // Надо убрать
        pathLength = (myPos - wayPos).sqrMagnitude;

        sign = (wayPos - myPos).x * (upPos - myPos).y - (wayPos - myPos).y * (upPos - myPos).x;
        angle = Vector2.Angle(wayPos - myPos, upPos - myPos);
        if (sign < 0)
            angle = 0 - angle;
        return angle;
    }
    private UnitController FindTarget()
    {
        // Простой скрипт - первый попавшийся не своей фракции
        for (int i = 0; i < uCs.Count; i++)
            if (uCs[i] != null && uCs[i].fraction != uC.fraction && GetDistanceTo(uCs[i].transform.position) < visionRange)
                return uCs[i];

        return null;
    }
    void FixedUpdate()
    {
        // Логика

        // Находим цель
        UnitController target = FindTarget();
        if (target != null)
        {
            pf.target = target.transform;
            float angle = GetAngleTo(target.transform.position);
            if (GetDistanceTo(target.transform.position) < 25)
            {
                // Если расстояние меньше 25 - Стреляем
                RotateTo(angle);
                if (GetAngleTo(target.transform.position) < uC.rotatingSpeed / 50)
                {
                    pf.postUnwalkableNodes.Clear();
                    pf.rememberNode.Clear();
                    uC.Shoot();
                }
            }
            // Иначе - догоняем
            else if (grid.path != null && grid.path.Count > 1)
            {
                MovingTo(grid.path[1].worldPosition);
                if (GetDistanceTo(grid.path[1].worldPosition) < minWaypointRad)
                {
                    grid.path.RemoveAt(0);
                }
            }
            // MovingOnWaypoints();
        }
        // Если цели нет - задом двигаемся на базу
        else
        {
            pf.target = waypointTr[0];
            if (grid.path != null && grid.path.Count > 1)
            {
                MovingTo(grid.path[1].worldPosition);
                if (GetDistanceTo(grid.path[1].worldPosition) < minWaypointRad)
                {
                    grid.path.RemoveAt(0);
                }
            }
        }
    }
}
