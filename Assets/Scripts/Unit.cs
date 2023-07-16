using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

	[SerializeField] int cap = 1;

	GameResourcesList unitResourcesList;
	[SerializeField] FloatingText floatingTextPrefab;

	public bool hasDestination;

	private void Start()
	{
		unitResourcesList = GetComponent<GameResourcesList>();
	}




	public GameResourcesList GetClosestBuilding<T>()
	{
		GameResourcesList closestBuilding = null;
		List<GameResourcesList> allBuildings = new List<GameResourcesList>(FindObjectsOfType<GameResourcesList>());
		List<GameResourcesList> buildings = new List<GameResourcesList>();

		foreach (GameResourcesList building in allBuildings)
		{
			if (building.GetComponent<T>() != null) buildings.Add(building);
		}
		if (buildings.Count == 0) return null;
		if (buildings.Count == 1) return buildings[0];
		for (int i = 1; i < buildings.Count; i++)
		{
			closestBuilding = GetDistanceTo(buildings[i]) < GetDistanceTo(buildings[i - 1]) ? buildings[i] : buildings[i - 1];
		}
		Debug.Log("Closest Building: " + closestBuilding.name);
		return closestBuilding;

	}
	float GetDistanceTo(GameResourcesList building)
	{
		return Vector3.Distance(building.transform.position, transform.position);
	}

	public void DropResource(GameResourcesList buildingResources, GameResourceSO resourceSO)
	{
		unitResourcesList.TryUse(resourceSO, cap);
		buildingResources.Add(resourceSO, cap);
		var floatingText = Instantiate(floatingTextPrefab, transform.position + Vector3.up, Quaternion.identity);
		floatingText.SetText(resourceSO.resourceName + "  -" + cap);
	}

	public bool TryGetResource(GameResourcesList buildingResources, GameResourceSO resourceSO)
	{

		if (!buildingResources.TryUse(resourceSO, cap)) return false;
		unitResourcesList.Add(resourceSO, cap);
		var floatingText = Instantiate(floatingTextPrefab, transform.position + Vector3.up, Quaternion.identity);
		floatingText.SetText(resourceSO.resourceName + "  	+" + cap);
		return true;
	}
}
