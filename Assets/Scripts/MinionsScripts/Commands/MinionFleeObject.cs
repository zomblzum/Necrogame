using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionFleeObject : FleeObject
{
    private bool inViewField = true;
    private Minion minion;
    private Camera mainCamera;
    private Plane[] planes;

    public override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
    }

    private void Update()
    {
        if (minion)
        {
            transform.position = minion.transform.position;
            //targets.RemoveAll(item => item == null);

            planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
            if (GeometryUtility.TestPlanesAABB(planes, minion.GetComponent<Collider>().bounds))
            {
                inViewField = true;
            }
            else
            {
                inViewField = false;
            }
        }
    }

    public void SetMinion(Minion minion)
    {
        this.minion = minion;
    }

    public override bool CorrectTarget(GameObject target)
    {
        return target.GetComponent<Enemy>()
            || (target.GetComponent<ZombieMinion>() && !target.GetComponent<Minion>().underControl);
    }

    public override Vector3 GetFleeCoordinates()
    {
        if (inViewField)
        {
            //Находим камеру
            ThirdPersonOrbitCamBasic thirdPersonOrbitCam = mainCamera.GetComponent<ThirdPersonOrbitCamBasic>();
            Vector3 cameraPosition = mainCamera.transform.position;
            //Определяем растояние до миньоны
            float dist = Vector3.Distance(transform.position, cameraPosition);
            //Считаем координату для бега
            Vector3 leftFleeCoordinates = cameraPosition + ((thirdPersonOrbitCam.leftBorder.position - cameraPosition) * dist/2);
            Vector3 rightFleeCoordinates = cameraPosition + ((thirdPersonOrbitCam.rightBorder.position - cameraPosition) * dist/2);

            //Если выйти через левую границу ближе и туда можно пройти
            if(Vector3.Distance(minion.transform.position, leftFleeCoordinates) <= Vector3.Distance(minion.transform.position, rightFleeCoordinates)
                && NavMesh.CalculatePath(transform.position, leftFleeCoordinates, NavMesh.AllAreas, new NavMeshPath()))
            {
                return new Vector3(leftFleeCoordinates.x, minion.transform.position.y, leftFleeCoordinates.z);
            }
            // (Правый путь ближе или Левый недоступен) и можно пройти направо
            else if (NavMesh.CalculatePath(transform.position, rightFleeCoordinates, NavMesh.AllAreas, new NavMeshPath()))
            {
                return new Vector3(rightFleeCoordinates.x, minion.transform.position.y, rightFleeCoordinates.z);
            }
            // обе стороны недоступны, отходим за камеру
            else
            {
                return cameraPosition + Vector3.back * 2;
            }
        } 
        else
        {
            return base.GetFleeCoordinates();
        }
    }

    public bool NeedToFlee()
    {
        return TargetsCount() > 0 || inViewField;
    }
}
