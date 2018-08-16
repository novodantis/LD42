using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bypass : MonoBehaviour {
	// Settings
	public float maxLinkDistance = 12f;
	public Color completeColour = Color.blue;
	public Color constructionColour = Color.yellow;

	// Instance
	public bool completed;
	public List<Starsystem> nodes = new List<Starsystem>();
	public LineRenderer routeLine;
	public float worth;
	private bool updateVis;
	public Economy localEconomy;

	public void Awake(){
		localEconomy = FindObjectOfType<Economy>();

		if (localEconomy == null)
			Debug.LogError("Could not find economy!");
	}

	public void Update (){
		if (updateVis){
			if (nodes.Count > 0){
				// visualise
				Vector3[] allPoints = new Vector3[nodes.Count];

				for(int i = 0; i < nodes.Count; i++){
					allPoints[i] = nodes[i].transform.position;
				}
				routeLine.positionCount = allPoints.Length;
				routeLine.SetPositions(allPoints);
				if (completed){
					routeLine.startColor = completeColour;
					routeLine.endColor = completeColour;
				} else {
					routeLine.startColor = constructionColour;
					routeLine.endColor = constructionColour;
				}

				updateVis = false;
			}
		}
	}

	public bool Begin(Starsystem _start){
		if (_start.destroyed){
			return false;
		} else {
			// Start!
			AddNode(_start);
			return true;
		}
	}

	public bool Route(Starsystem _newNode){
		if (_newNode == nodes[nodes.Count-1]){
			Debug.LogWarning("Same Node!");
		} else if (CheckUniqueNode(_newNode)){
			if (_newNode.destroyed){
				// valid step, check distance
				if (CheckRange(_newNode.transform.position)){
					// close enought to link
					AddNode(_newNode);
					return true;
				} else {
					// too far to link
					Debug.LogWarning("Link too far: "+Vector3.Distance(_newNode.transform.position, nodes[nodes.Count-1].transform.position));
				}
				
			} else {
				if (CheckUnique(_newNode)){
					// finish if you can
					int _cost = GetCost(true);
					if (localEconomy.Pay(_cost)){
						Complete(_newNode);
						return true;
					} else {
						Debug.LogWarning("Cannot afford bypass, cost: "+_cost);
					}
				}
			}
		} else {
			// already have this node in the bypass
		}
		
		return false;
	}

	public bool CheckRange(Vector3 _target){
		// check if _target is in range to be added
		if (Vector3.Distance(_target, nodes[nodes.Count-1].transform.position) <= maxLinkDistance){
			return true;
		} else {
			return false;
		}
	}

	public int GetCost(bool _projection){
		int _nodeCount = (_projection ? nodes.Count + 1 : nodes.Count);
		int _costCalc = Mathf.RoundToInt((GlobalSettings.bypassNodeCost * _nodeCount) + (GlobalSettings.bypassCost * localEconomy.allBypasses.Length));
		Debug.Log("Cost calc: " + _nodeCount + " nodes, " + localEconomy.allBypasses.Length + " bypasses built, projected: "+_costCalc);
		return _costCalc;
	}

	public float ProjectProfit(Starsystem _lastSystem){
		return ((nodes[0].population * _lastSystem.population) / 2) * (nodes.Count + 1);
	}

	public Starsystem GetLastNode(){
		if (nodes.Count > 0){
			return nodes[nodes.Count-1];
		} else {
			return null;
		}
	}

	public bool CheckUnique(Starsystem _lastNode){
		// if we join to this, will we be duplicating?
		foreach (Bypass b in localEconomy.allBypasses){
			if (b.nodes[0] == nodes[0]){
				// same start
				if (b.GetLastNode() == _lastNode){
					// its the same path
					return false;
				}
			}
			// check inverse
			if (b.nodes[0] == _lastNode){
				// start matches end
				if (b.GetLastNode() == nodes[0]){
					// its the same path in reverse
					return false;
				}
			}
		}

		return true;
	}

	public bool CheckUniqueNode(Starsystem _node){
		foreach (Starsystem n in nodes){
			if (_node == n)
				return false;
		}
		return true;
	}

	public void Remove(bool _updateEconomy){
		// destroy Bypass
		Destroy(gameObject);
		if (_updateEconomy)
			localEconomy.Process(false);
	}

	private void Complete(Starsystem _lastNode){
		// finish route
		worth = ProjectProfit(_lastNode);
		AddNode(_lastNode);
		completed = true;
		updateVis = true;
		localEconomy.Process(false);
	}

	private void AddNode(Starsystem _node){
		nodes.Add(_node);
		updateVis = true;
	}

}
