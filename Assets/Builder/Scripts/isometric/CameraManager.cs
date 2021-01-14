using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
	private Vector3 positiveInfinityVector = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

	public class CameraEvent
	{
		public Vector3 point;
		public BaseItemScript baseItem;
	}

	public enum RaycastTarget
	{
		BASE_ITEM,
		GROUND
	}

	public static CameraManager instance;

	/* object refs */
	public Camera MainCamera;
	public Text debugText;

	public EventSystem EventSystem;

	//public Transform CameraClampTopLeft;
	//public Transform CameraClampBottomRight;

	/* public variables */
	public UnityAction<CameraEvent> OnItemTap;
	public UnityAction<CameraEvent> OnItemDragStart;
	public UnityAction<CameraEvent> OnItemDrag;
	public UnityAction<CameraEvent> OnItemDragStop;
	public UnityAction<CameraEvent> OnTapGround;


	/* private vars */

	private int _layerMaskBaseItemCollider;
	private int _layerMaskGroundCollider;

	private float screenRatio = Screen.width / Screen.height;
	private Vector2 _defaultTouchPos = new Vector2(9999, 9999);
	private float _minimumMoveDistanceForItemMove = 0.2f;
	private float _maxZoomFactor = 30;
	private float _minZoomFactor = 3;
	private float _clampZoomOffset = 2.0f;

	private Vector3 _tapItemStartPos;
	private Vector3 _tapGroundStartPosition;

	private bool _isTappedBaseItem;
	private bool _isDraggingBaseItem;
	private bool _isPanningScene;

	private BaseItemScript _selectedBaseItem;

	void Awake()
	{
		instance = this;
		this._layerMaskBaseItemCollider = LayerMask.GetMask("BaseItemCollider");
		this._layerMaskGroundCollider = LayerMask.GetMask("GroundCollider");
	}

	void Update()
	{
		if (this.IsUsingUI())
		{
			return;
		}

		this.UpdateBaseItemTap();
		this.UpdateBaseItemMove();
		this.UpdateGroundTap();
		this.UpdateScenePan();
		this.UpdateSceneZoom();
	}

	public bool IsUsingUI()
	{
		if (this._isDraggingBaseItem)
		{
			return false;
		}

		if (_isPanningSceneStarted)
		{
			return false;
		}

		return (EventSystem.IsPointerOverGameObject() || EventSystem.IsPointerOverGameObject(0));
	}


	private void _GetTouches(out int touchCount, out Vector2 touch0, out Vector2 touch1)
	{
		touchCount = 0;
		touch0 = _defaultTouchPos;
		touch1 = _defaultTouchPos;

		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			touchCount = Input.touchCount;
			touch0 = Input.GetTouch(0).position;
		}
		else
		{
			if (Input.GetMouseButton(0))
			{
				touchCount = 1;
				touch0 = Input.mousePosition;
			}
			else
			{
				touchCount = 0;
				touch0 = _defaultTouchPos;
			}
		}
	}

	private BaseItemScript _TryGetRaycastHitBaseItem(Vector2 touch)
	{
		RaycastHit hit;
		if (_TryGetRaycastHit(touch, out hit, RaycastTarget.BASE_ITEM))
		{
			return hit.collider.gameObject.GetComponent<BaseItemScript>();
		}

		return null;
	}

	private Vector3 _TryGetRaycastHitBaseGround(Vector2 touch)
	{
		RaycastHit hit;
		if (_TryGetRaycastHit(touch, out hit, RaycastTarget.GROUND))
		{
			return hit.point;
		}
		return positiveInfinityVector;
	}

	private bool _TryGetRaycastHit(Vector2 touch, out RaycastHit hit, RaycastTarget target)
	{
		Ray ray = MainCamera.ScreenPointToRay(touch);
		return (Physics.Raycast(ray, out hit, 1000, (target == RaycastTarget.BASE_ITEM) ? _layerMaskBaseItemCollider : _layerMaskGroundCollider));
	}

	public void UpdateBaseItemTap()
	{
		if (!Input.GetMouseButtonUp(0))
		{
			return;
		}


		if (this._isPanningSceneStarted)
		{
			return;
		}

		if (this._isDraggingBaseItem)
		{
			return;
		}

		if (this.IsUsingUI())
		{
			return;
		}

		BaseItemScript baseItemTapped = this._TryGetRaycastHitBaseItem(Input.mousePosition);
		if (baseItemTapped != null)
		{
			this._isTappedBaseItem = true;

			this._selectedBaseItem = baseItemTapped;

			CameraEvent evt = new CameraEvent()
			{
				baseItem = baseItemTapped
			};
			if (this.OnItemTap != null)
			{
				this.OnItemTap.Invoke(evt);
			}


		}
		else
		{
			this._isTappedBaseItem = false;
			this._selectedBaseItem = null;
		}
	}

	public void UpdateGroundTap()
	{
		if (this._isTappedBaseItem)
		{
			return;
		}

		if (this._isDraggingBaseItem)
		{
			return;
		}

		if (this._isPanningScene)
		{
			return;
		}

		if (this._isPanningSceneStarted)
		{
			return;
		}

		if (!Input.GetMouseButtonUp(0))
		{
			return;
		}

		Vector3 tapPosition = this._TryGetRaycastHitBaseGround(Input.mousePosition);
		if (tapPosition != positiveInfinityVector)
		{
			//			Debug.Log ("GroundTap");
			CameraEvent evt = new CameraEvent()
			{
				point = tapPosition
			};
			if (this.OnTapGround != null)
			{
				this.OnTapGround.Invoke(evt);
			}
		}
	}

	private BaseItemScript _tapStartRaycastedItem = null;
	private bool _isDragItemStarted;
	private bool _baseItemMoved;
	public void UpdateBaseItemMove()
	{

		if (Input.GetMouseButtonDown(0))
		{
			this._tapItemStartPos = this._TryGetRaycastHitBaseGround(Input.mousePosition);
			this._tapStartRaycastedItem = this._TryGetRaycastHitBaseItem(Input.mousePosition);
			this._isDraggingBaseItem = false;
			this._isDragItemStarted = false;
		}


		if (Input.GetMouseButton(0) && this._tapItemStartPos != positiveInfinityVector)
		{
			if (this._isTappedBaseItem && this._selectedBaseItem == this._tapStartRaycastedItem)
			{
				Vector3 currentTapPosition = this._TryGetRaycastHitBaseGround(Input.mousePosition);
				if (Vector3.Distance(this._tapItemStartPos, currentTapPosition) >= _minimumMoveDistanceForItemMove)
				{

					CameraEvent evt = new CameraEvent()
					{
						point = currentTapPosition,
						baseItem = this._selectedBaseItem
					};

					if (!this._isDragItemStarted)
					{
						//						Debug.Log ("BaseItemDragStart");
						this._isDragItemStarted = true;
						if (this.OnItemDragStart != null)
						{
							this.OnItemDragStart.Invoke(evt);
						}
					}

					//					Debug.Log ("BaseItemDrag");
					this._isDraggingBaseItem = true;
					if (this.OnItemDrag != null)
					{
						this.OnItemDrag.Invoke(evt);
					}
				}
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			this._tapItemStartPos = positiveInfinityVector;
			if (_isDragItemStarted)
			{
				//				Debug.Log ("BaseItemDragStop");
				this._isDragItemStarted = false;
				this._isDraggingBaseItem = false;
				if (this.OnItemDragStop != null)
				{
					this.OnItemDragStop.Invoke(null);
				}
			}
		}
	}

	private int _previousTouchCount = 0;
	private bool _touchCountChanged;
	private Vector2 _touchPosition;
	private bool _canPan;

	private void _RefreshTouchValues()
	{
		this._touchCountChanged = false;
		int touchCount = 0;
		bool isInEditor = false;


		if (Input.touchCount == 0)
		{
			if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)))
			{
				//editor
				touchCount = 1;
				isInEditor = true;
			}
			else
			{
				touchCount = 0;
			}

		}
		else
		{
			if (Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				touchCount = 0;
			}
			else
			{
				touchCount = Input.touchCount;
			}
		}

		if (touchCount != this._previousTouchCount)
		{
			if (touchCount != 0)
			{
				this._touchCountChanged = true;
			}
		}

		if (isInEditor)
		{
			this._touchPosition = (Vector2)Input.mousePosition;
		}
		else
		{
			if (touchCount == 1)
			{
				this._touchPosition = Input.GetTouch(0).position;
			}
			else if (touchCount >= 2)
			{
				this._touchPosition = (Input.GetTouch(0).position + Input.GetTouch(1).position) / 2.0f;
			}
		}

		this._canPan = (touchCount > 0);

		this._previousTouchCount = touchCount;
	}

	private bool _isPanningSceneStarted;
	public void UpdateScenePan()
	{
		this._RefreshTouchValues();

		if (this._isDraggingBaseItem)
		{
			return;
		}

		if (this._touchCountChanged)
		{
			this._tapGroundStartPosition = this._TryGetRaycastHitBaseGround(this._touchPosition);
		}

		if (this._canPan)
		{
			Vector3 currentTapPosition = this._TryGetRaycastHitBaseGround(this._touchPosition);

			if (this._touchCountChanged)
			{
				CameraEvent evt = new CameraEvent()
				{
					point = currentTapPosition
				};
				this.OnChangeTouchCountScenePan(evt);
			}

			if (!this._isPanningSceneStarted && Vector3.Distance(this._tapGroundStartPosition, currentTapPosition) >= 1f)
			{
				this._isPanningSceneStarted = true;
				this._previousPanPoint = currentTapPosition;
			}

			if (this._isPanningSceneStarted)
			{
				CameraEvent evt = new CameraEvent()
				{
					point = currentTapPosition
				};

				this._isPanningScene = true;
				this.OnScenePan(evt);
			}

		}
		else
		{
			this._isPanningScene = false;

			if (this._isPanningSceneStarted)
			{
				this._isPanningSceneStarted = false;
				this.OnStopScenePan(null);
			}
		}

		if (!this._isPanningScene)
		{
			this.UpdatePanInertia();
		}
	}

	private Vector3 _touchPoint1;
	private Vector3 _touchPoint2;
	private bool _isZoomingStarted;
	private float _previousPinchDistance;
	private float _oldZoom = -1;
	public void UpdateSceneZoom()
	{

		if (this._isDraggingBaseItem)
		{
			return;
		}

		float newZoom = this.MainCamera.orthographicSize;

		//Editor
		float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
		if (scrollAmount != 0)
		{
			newZoom = newZoom - scrollAmount;
		}

		//Android
		if (Input.touchCount == 0)
		{
			this._isZoomingStarted = false;
		}

		if (Input.touchCount == 2)
		{
			_touchPoint1 = _TryGetRaycastHitBaseGround(Input.GetTouch(0).position);
			_touchPoint2 = _TryGetRaycastHitBaseGround(Input.GetTouch(1).position);
			if (!_isZoomingStarted)
			{
				this._isZoomingStarted = true;
				this._previousPinchDistance = (_touchPoint2 - _touchPoint1).magnitude;
			}
		}

		if (this._isZoomingStarted)
		{
			float _currentPinchDistance = (_touchPoint2 - _touchPoint1).magnitude;
			float delta = this._previousPinchDistance - _currentPinchDistance;
			newZoom = this.MainCamera.orthographicSize + (delta / (2 * screenRatio));
		}

		//clamp zoom
		newZoom = Mathf.Clamp(newZoom - scrollAmount, this._minZoomFactor, this._maxZoomFactor);
		if (newZoom < this._minZoomFactor + _clampZoomOffset)
		{
			newZoom = Mathf.Lerp(newZoom, this._minZoomFactor + _clampZoomOffset, Time.deltaTime * 2);

		}
		else if (newZoom > this._maxZoomFactor - _clampZoomOffset)
		{
			newZoom = Mathf.Lerp(newZoom, this._maxZoomFactor - _clampZoomOffset, Time.deltaTime * 2);
		}

		if (this._oldZoom != newZoom)
		{
			this.MainCamera.orthographicSize = newZoom;
			this.ClampCamera();
			this._oldZoom = newZoom;
		}
	}


	//panning
	private Vector3 _previousPanPoint;
	private Vector3 _panVelocity = Vector3.zero;
	public void OnChangeTouchCountScenePan(CameraEvent evt)
	{
		this._previousPanPoint = evt.point;
	}

	public void OnScenePan(CameraEvent evt)
	{
		Vector3 delta = this._previousPanPoint - evt.point;
		this.MainCamera.transform.localPosition += delta;
		this._panVelocity = delta;
		//		if(this._panVelocity.magnitude > 0.5f){
		//			this._panVelocity = this._panVelocity.normalized * 0.5f;
		//		}
		this.ClampCamera();
	}

	public void OnStopScenePan(CameraEvent evt)
	{
		//		Debug.Log ("OnStopPan");
	}

	public void UpdatePanInertia()
	{
		if (this._panVelocity.magnitude < 0.05f)
		{
			this._panVelocity = Vector3.zero;
		}
		if (this._panVelocity != Vector3.zero)
		{
			this._panVelocity = Vector3.Lerp(_panVelocity, Vector3.zero, Time.deltaTime * 2);
			this.MainCamera.transform.localPosition += this._panVelocity;
			this.ClampCamera();
		}
	}

	//clamps the camera within the scene limits, the limits can adjusted with '_CameraClampLeft' and 
	//'_CameraClampRight' components


	public void ClampCamera()
	{
		//		return;
		float worldSizePerPixel = 2 * this.MainCamera.orthographicSize / (float)Screen.height;

		//clamp camera left and top
		Vector3 leftClampScreenPos = this.MainCamera.WorldToScreenPoint(CameraBoundScript.instance.CameraClampTopLeftPosition);
		if (leftClampScreenPos.x > 0)
		{
			float deltaFactor = leftClampScreenPos.x * worldSizePerPixel;
			Vector3 delta = new Vector3(deltaFactor, 0, 0);
			delta = this.MainCamera.transform.TransformVector(delta);
			this.MainCamera.transform.localPosition += delta;
		}

		if (leftClampScreenPos.y < Screen.height)
		{
			float deltaFactor = (Screen.height - leftClampScreenPos.y) * worldSizePerPixel;
			Vector3 delta = new Vector3(-deltaFactor, 0, -deltaFactor);
			this.MainCamera.transform.localPosition += delta;
		}
		//clamp camera right and bottom
		Vector3 rightClampScreenPos = this.MainCamera.WorldToScreenPoint(CameraBoundScript.instance.CameraClampBottomRightPosition);

		if (rightClampScreenPos.x < Screen.width)
		{
			float deltaFactor = (rightClampScreenPos.x - Screen.width) * worldSizePerPixel;
			Vector3 delta = new Vector3(deltaFactor, 0, 0);
			delta = this.MainCamera.transform.TransformVector(delta);
			this.MainCamera.transform.localPosition += delta;
		}

		if (rightClampScreenPos.y > 0)
		{
			float deltaFactor = rightClampScreenPos.y * worldSizePerPixel;
			Vector3 delta = new Vector3(deltaFactor, 0, deltaFactor);
			this.MainCamera.transform.localPosition += delta;
		}
	}
    
    /* SHAKE SCRIPT */

    private float _shakeAmount = 5.0f;
	private float _shakeDuration = 1.0f;
    private bool _isShaking = false;

	private Vector3 _originalPos;
    
	public void ShakeCamera()
	{
		ShakeCamera(0.5f, 0.5f);
    }

    public void ShakeCamera(float amount, float duration)
    {
		_originalPos = transform.localPosition;
		_shakeAmount = amount;
		_shakeDuration = duration;

        if (!_isShaking) 
			StartCoroutine(_Shake());
    }


    private IEnumerator _Shake()
    {
		_isShaking = true;
		while (_shakeDuration > 0.01f)
		{
			this.transform.localPosition = _originalPos + Random.insideUnitSphere * _shakeAmount;
            _shakeDuration -= Time.deltaTime;
			yield return null;
		}

		_shakeDuration = 0f;
        this.transform.localPosition = _originalPos;

        _isShaking = false;
    }

}
