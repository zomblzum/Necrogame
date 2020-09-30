using UnityEngine;

/// <summary>
/// Перемещение персонажа
/// </summary>
public class MoveBehaviour : GenericBehaviour
{
	[Header("Скорость ходьбы")] public float walkSpeed = 0.15f;                 
	[Header("Скорость бега")] public float runSpeed = 1.0f;                   
	[Header("Скорость быстрого бега")] public float sprintSpeed = 2.0f;                
	[Header("Время перехода с бега на шаг")] public float speedDampTime = 0.1f;              
	[Header("Высота прыжка")] public float jumpHeight = 1.5f;                 
	[Header("Сила прыжка в длинну")] public float jumpIntertialForce = 10f;

	//public Transform posTarget;

	private float speed, speedSeeker;               
	private string jumpButton = "Jump";
	private int jumpBool;                           
	private int groundedBool;                       
	private bool jump;                              
	private bool isColliding;                       

	void Start()
	{
		jumpBool = Animator.StringToHash("Jump");
		groundedBool = Animator.StringToHash("Grounded");

		behaviourManager.GetAnim.SetBool(groundedBool, true);
		behaviourManager.SubscribeBehaviour(this);
		behaviourManager.RegisterDefaultBehaviour(this.behaviourCode);

		speedSeeker = runSpeed;
	}

	void Update()
	{
		// Отслеживание нажатия клавиши прыжка
		if (!jump && Input.GetButtonDown(jumpButton) && behaviourManager.IsCurrentBehaviour(this.behaviourCode) && !behaviourManager.IsOverriding())
		{
			jump = true;
		}
	}


	public override void LocalFixedUpdate()
	{
		MovementManagement(behaviourManager.GetH, behaviourManager.GetV);

		//Возможность прыжка, отключено по заявкам
		//JumpManagement();
	}

	/// <summary>
	/// Реализация прыжка
	/// </summary>
	void JumpManagement()
	{
		// Начинаем новый прыжок
		if (jump && !behaviourManager.GetAnim.GetBool(jumpBool) && behaviourManager.IsGrounded())
		{
			behaviourManager.LockTempBehaviour(this.behaviourCode);
			behaviourManager.GetAnim.SetBool(jumpBool, true);
			// Обработка только прыжков с разбега
			if (behaviourManager.GetAnim.GetFloat(speedFloat) > 0.1)
			{
				// Временно меняем трение игрока для более плавного прыжка
				GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
				GetComponent<CapsuleCollider>().material.staticFriction = 0f;
				// Вертикальную скорость лучше убрать, чтобы не было супер прыжков
				RemoveVerticalVelocity();
				// И устанавливаем ее сами
				float velocity = 2f * Mathf.Abs(Physics.gravity.y) * jumpHeight;
				velocity = Mathf.Sqrt(velocity);
				// Прыгаем
				behaviourManager.GetRigidBody.AddForce(Vector3.up * velocity, ForceMode.VelocityChange);
			}
		}
		// Уже в прыжке
		else if (behaviourManager.GetAnim.GetBool(jumpBool))
		{
			// Продолжаем движение вперед
			if (!behaviourManager.IsGrounded() && !isColliding && behaviourManager.GetTempLockStatus())
			{
				behaviourManager.GetRigidBody.AddForce(transform.forward * jumpIntertialForce * Physics.gravity.magnitude * sprintSpeed, ForceMode.Acceleration);
			}
			// Обработка приземления
			if ((behaviourManager.GetRigidBody.velocity.y < 0) && behaviourManager.IsGrounded())
			{
				behaviourManager.GetAnim.SetBool(groundedBool, true);
				// Возвращаем дефолтное трение
				GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
				GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
				// Завершаем прыжок
				jump = false;
				behaviourManager.GetAnim.SetBool(jumpBool, false);
				behaviourManager.UnlockTempBehaviour(this.behaviourCode);
			}
		}
	}

	/// <summary>
	/// Реализация движения
	/// </summary>
	void MovementManagement(float horizontal, float vertical)
	{
		// Используем гравитацию, если на земле
		if (behaviourManager.IsGrounded())
			behaviourManager.GetRigidBody.useGravity = true;

		// Движение только по плоскости
		else if (!behaviourManager.GetAnim.GetBool(jumpBool) && behaviourManager.GetRigidBody.velocity.y > 0)
		{
			RemoveVerticalVelocity();
		}

		// Крутим модельку
		Rotating(horizontal, vertical);

		// Устанавливаем скорость
		Vector2 dir = new Vector2(horizontal, vertical);
		speed = Vector2.ClampMagnitude(dir, 1f).magnitude;
		// Установка скорости бега через кручение колесика мышки, почему бы и нет
		speedSeeker += Input.GetAxis("Mouse ScrollWheel");
		speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, runSpeed);
		speed *= speedSeeker;
		if (behaviourManager.IsSprinting())
		{
			speed = sprintSpeed;
		}

		behaviourManager.GetAnim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
	}

	/// <summary>
	/// Отключение вертикального перемещения
	/// </summary>
	private void RemoveVerticalVelocity()
	{
		Vector3 horizontalVelocity = behaviourManager.GetRigidBody.velocity;
		horizontalVelocity.y = 0;
		behaviourManager.GetRigidBody.velocity = horizontalVelocity;
	}

	/// <summary>
	/// Поворот персонажа, исходя из нажатия кнопок и мыши
	/// </summary>
	Vector3 Rotating(float horizontal, float vertical)
	{
        Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);
        forward.y = 0.0f;
        forward = forward.normalized;

        // Вычисление направления
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 targetDirection;
        targetDirection = forward * vertical + right * horizontal;

        // Плавный поворот
        if ((behaviourManager.IsMoving() && targetDirection != Vector3.zero))
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, behaviourManager.turnSmoothing);
            behaviourManager.GetRigidBody.MoveRotation(newRotation);
            behaviourManager.SetLastDirection(targetDirection);
        }

        if (!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
        {
            behaviourManager.Repositioning();
        }


        return targetDirection;
	}

	private void OnCollisionStay(Collision collision)
	{
		isColliding = true;
		// Вертикальное препятствие
		if (behaviourManager.IsCurrentBehaviour(this.GetBehaviourCode()) && collision.GetContact(0).normal.y <= 0.1f)
		{
			float vel = behaviourManager.GetAnim.velocity.magnitude;
			Vector3 tangentMove = Vector3.ProjectOnPlane(transform.forward, collision.GetContact(0).normal).normalized * vel;
			behaviourManager.GetRigidBody.AddForce(tangentMove, ForceMode.VelocityChange);
		}

	}
	private void OnCollisionExit(Collision collision)
	{
		isColliding = false;
	}

	public override void StopAction()
	{
		behaviourManager.GetAnim.SetFloat(speedFloat, 0, speedDampTime, Time.deltaTime);
	}
}
