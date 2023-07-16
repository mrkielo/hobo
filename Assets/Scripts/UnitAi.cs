using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAi : MonoBehaviour
{
	Unit unit;
	UnitMovement movement;
	GameResourcesList destinationBuilding = null;

	public GameResourceSO extractionResource;
	public GameResourceSO productionResource;



	enum BuildingType
	{
		Extraction,
		Production,
		Warehouse,
		Error
	}

	void Start()
	{
		unit = GetComponent<Unit>();
		movement = GetComponent<UnitMovement>();
		Debug.LogWarning(unit.GetClosestBuilding<ExtractionBuilding>());
		StartCoroutine(WaitForBuilding<ExtractionBuilding>());
	}

	void GoToBuilding<T>()
	{
		destinationBuilding = unit.GetClosestBuilding<T>();
		movement.Move(destinationBuilding.transform.position);
	}

	BuildingType GetBuildingType(GameResourcesList building)
	{
		if (building == null) return BuildingType.Error;
		if (building.GetComponent<ProductionBuilding>() != null) return BuildingType.Production;
		if (building.GetComponent<ExtractionBuilding>() != null) return BuildingType.Extraction;
		if (building.GetComponent<Warehouse>() != null) return BuildingType.Warehouse;
		return BuildingType.Error;
	}
	void Resources()
	{
		BuildingType destinationBuildingType = GetBuildingType(destinationBuilding);
		switch (destinationBuildingType)
		{
			case BuildingType.Extraction:
				StartCoroutine(WaitForExtractionResource());
				break;

			case BuildingType.Production:
				unit.DropResource(destinationBuilding, extractionResource);
				if (unit.TryGetResource(destinationBuilding, productionResource)) StartCoroutine(WaitForBuilding<Warehouse>());
				else GoToBuilding<ExtractionBuilding>();
				break;

			case BuildingType.Warehouse:
				unit.DropResource(destinationBuilding, productionResource);
				GoToBuilding<ExtractionBuilding>();
				break;
		}
	}




	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Trriger");
		if (other.GetComponent<GameResourcesList>() == destinationBuilding)
		{
			Debug.Log("CorrectBuilding");
			Resources();
		}
	}

	IEnumerator WaitForExtractionResource()
	{
		while (!unit.TryGetResource(destinationBuilding, extractionResource))
		{
			yield return null;
		}
		StartCoroutine(WaitForBuilding<ProductionBuilding>());
	}

	IEnumerator WaitForBuilding<T>()
	{
		while (unit.GetClosestBuilding<T>() == null)
		{
			yield return null;
		}
		GoToBuilding<T>();
	}




}
