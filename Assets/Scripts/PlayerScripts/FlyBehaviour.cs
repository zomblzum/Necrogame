using UnityEngine;

// Класс для полета, удобен для тестирования и дебага, ну и просто прикольно)
/// <summary>
/// Полет персонажа
/// </summary>
public class FlyBehaviour : GenericBehaviour
{
	[Header("Скорость полета")] public float flySpeed = 4.0f;                 
	[Header("Скорость ускоренного полета")] public float sprintFactor = 2.0f;             
	[Header("Лимит поворота камеры при полете")] public float flyMaxVerticalAngle = 60f;       

	private string flyButton = "Fly";
	private int flyBool;                          
	private bool fly = false;                    
	private CapsuleCollider col;                  

	void Start()
	{
		flyBool = Animator.StringToHash("Fly");
		col = this.GetComponent<CapsuleCollider>();
		behaviourManager.SubscribeBehaviour(this);
	}

	void Update()
	{
		// Обработка нажатия кнопки полета
		if (Input.GetButtonDown(flyButton) && !behaviourManager.IsOverriding() 
			&& !behaviourManager.GetTempLockStatus(behaviourManager.GetDefaultBehaviour))
		{
			fly = !fly;

			// Завершаем прыжок, если был
			behaviourManager.UnlockTempBehaviour(behaviourManager.GetDefaultBehaviour);

			// Устанавливаем гравитацию в зависимости от состояния полета
			behaviourManager.GetRigidBody.useGravity = !fly;

			// Летим
			if (fly)
			{
				behaviourManager.RegisterBehaviour(this.behaviourCode);
			}
			// Не летим
			else
			{
				col.direction = 1;
				behaviourManager.GetCamScript.ResetTargetOffsets();
				behaviourManager.UnregisterBehaviour(this.behaviourCode);
			}
		}

		fly = fly && behaviourManager.IsCurrentBehaviour(this.behaviourCode);
		behaviourManager.GetAnim.SetBool(flyBool, fly);
	}

	public override void OnOverride()
	{
		col.direction = 1;
	}

	public override void LocalFixedUpdate()
	{
		behaviourManager.GetCamScript.SetMaxVerticalAngle(flyMaxVerticalAngle);
		FlyManagement(behaviourManager.GetH, behaviourManager.GetV);
	}
	
	/// <summary>
	/// Управление персонажем в полете
	/// </summary>
	void FlyManagement(float horizontal, float vertical)
	{
		Vector3 direction = Rotating(horizontal, vertical);
		behaviourManager.GetRigidBody.AddForce((direction * flySpeed * 100 * (behaviourManager.IsSprinting() ? sprintFactor : 1)), ForceMode.Acceleration);
	}

	/// <summary>
	/// Поворот персонажа в полете
	/// </summary>
	Vector3 Rotating(float horizontal, float vertical)
	{
		Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);
		forward = forward.normalized;

		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		Vector3 targetDirection = forward * vertical + right * horizontal;

		// Плавно крутим
		if ((behaviourManager.IsMoving() && targetDirection != Vector3.zero))
		{
			Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

			Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, behaviourManager.turnSmoothing);

			behaviourManager.GetRigidBody.MoveRotation(newRotation);
			behaviourManager.SetLastDirection(targetDirection);
		}

		// Персонаж парит в воздухе
		if (!(Mathf.Abs(horizontal) > 0.2 || Mathf.Abs(vertical) > 0.2))
		{
			behaviourManager.Repositioning();
			col.direction = 1;
		}
		// Движется
		else
		{
			col.direction = 2;
		}

		return targetDirection;
	}
}
