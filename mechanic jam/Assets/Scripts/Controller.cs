using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	private List<Pipe> pipes;

	private void Awake()
	{
		pipes = new List<Pipe>();
		foreach (Pipe p in FindObjectsOfType<Pipe>())
		{
			pipes.Add(p);
		}
	}
	// Update is called once per frame
	void Update()
    {
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
		{

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			int mask = LayerMask.GetMask("BB");
			if (Physics.Raycast(ray, out hit, 100f, mask))
			{
				Pipe pipe = hit.collider.gameObject.GetComponent<Pipe>();
				pipe.Rotate(Input.GetMouseButtonDown(0));
				foreach (Pipe p in pipes)
				{
					p.Empty();
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			int mask = LayerMask.GetMask("BB");
			if (Physics.Raycast(ray, out hit, 100f, mask))
			{
				Pipe pipe = hit.collider.gameObject.GetComponent<Pipe>();
				pipe.Fill();
			}
		}
    }
}
