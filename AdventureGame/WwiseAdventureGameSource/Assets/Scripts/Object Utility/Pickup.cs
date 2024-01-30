////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Pickup : MonoBehaviour, IInteractable
{
	public GameObject pickupParticles;
	public bool DestroyOnPickup = false;

	[Header("Animation Properties")]
	public bool pickupAnimationOnInteract = true;
	public bool hoverEffect = true;
	public float hoverScale = 0.1f;
	public bool rotation = true;
	public float rotationSpeed = 50f;
	public bool addedToInteractManager = false;
	public bool InteractionEnabled = true;

	public bool interactionSound = true;
	[HideInInspector]
	public SphereCollider trigger;

	[Header("Wwise")]
	public AK.Wwise.Event PickUpEvent;
	[Space(15f)]
	public AK.Wwise.Switch PickupType;

	#region private variables
	private float randomOffset;
	private bool playerInTrigger;
	private GameObject outline;
	private bool inConversation = false;
	private bool isFocus = false;
	private ObjectOutline objectOutline;
	#endregion

	//Events
	public UnityEvent OnBecameFocus;
	public UnityEvent OnInteraction;

	void Start()
	{
		randomOffset = Random.Range(0, 2 * Mathf.PI);
	}

	void OnEnable()
	{
        PickupType.SetValue(gameObject);
        if (transform.childCount > 0)
		{
			Transform T = transform.Find("Outline");
			if (T != null)
			{
				outline = T.gameObject;
				objectOutline = outline.GetComponent<ObjectOutline>();
			}
		}

		if (trigger == null)
		{
			var sphereCollider = GetComponent<SphereCollider>();
			if (sphereCollider != null)
			{
				trigger = sphereCollider;
			}
			else
			{
				SphereCollider col = gameObject.AddComponent<SphereCollider>();
				trigger = col;
				col.isTrigger = true;
				col.radius = 1;
			}
		}
		if (trigger != null)
		{
			trigger.enabled = true;
		}
		else
		{
			print("You forgot a sphere trigger on object: " + this.gameObject.name);
		}

        if (!InteractionEnabled)
        {
            InteractionEnabled = true;
        }
	}

	void Update()
	{
		if (hoverEffect)
		{
			transform.position += new Vector3(0, Mathf.Sin(Time.time + randomOffset) * Time.deltaTime * hoverScale, 0);
		}
		if (rotation)
		{
			transform.Rotate(new Vector3(0, Time.deltaTime * rotationSpeed, 0));
		}

		if (InteractionManager.InteractableObjects.Count > 0 && InteractionManager.InteractableObjects[0] == gameObject)
		{
			if (!isFocus)
			{
				OnBecameFocus.Invoke();
			}
			isFocus = true;
			if (!inConversation)
			{
				if (objectOutline != null)
				{
					if (!objectOutline.isEnabled)
					{
						objectOutline.EnableOutline();
					}
				}
				else if (outline != null)
				{
					outline.SetActive(true);
				}
			}

		}
		else
		{
			isFocus = false;

			if (objectOutline != null)
			{
				if (objectOutline.isEnabled)
				{
					objectOutline.DisableOutline();
				}
			}
			else if (outline != null)
			{
				outline.SetActive(false);
			}
			inConversation = false;
		}
	}

	void OnDisable()
	{
		trigger.enabled = false;
		InteractionManager.SetCanInteract(gameObject, false);
		InteractionEnabled = false;
		addedToInteractManager = false;
		if (outline != null)
		{
			outline.SetActive(false);
			inConversation = false;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Player") && !addedToInteractManager)
		{
			if (InteractionEnabled)
			{
				InteractionManager.SetCanInteract(gameObject, true);
				addedToInteractManager = true;
			}
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.CompareTag("Player"))
		{
			if (InteractionEnabled)
			{
				InteractionManager.SetCanInteract(gameObject, false);
				addedToInteractManager = false;
				inConversation = false;
			}
		}
	}

	public void OnInteract()
	{
		if (InteractionEnabled && this.enabled)
		{
			OnInteraction.Invoke();

			if (interactionSound)
			{
				
				PickUpEvent.Post(gameObject);
			}
			if (pickupParticles != null)
			{
				GameObject p = Instantiate(pickupParticles, transform.position, Quaternion.identity) as GameObject;
				Destroy(p, 5f);
			}

			if (pickupAnimationOnInteract)
			{
				SetInteractionEnabled(false);
				if (trigger != null)
				{
					trigger.enabled = false;
				}
				InteractionManager.SetCanInteract(this.gameObject, false);
				StartCoroutine(PickupAnimation());
			}
		}
	}

	public void SetInteractionEnabled(bool enabled)
	{
		InteractionEnabled = enabled;

        if (!InteractionEnabled)
        {
            InteractionManager.SetCanInteract(gameObject, false);
            addedToInteractManager = false;
            inConversation = false;
        }
	}

    public void SetInteractionEnabledTrueWithDelay(float delay)
    {
        StartCoroutine(SetInteractionEnabledDelayed(delay));
    }

    private IEnumerator SetInteractionEnabledDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetInteractionEnabled(true);
    }

	public void SetInteractionSoundActive(bool enabled)
	{
		interactionSound = enabled;
	}

	IEnumerator PickupAnimation()
	{
		Transform target = PlayerManager.Instance.playerHead.transform;
		Vector3 origPos = transform.position;

		Vector3 origSize = transform.localScale;

		float speed = 1f;
		for (float t = 0; t < 1f; t += Time.deltaTime / speed)
		{
			float yOffsetValue = Curves.Instance.Hill.Evaluate(t) * 2f;
			transform.position = Vector3.Lerp(origPos, target.position + Vector3.zero.WithY(yOffsetValue), t);

			float scaleCurveValue = Curves.Instance.SmoothIn.Evaluate(t);
			transform.localScale = Vector3.Lerp(origSize, Vector3.zero, scaleCurveValue);
			yield return null;
		}
		transform.localScale = Vector3.zero;

		if (DestroyOnPickup)
		{
			Destroy(gameObject);
		}
	}

}